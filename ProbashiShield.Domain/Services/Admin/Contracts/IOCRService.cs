using ProbashiShield.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Contracts;

public interface IOCRService
{
    Task<OCRResponse> ExtractTextAsync(List<byte[]> images);
}