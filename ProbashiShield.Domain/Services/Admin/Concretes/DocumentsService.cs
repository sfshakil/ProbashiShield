using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenCvSharp;
using ProbashiShield.DAL.UnitOfWork.Contracts;
using ProbashiShield.Database.DBEntities;
using ProbashiShield.Domain.Models;
using ProbashiShield.Domain.Services.Admin.Contracts;
using ProbashiShield.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Concretes
{
    public class DocumentsService : IDocumentsService
    {
        private readonly IMasterUnitOfWork _unitOfWork;
        private readonly IOCRService _oCRService;
        private readonly IOllamaService _ollamaService;
        private readonly ICurrencyService _currencyService;

        public DocumentsService(IMasterUnitOfWork unitOfWork,
            IOCRService oCRService,
            IOllamaService ollamaService,
            ICurrencyService currencyService)
        {
            _unitOfWork = unitOfWork;
            _oCRService = oCRService;
            _ollamaService = ollamaService;
            _currencyService = currencyService;
        }

        public async Task<long> UploadDocuments(VerificationRequest verificationRequest, List<DocumentFile> documents)
        {
            try
            {
                await _unitOfWork.VerificationRequestRepository.AddAsync(verificationRequest);
                await _unitOfWork.SaveAsync();
                foreach (var doc in documents)
                {
                    var document = new Document
                    {
                        VerificationRequestId = verificationRequest.Id,
                        DocumentType = doc.DocumentType,
                        OriginalFileName = doc.FileName,
                        FilePath = doc.FileData,
                        FileSize = doc.FileSize,
                        UploadedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.DocumentRepository.AddAsync(document);
                }
                await _unitOfWork.SaveAsync();
                return verificationRequest.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> OCRAnalysis(long requestId)
        {
            try
            {
                var ocrResult = await ExtractDataFromOCR(requestId);

                await _unitOfWork.OCRResultRepository.AddAsync(ocrResult);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<VerdictResult> ValidationAndAIAnalysis(long requestId)
        {
            try
            {
                OCRResult oCRResult = await _unitOfWork.OCRResultRepository.Get(_ => _.RequestId == requestId).FirstOrDefaultAsync();

                var (isValid, errorMessages, response) = await ValidateOCRData(oCRResult);

                var (aiValid, aiErrors, aiResponse, bngRecommendation) = await AssessOCRResultsWithAI(oCRResult, response);

                AIAnalysisLog aIAnalysisLog = new AIAnalysisLog
                {
                    ResultId = requestId,
                    RiskLevel = aiResponse.RiskScore.ToString(),
                    PromptText = aiResponse.prompt,
                    AIResponse = JsonConvert.SerializeObject(aiResponse),
                    CreatedAt = DateTime.UtcNow,
                };

                await _unitOfWork.AIAnalysisLogRepository.AddAsync(aIAnalysisLog);

                var verdictResult = BuildVerdict(response, aiErrors, aiResponse, bngRecommendation);
                
                VerificationResult verificationResult = new VerificationResult
                {
                    ResultId = requestId,
                    Verdict = JsonConvert.SerializeObject(verdictResult),
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.VerificationResultRepository.AddAsync(verificationResult);

                await _unitOfWork.SaveAsync();

                return verdictResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<OCRResult> ExtractDataFromOCR(long requestId)
        {
            var allDocuments = await _unitOfWork.DocumentRepository.Get(_ => _.VerificationRequestId == requestId).ToListAsync();
            var imageBytesList = new List<byte[]>();

            foreach (var image in allDocuments)
            {
                if (string.IsNullOrWhiteSpace(image.FilePath))
                    continue;

                var base64 = image.FilePath;

                if (base64.Contains(','))
                {
                    base64 = base64.Split(',')[1];
                }

                imageBytesList.Add(Convert.FromBase64String(base64));
            }

            var ocrResponse = await _oCRService.ExtractTextAsync(imageBytesList);

            if (!string.IsNullOrEmpty(ocrResponse.SalaryCurrency) && ocrResponse.SalaryCurrency != "BDT")
            {
                var currencyRate = _currencyService.GetRate(ocrResponse.SalaryCurrency);
                ocrResponse.Salary = ocrResponse.Salary * currencyRate;
                ocrResponse.SalaryCurrency = "BDT";
            }

            OCRResult oCRResult = new OCRResult
            {
                RequestId = requestId,
                AgencyName = ocrResponse.AgencyName,
                LicenseNumber = ocrResponse.LicenseNumber,
                DestinationCountry = ocrResponse.DestinationCountry,
                Salary = ocrResponse.Salary,
                RecruitmentFee = ocrResponse.RecruitmentFee,
                JobTitle = ocrResponse.JobTitle,
                OCRConfidence = ocrResponse.OCRConfidence,
                FullExtractedText = ocrResponse.FullExtractedText,
                ProcessedAt = DateTime.Now,
                SalaryCurrency = ocrResponse.SalaryCurrency,
                RecruitmentFeeCurrency = ocrResponse.RecruitmentFeeCurrency
            };

            return oCRResult;
        }

        private async Task<(bool isValid, List<string> errorMessage, ValidationResponse response)> ValidateOCRData(OCRResult oCRResult)
        {
            var errors = new List<string>();
            ValidationResponse response = new ValidationResponse();

            if (string.IsNullOrEmpty(oCRResult.LicenseNumber)
                || string.IsNullOrEmpty(oCRResult.AgencyName)
                || string.IsNullOrEmpty(oCRResult.DestinationCountry)
                || oCRResult.Salary == null
                || oCRResult.RecruitmentFee == null
                || string.IsNullOrEmpty(oCRResult.JobTitle)
                || string.IsNullOrEmpty(oCRResult.SalaryCurrency)
                || string.IsNullOrEmpty(oCRResult.RecruitmentFeeCurrency)
                )
            {
                errors.Add("Information is missing.");
                response.IsInformationMissing = true;
            }

            if (!string.IsNullOrEmpty(oCRResult.LicenseNumber))
            {
                var bmetAgency = await _unitOfWork.AgencyRepository.Get(_ => _.LicenseNumber == oCRResult.LicenseNumber).FirstOrDefaultAsync();

                if (bmetAgency == null)
                {
                    errors.Add("BMET agency not found.");
                    response.IsAgencyFound = false;
                }
                if (!string.Equals(bmetAgency.AgencyName, oCRResult.AgencyName, StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add("Agency name does not match with BMET records.");
                    response.IsAgencyNameMissmatch = true;
                }
                if (!bmetAgency.IsActive)
                {
                    errors.Add("BMET agency is not active.");
                    response.IsAgencyActive = false;
                }
            }

            var country = new Country();
            var jobCategory = new JobCategory();
            var countryFeeLimit = new CountryFeeLimit();
            var salaryReference = new SalaryReference();

            if (!string.IsNullOrEmpty(oCRResult.DestinationCountry))
            {
                country = await _unitOfWork.CountryRepository.Get(_ => _.CountryName == oCRResult.DestinationCountry).FirstOrDefaultAsync();
                if (country == null)
                {
                    errors.Add("Destination country not found.");
                    response.IsDestinationCountryFound = false;
                }

                countryFeeLimit = await _unitOfWork.CountryFeeLimitRepository.Get(_ => _.CountryId == country.Id).FirstOrDefaultAsync();
                if (countryFeeLimit == null)
                {
                    errors.Add("Country fee limit not found.");
                    response.IsCountryFeeLimitFound = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(oCRResult.RecruitmentFeeCurrency))
                    {
                        if (oCRResult.RecruitmentFeeCurrency != "BDT")
                        {
                            var ocrFeeCurrencyRate = _currencyService.GetRate(oCRResult.RecruitmentFeeCurrency);
                            oCRResult.RecruitmentFee = oCRResult.RecruitmentFee * ocrFeeCurrencyRate;
                        }

                        if (oCRResult.RecruitmentFee > countryFeeLimit.MaximumAllowedFee)
                        {
                            errors.Add("Recruitment Fee is higher than Country Recruitment Fee limit.");
                            response.IsRecruitementFeeHigherThenCountryFeeLimit = true;
                        }
                        if (oCRResult.RecruitmentFee < countryFeeLimit.MaximumAllowedFee)
                        {
                            errors.Add("Recruitment Fee is lower than Country Recruitment Fee limit.");
                            response.IsRecruitementFeeLowerThenCountryFeeLimit = true;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(oCRResult.JobTitle))
            {
                jobCategory = await _unitOfWork.JobCategoryRepository.Get(_ => _.CategoryName.Contains(oCRResult.JobTitle) || _.CategoryName == oCRResult.JobTitle).FirstOrDefaultAsync();
                if (jobCategory == null)
                {
                    errors.Add("Job category not found.");
                    response.IsJobCategoryFound = false;
                }
            }

            if (country?.Id != null && jobCategory?.Id != null)
            {
                salaryReference = await _unitOfWork.SalaryReferenceRepository.Get(_ => _.CountryId == country.Id && _.JobCategoryId == jobCategory.Id).FirstOrDefaultAsync();

                if (salaryReference == null)
                {
                    errors.Add("Salary reference not found for the given country and job category.");
                    response.IsSalaryReferenceFound = false;
                }
                else
                {
                    if (salaryReference.Currency != "BDT")
                    {
                        var currencyRate = _currencyService.GetRate(salaryReference.Currency);
                        salaryReference.MaxSalary = salaryReference.MaxSalary * currencyRate;
                        salaryReference.MinSalary = salaryReference.MinSalary * currencyRate;
                        salaryReference.Currency = "BDT";

                        if (!string.IsNullOrEmpty(oCRResult.SalaryCurrency))
                        {
                            if (oCRResult.SalaryCurrency != "BDT")
                            {
                                var ocrSalaryCurrencyRate = _currencyService.GetRate(oCRResult.SalaryCurrency);
                                oCRResult.Salary = oCRResult.Salary * ocrSalaryCurrencyRate;
                            }

                            if (oCRResult.Salary > salaryReference.MaxSalary)
                            {
                                errors.Add("Salary is higher than Maximum salary range.");
                                response.IsSalaryHigherThenSalaryReferenceFeeLimit = true;
                            }
                            if (oCRResult.Salary < salaryReference.MinSalary)
                            {
                                errors.Add("Salary is lower than Minimum salary range.");
                                response.IsSalaryLowerThenSalaryReferenceFeeLimit = true;
                            }
                        }
                    }
                }
            }

            if (errors.Any())
            {
                return (false, errors, response);
            }

            return (true, new List<string>(), response);
        }

        private async Task<(bool IsValid, List<string> ErrorMessages, OllamaVerificationResult aiResponse, string bngRecommendation)> AssessOCRResultsWithAI(OCRResult ocrResult, ValidationResponse response)
        {
            var errors = new List<string>();
            var aiAnalysis = new OllamaVerificationResult();
            var translatedRecommendation = "";
            if (_ollamaService != null)
            {
                try
                {
                    if (await _ollamaService.IsHealthyAsync())
                    {
                        var prompt = FraudDetectionPromptBuilder.BuildPrompt(ocrResult, response);
                        aiAnalysis = await _ollamaService.AnalyzeForFraud(prompt);
                        aiAnalysis.prompt = prompt;

                        ApplyAiGuardrail(aiAnalysis, ocrResult);

                        var transPrompt = FraudDetectionPromptBuilder.BuildTransatorPrompt(aiAnalysis.Recommendation);
                        translatedRecommendation = await _ollamaService.TranslateEngToBng(transPrompt);

                        if (aiAnalysis.RiskScore > 0.6m)
                        {
                            errors.Add($"AI Fraud Detection Alert: {aiAnalysis.Recommendation} (Risk Score: {aiAnalysis.RiskScore:P})");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ollama service error: {ex.Message}");
                }
            }
            return (errors.Count == 0, errors, aiAnalysis, translatedRecommendation);
        }

        private const string HighRiskAction = "টাকা প্রদানের আগে এজেন্সিকে ব্যাখ্যা দিতে বলুন অথবা বিএমইটি/রামরু-তে অভিযোগ করুন।";
        private const string CautionAction = "এগোনোর আগে এজেন্সির কাছে বিস্তারিত জানতে চান।";
        private const string VerifiedAction = "নথিটি সঠিক মনে হচ্ছে, আপনি এগোতে পারেন।";
        public enum VerificationVerdict
        {
            Verified,
            Caution,
            HighRisk
        }

        public class VerdictResult
        {
            public string Verdict { get; set; }
            public List<string> ReasonsBangla { get; set; } = new();
            public string SuggestedAction { get; set; } // proceed / ask agency to clarify / report to BMET
            public string Recommendation { get; set; } // raw
            public string BngRecommendation { get; set; } // translated recommendation in Bengali
        }

        private VerdictResult BuildVerdict(ValidationResponse response, List<string> aiErrors, OllamaVerificationResult aiResponse, string bngRecommendation)
        {
            var reasons = new List<string>();
            bool highRisk = false, caution = false;

            if (response.IsInformationMissing == true) { reasons.Add("প্রয়োজনীয় তথ্য অনুপস্থিত।"); highRisk = true; }
            if (response.IsAgencyFound == false) { reasons.Add("বিএমইটি রেজিস্ট্রিতে এই এজেন্সি পাওয়া যায়নি।"); highRisk = true; }
            if (response.IsAgencyNameMissmatch == true) { reasons.Add("এজেন্সির নাম বিএমইটি রেকর্ডের সাথে মিলছে না।"); highRisk = true; }
            if (response.IsAgencyActive == false) { reasons.Add("এই এজেন্সির লাইসেন্স বর্তমানে সক্রিয় নয়।"); highRisk = true; }
            if (response.IsRecruitementFeeHigherThenCountryFeeLimit == true) { reasons.Add("রিক্রুটমেন্ট ফি সরকার নির্ধারিত সর্বোচ্চ সীমার চেয়ে বেশি।"); highRisk = true; }

            if (response.IsDestinationCountryFound == false) { reasons.Add("গন্তব্য দেশের তথ্য পাওয়া যায়নি।"); caution = true; }
            if (response.IsCountryFeeLimitFound == false) { reasons.Add("এই দেশের জন্য সর্বোচ্চ ফি সীমা নির্ধারিত নেই।"); caution = true; }
            if (response.IsJobCategoryFound == false) { reasons.Add("চাকরির ক্যাটাগরি সনাক্ত করা যায়নি।"); caution = true; }
            if (response.IsSalaryReferenceFound == false) { reasons.Add("এই দেশ ও পদের জন্য বেতনের রেফারেন্স তথ্য পাওয়া যায়নি।"); caution = true; }
            if (response.IsSalaryHigherThenSalaryReferenceFeeLimit == true) { reasons.Add("বেতন প্রত্যাশিত সর্বোচ্চ সীমার চেয়ে বেশি — যাচাই করে দেখুন।"); caution = true; }
            if (response.IsSalaryLowerThenSalaryReferenceFeeLimit == true) { reasons.Add("বেতন প্রত্যাশিত সর্বনিম্ন সীমার চেয়ে কম — এটি সন্দেহজনক হতে পারে।"); caution = true; }
            // IsRecruitementFeeLowerThenCountryFeeLimit: informational only, not surfaced as a reason.

            bool aiHigh = aiErrors.Any(e => e.Contains("High suspicion", StringComparison.OrdinalIgnoreCase));
            foreach (var e in aiErrors) reasons.Add($"এআই বিশ্লেষণ: {e}");
            if (aiHigh) highRisk = true;
            else if (aiErrors.Any()) caution = true;

            var verdict = highRisk ? VerificationVerdict.HighRisk
                        : caution ? VerificationVerdict.Caution
                        : VerificationVerdict.Verified;

            return new VerdictResult
            {
                Verdict = verdict.ToString(),
                ReasonsBangla = reasons,
                SuggestedAction = verdict switch
                {
                    VerificationVerdict.HighRisk => HighRiskAction,
                    VerificationVerdict.Caution => CautionAction,
                    _ => VerifiedAction
                },
                BngRecommendation = bngRecommendation,
                Recommendation = aiResponse.Recommendation
            };
        }
        
        private void ApplyAiGuardrail(OllamaVerificationResult ai, OCRResult ocrResult)
        {
            bool isDegenerate = ai.RiskScore == 0.0m && ai.ConfidenceInAssessment == 0.0m;
            bool jobDataIncomplete = string.IsNullOrWhiteSpace(ocrResult.JobTitle)
                || string.IsNullOrWhiteSpace(ocrResult.DestinationCountry)
                || ocrResult.Salary is null or 0
                || ocrResult.RecruitmentFee is null or 0;

            if (isDegenerate && jobDataIncomplete)
            {
                ai.RiskScore = 0.35m;
                ai.Recommendation = "AI analysis was inconclusive because key job/salary/fee details were missing or zero — manual verification recommended.";
            }
        }
    }
}
