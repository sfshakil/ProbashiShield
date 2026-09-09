using ProbashiShield.Domain.Models;
using ProbashiShield.Domain.Services.Admin.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;

namespace ProbashiShield.Domain.Services.Admin.Concretes
{
    public class OCRService : IOCRService
    {
        public async Task<OCRResponse> ExtractTextAsync(List<byte[]> images)
        {
            var tessDataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");

            using var engine = new TesseractEngine(
                tessDataPath,
                "eng",
                EngineMode.Default);

            var extractedText = new StringBuilder();
            decimal totalConfidence = 0;
            int pageCount = 0;

            foreach (var imageBytes in images)
            {
                using var img = Pix.LoadFromMemory(imageBytes);
                using var page = engine.Process(img);

                extractedText.AppendLine(page.GetText());

                totalConfidence += (decimal)page.GetMeanConfidence();
                pageCount++;
            }

            var fullText = extractedText.ToString();

            var response = GetParseData(fullText);

            response.OCRConfidence =
                pageCount > 0
                ? Math.Round(totalConfidence / pageCount * 100, 2)
                : 0;

            return await Task.FromResult(response);
        }

        public OCRResponse GetParseData(string text)
        {
            return new OCRResponse
            {
                AgencyName = ExtractAgencyName(text),
                LicenseNumber = ExtractLicense(text),
                DestinationCountry = ExtractCountry(text),
                JobTitle = ExtractJobTitle(text),
                Salary = ExtractSalary(text),
                SalaryCurrency = ExtractSalaryCurrency(text),
                RecruitmentFee = ExtractRecruitmentFee(text),
                RecruitmentFeeCurrency = ExtractRecruitmentFeeCurrency(text),
                FullExtractedText = text
            };
        }

        private string? ExtractAgencyName(string text)
        {
            var patterns = new[]
            {
        @"Agency Name\s*:\s*(.+)",
        @"Agency\s*:\s*(.+)"
    };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(
                    text,
                    pattern,
                    RegexOptions.IgnoreCase);

                if (match.Success)
                    return match.Groups[1].Value.Trim();
            }

            return null;
        }

        private string? ExtractLicense(string text)
        {
            var match = Regex.Match(
                text,
                @"(?:License No|BMET License No)\s*:\s*([A-Z0-9\-]+)",
                RegexOptions.IgnoreCase);

            return match.Success
                ? match.Groups[1].Value.Trim()
                : null;
        }

        private string? ExtractCountry(string text)
        {
            var patterns = new[]
            {
        @"Destination Country\s*:\s*(.+)",
        @"Country\s*:\s*(.+)"
    };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(
                    text,
                    pattern,
                    RegexOptions.IgnoreCase);

                if (match.Success)
                    return match.Groups[1].Value.Trim();
            }

            return null;
        }

        private string? ExtractJobTitle(string text)
        {
            var match = Regex.Match(
                text,
                @"Job\s*Title\s*[:\-]?\s*([^\r\n]+)",
                RegexOptions.IgnoreCase);

            return match.Success
                ? match.Groups[1].Value.Trim()
                : null;
        }

        private decimal? ExtractSalary(string text)
        {
            var match = Regex.Match(
                text,
                @"(?:Monthly Salary|Basic Salary|Salary).*?(\d[\d,]*)",
                RegexOptions.IgnoreCase);

            if (!match.Success)
                return null;

            var value = match.Groups[1]
                .Value
                .Replace(",", "");

            return decimal.TryParse(value, out var salary)
                ? salary
                : null;
        }
        private string? ExtractSalaryCurrency(string text)
        {
            var match = Regex.Match(
                text,
                @"(?:Monthly Salary|Basic Salary|Salary)\s*:\s*([A-Z]{3})",
                RegexOptions.IgnoreCase);

            return match.Success
                ? match.Groups[1].Value.ToUpper()
                : null;
        }

        private decimal? ExtractRecruitmentFee(string text)
        {
            var match = Regex.Match(
                text,
                @"Recruitment Fee[\s\S]*?(\d[\d,]*)",
                RegexOptions.IgnoreCase);

            if (!match.Success)
                return null;

            var value = match.Groups[1]
                .Value
                .Replace(",", "");

            return decimal.TryParse(value, out var fee)
                ? fee
                : null;
        }
        private string? ExtractRecruitmentFeeCurrency(string text)
        {
            var match = Regex.Match(
                text,
                @"Recruitment Fee[\s\S]*?([A-Z]{3})\s*\d",
                RegexOptions.IgnoreCase);

            return match.Success
                ? match.Groups[1].Value.ToUpper()
                : null;
        }
    }
}