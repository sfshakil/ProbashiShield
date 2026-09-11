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

            return $@"You are a recruitment-fraud reviewer for Bangladeshi migrant workers. Rule-based checks below are ALREADY VERIFIED — do not re-evaluate them, only reason about what they imply.

            VERIFIED FACTS:
            - Missing required info: {response.IsInformationMissing}
            - Agency found in BMET registry: {response.IsAgencyFound}
            - Agency currently active: {response.IsAgencyActive}
            - Agency name matches registry: {Invert(response.IsAgencyNameMissmatch)}
            - Recruitment fee exceeds legal limit: {(response.IsRecruitementFeeHigherThenCountryFeeLimit == null ? "Not checked (reference data missing)" : response.IsRecruitementFeeHigherThenCountryFeeLimit.ToString())}
            - Salary above expected max: {(response.IsSalaryHigherThenSalaryReferenceFeeLimit == null ? "Not checked (reference data missing)" : response.IsSalaryHigherThenSalaryReferenceFeeLimit.ToString())}
            - Salary below expected min: {(response.IsSalaryLowerThenSalaryReferenceFeeLimit == null ? "Not checked (reference data missing)" : response.IsSalaryLowerThenSalaryReferenceFeeLimit.ToString())}

            RULE: If either fee-limit or salary-range checks above are ""Not checked"", you MUST list that as a concern (missing reference data lowers confidence — never return risk_score 0.0 with confidence_in_assessment 0.0 together in that case).

            JOB DETAILS:
            Title: {(string.IsNullOrEmpty(ocrResult.JobTitle) ? "Missing" : ocrResult.JobTitle)}
            Destination: {(string.IsNullOrEmpty(ocrResult.DestinationCountry) ? "Missing" : ocrResult.DestinationCountry)}
            Salary: {(string.IsNullOrEmpty(ocrResult.SalaryDisplay) ? "Missing" : ocrResult.SalaryDisplay)} {ocrResult.SalaryCurrency}
            Recruitment Fee: {(string.IsNullOrEmpty(ocrResult.RecruitmentFeeDisplay) ? "Missing" : ocrResult.RecruitmentFeeDisplay)} {ocrResult.RecruitmentFeeCurrency}

            RAW OCR TEXT (for cross-checking only — do not re-verify agency/fee/salary numbers already covered above):
            {text}

            YOUR TASK — only assess what the verified facts above CANNOT cover:
            1. Vague or generic job descriptions (e.g. no specific duties, no employer contact details)
            2. Pressuring, urgent, or unusual payment language in the document
            3. Any inconsistency between the raw OCR text and the structured job details above (e.g. currency mismatch, salary figures that don't reconcile, mismatched names)

            Respond with the required JSON structure only.";
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