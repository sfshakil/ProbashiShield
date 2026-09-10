using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProbashiShield.DAL.UnitOfWork.Contracts;
using ProbashiShield.Database.DBEntities;
using ProbashiShield.Domain.Services.Admin.Contracts;
using ProbashiShield.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProbashiShield.Web.Areas.User.Controllers
{
    [Area("User")]
    public class DocumentsController : Controller
    {
        private readonly IMasterUnitOfWork _unitOfWork;
        private readonly IDocumentsService _documentsService;
        private const int PageSize = 5;

        public DocumentsController(IMasterUnitOfWork unitOfWork,
            IDocumentsService documentsService)
        {
            _unitOfWork = unitOfWork;
            _documentsService = documentsService;
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> UploadDocuments([FromBody] UploadDocumentRequest request)
        {
            try
            {
                if (request?.Documents == null || request.Documents.Count == 0)
                    return Json(new { success = false, message = "No files provided" });

                var verificationRequest = new VerificationRequest
                {
                    RequestId = Guid.NewGuid(),
                    MobileNumber = User.FindFirst("PhoneNumber")?.Value,
                    DeviceId = Request.Headers["User-Agent"].ToString(),
                    RequestedAt = DateTime.UtcNow
                };

                var result = await _documentsService.UploadDocuments(verificationRequest, request.Documents);

                if (result != null)
                {
                    return Json(new { success = true, message = "Documents uploaded successfully", verificationId = verificationRequest.Id, requestId = verificationRequest.RequestId.ToString().Substring(0, 8).ToUpper() });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to upload documents" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<JsonResult> OCRAnalysis(long requestId)
        {
            try
            {
                if (requestId == 0)
                    return Json(new { success = false, message = "No id provided" });

                var result = await _documentsService.OCRAnalysis(requestId);

                if (result == true)
                {
                    return Json(new { success = true, message = "OCR Analysis done", });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to OCR Analysis" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<JsonResult> ValidationAndAIAnalysis(long requestId)
        {
            try
            {
                if (requestId < 0)
                    return Json(new { success = false, message = "No id provided" });

                var result = await _documentsService.ValidationAndAIAnalysis(requestId);

                if (result != null)
                {
                    return Json(new { success = true, message = "Success.", result = result });
                }
                else
                {
                    return Json(new { success = false, message = "Failed." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> MyDocuments(int page = 1)
        {
            try
            {
                if (page < 1) page = 1;

                var query = _unitOfWork.VerificationRequestRepository.GetAll()
                    .OrderByDescending(v => v.RequestedAt);

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
                if (totalPages > 0 && page > totalPages) page = totalPages;

                var verifications = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;

                return View(verifications);
            }
            catch (Exception ex)
            {
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = 0;
                return View(new List<VerificationRequest>());
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetVerificationDocuments(long id)
        {
            try
            {
                var documents = await _unitOfWork.DocumentRepository
                    .Get(d => d.VerificationRequestId == id).ToListAsync();

                return Json(new { success = true, documents });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetVerificationDetails(long id)
        {
            try
            {
                var documents = await _unitOfWork.DocumentRepository
                    .Get(d => d.VerificationRequestId == id).ToListAsync();

                var ocrResult = await _unitOfWork.OCRResultRepository
                    .Get(o => o.RequestId == id).OrderByDescending(a => a.ProcessedAt).FirstOrDefaultAsync();

                var aiLog = await _unitOfWork.AIAnalysisLogRepository
                    .Get(a => a.ResultId == id).OrderByDescending(a => a.CreatedAt).FirstOrDefaultAsync();

                var verificationResult = await _unitOfWork.VerificationResultRepository
                    .Get(v => v.ResultId == id).OrderByDescending(v => v.CreatedAt).FirstOrDefaultAsync();

                return Json(new
                {
                    success = true,
                    documents = documents.Select(d => new
                    {
                        d.Id,
                        d.DocumentType,
                        d.OriginalFileName,
                        d.FileSize,
                        d.UploadedAt,
                        d.FilePath
                    }),
                    ocrResult = ocrResult == null ? null : new
                    {
                        ocrResult.AgencyName,
                        ocrResult.LicenseNumber,
                        ocrResult.DestinationCountry,
                        ocrResult.Salary,
                        ocrResult.SalaryCurrency,
                        ocrResult.RecruitmentFee,
                        ocrResult.RecruitmentFeeCurrency,
                        ocrResult.JobTitle,
                        ocrResult.OCRConfidence,
                        ocrResult.ProcessedAt
                    },
                    aiLog = aiLog == null ? null : new
                    {
                        aiLog.RiskLevel,
                        aiLog.CreatedAt
                    },
                    verdictJson = verificationResult?.Verdict
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetAllVerificationRequests()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                var length = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "10");
                var searchValue = Request.Form["sSearch"].FirstOrDefault();

                var baseQuery = _unitOfWork.VerificationRequestRepository.GetAll();
                var recordsTotal = await baseQuery.CountAsync();

                var query = baseQuery.AsQueryable();
                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    query = query.Where(v => v.RequestId.ToString().Contains(searchValue));
                }
                var recordsFiltered = await query.CountAsync();

                var data = await query
                    .OrderByDescending(v => v.RequestedAt)
                    .Skip(start).Take(length)
                    .Select(v => new
                    {
                        id = v.Id,
                        requestId = v.RequestId.ToString().Substring(0, 8).ToUpper(),
                        requestedAt = v.RequestedAt
                    })
                    .ToListAsync();

                return Json(new { draw, recordsFiltered, recordsTotal, data });
            }
            catch (Exception ex)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, data = new object[0], error = ex.Message });
            }
        }
    }
}