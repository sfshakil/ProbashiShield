using ProbashiShield.Database.DBEntities;
using ProbashiShield.Domain.Models;

namespace ProbashiShield.Domain.Services.Admin.Concretes
{
    public static class FraudDetectionPromptBuilder
    {
        private const int MaxTextLength = 800;

        public static string BuildPrompt(OCRResult ocrResult, ValidationResponse response)
        {
            var text = ocrResult.FullExtractedText ?? "";
            if (text.Length > MaxTextLength)
                text = text.Substring(0, MaxTextLength) + "...";

            return $@"Recruitment fraud check. Rule checks already done (trust these):
            - Info missing: {response.IsInformationMissing}
            - Agency found/active/name-match: {response.IsAgencyFound}/{response.IsAgencyActive}/{Invert(response.IsAgencyNameMissmatch)}
            - Fee over legal limit: {(response.IsRecruitementFeeHigherThenCountryFeeLimit == null ? "Missing" : response.IsRecruitementFeeHigherThenCountryFeeLimit)}
            - Salary out of range (high/low): {(response.IsSalaryHigherThenSalaryReferenceFeeLimit == null ? "Missing" : response.IsSalaryHigherThenSalaryReferenceFeeLimit)}/{(response.IsSalaryLowerThenSalaryReferenceFeeLimit == null ? "Missing" : response.IsSalaryLowerThenSalaryReferenceFeeLimit)}
            -If Fee over legal limit and Salary out of range (high/low)are missing/blank, this itself is a concern to list. Do not return risk_score 0.0 together with confidence_in_assessment 0.0 — if uncertain, raise confidence-lowering language in ""concerns"" instead.
            Job: {(string.IsNullOrEmpty(ocrResult.JobTitle) ? "Missing": ocrResult.JobTitle)}, {(string.IsNullOrEmpty(ocrResult.DestinationCountry) ? "Missing" :ocrResult.DestinationCountry)}, salary {(string.IsNullOrEmpty(ocrResult.SalaryDisplay) ? "Missing": ocrResult.SalaryDisplay)} {(string.IsNullOrEmpty(ocrResult.SalaryCurrency) ? "": ocrResult.SalaryCurrency)}, fee {(string.IsNullOrEmpty(ocrResult.RecruitmentFeeDisplay)? "Missing": ocrResult.RecruitmentFeeDisplay)} {(string.IsNullOrEmpty(ocrResult.RecruitmentFeeCurrency)?"": ocrResult.RecruitmentFeeCurrency)}
            OCR Document text: {text}
            Only check what the rules above can't: vague job details, pressuring/urgent payment language, inconsistencies between text and fields. Respond ONLY with JSON: {{""fraud_detected"": bool, ""risk_score"": 0.0-1.0, ""concerns"": [strings], ""confidence_in_assessment"": 0.0-1.0}}";
        }

        private static string Invert(bool? flag) => flag switch
        {
            true => "false",
            false => "true",
            _ => "unknown"
        };

        public static string BuildTransatorPrompt(string text)
        {
            return $@"You are a professional English-to-Bangla translation engine.

            Translate the following English text into Bangla (Bengali script, as used in Bangladesh).
            Output ONLY the Bangla translation text. No English, no explanation, no quotes, no JSON, no <think> tags.

            Example:
            English: ""This is a test.""
            Bangla: এটি একটি পরীক্ষা।

            Now translate this:
            English:
            {text}
            ";
        }
    }
}