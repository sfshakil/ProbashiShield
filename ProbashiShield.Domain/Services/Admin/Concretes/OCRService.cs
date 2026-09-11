using ProbashiShield.Domain.Models;
using ProbashiShield.Domain.Services.Admin.Contracts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;

namespace ProbashiShield.Domain.Services.Admin.Concretes
{
    public class OCRService : IOCRService
    {
        private const decimal ConfidenceThreshold = 90;

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
                var bestResult = ProcessBestOcr(
                    engine,
                    imageBytes);

                extractedText.AppendLine(bestResult.Text);

                totalConfidence += bestResult.Confidence;
                pageCount++;
            }

            var fullText = extractedText.ToString();

            var response = GetParseData(fullText);

            response.OCRConfidence =
                pageCount > 0
                ? Math.Round(totalConfidence / pageCount, 2)
                : 0;

            return await Task.FromResult(response);
        }

        private OcrAttemptResult ProcessBestOcr(
            TesseractEngine engine,
            byte[] imageBytes)
        {
            var originalResult = RunOcr(
                engine,
                imageBytes);

            if (originalResult.Confidence >= ConfidenceThreshold)
                return originalResult;

            var grayResult = RunOcr(
                engine,
                ConvertToGrayScale(imageBytes));

            if (grayResult.Confidence >= ConfidenceThreshold)
                return grayResult;

            var binaryResult = RunOcr(
                engine,
                ConvertToBinary(imageBytes));

            return new[]
            {
            originalResult,
            grayResult,
            binaryResult
        }
            .OrderByDescending(x => x.Confidence)
            .First();
        }

        private OcrAttemptResult RunOcr(
            TesseractEngine engine,
            byte[] imageBytes)
        {
            using var img = Pix.LoadFromMemory(imageBytes);

            using var page = engine.Process(
                img,
                PageSegMode.SingleBlock);

            return new OcrAttemptResult
            {
                Text = page.GetText(),
                Confidence = (decimal)page.GetMeanConfidence() * 100
            };
        }

        private byte[] ConvertToGrayScale(byte[] imageBytes)
        {
            using var image = SixLabors.ImageSharp.Image.Load(imageBytes);

            image.Mutate(x =>
            {
                x.Grayscale();
                x.Contrast(1.5f);
            });

            using var ms = new MemoryStream();

            image.SaveAsPng(ms);

            return ms.ToArray();
        }

        private byte[] ConvertToBinary(byte[] imageBytes)
        {
            using var image = SixLabors.ImageSharp.Image.Load(imageBytes);

            image.Mutate(x =>
            {
                x.Grayscale();
                x.BinaryThreshold(0.5f);
            });

            using var ms = new MemoryStream();

            image.SaveAsPng(ms);

            return ms.ToArray();
        }

        public OCRResponse GetParseData(string text)
        {
            return new OCRResponse
            {
                AgencyName = ExtractAgencyName(text),
                LicenseNumber = ExtractLicense(text),
                DestinationCountry = ExtractCountry(text),

                Salary = ExtractSalary(text),
                SalaryCurrency = ExtractSalaryCurrency(text),

                RecruitmentFee = ExtractRecruitmentFee(text),
                RecruitmentFeeCurrency = ExtractRecruitmentFeeCurrency(text),

                JobTitle = ExtractJobTitle(text),

                FullExtractedText = text
            };
        }

        private string? ExtractAgencyName(string text)
        {
            string[] patterns =
            {
            @"Agency Name\s*:\s*([^\r\n]+)",
            @"Agency\s*:\s*([^\r\n]+)"
        };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(
                    text,
                    pattern,
                    RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    return match.Groups[1].Value.Trim();
                }
            }

            return null;
        }

        private string? ExtractLicense(string text)
        {
            var match = Regex.Match(
                text,
                @"(?:License No|BMET License No)\s*:\s*([A-Z0-9]+)",
                RegexOptions.IgnoreCase);

            if (!match.Success)
                return null;

            return NormalizeLicense(
                match.Groups[1].Value.Trim());
        }

        private string NormalizeLicense(string value)
        {
            return value
                .Replace("I", "1")
                .Replace("O", "0")
                .Replace("S", "5");
        }

        private string? ExtractCountry(string text)
        {
            string[] patterns =
            {
            @"Destination Country\s*:\s*([^\r\n]+)",
            @"Country\s*:\s*([^\r\n]+)"
        };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(
                    text,
                    pattern,
                    RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    return match.Groups[1].Value.Trim();
                }
            }

            return null;
        }

        private string? ExtractJobTitle(string text)
        {
            string[] patterns =
            {
            @"Job\s*Title\s*[:\-]?\s*([^\r\n]+)",
            @"Job\s*Tile\s*[:\-]?\s*([^\r\n]+)",
            @"Position\s*[:\-]?\s*([^\r\n]+)"
        };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(
                    text,
                    pattern,
                    RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    return match.Groups[1].Value.Trim();
                }
            }

            return null;
        }

        private decimal? ExtractSalary(string text)
        {
            var match = Regex.Match(
                text,
                @"(?:Basic Salary|Monthly Salary|Salary).*?(\d[\d,\.]*)",
                RegexOptions.IgnoreCase);

            if (!match.Success)
                return null;

            var value = match.Groups[1].Value;

            value = value
                .Replace(",", "")
                .Replace(".", "");

            return decimal.TryParse(
                value,
                out var salary)
                ? salary
                : null;
        }

        private string? ExtractSalaryCurrency(string text)
        {
            var match = Regex.Match(
                text,
                @"(?:Basic Salary|Monthly Salary|Salary)\s*:\s*([A-Z]{3})",
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

            var value = match.Groups[1].Value
                .Replace(",", "");

            return decimal.TryParse(
                value,
                out var fee)
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