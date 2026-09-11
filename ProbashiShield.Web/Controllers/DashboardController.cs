using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProbashiShield.DAL.UnitOfWork.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProbashiShield.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IMasterUnitOfWork _unitOfWork;
        public DashboardController(IMasterUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<JsonResult> GetDashboardStats()
        {
            var all = await _unitOfWork.VerificationRequestRepository.GetAll()
                .Select(v => new { v.DeviceId, v.RequestedAt })
                .ToListAsync();

            bool IsMobile(string deviceId) => !string.IsNullOrEmpty(deviceId) && deviceId.Contains("Dart");

            var totalCount = all.Count;
            var mobileCount = all.Count(v => IsMobile(v.DeviceId));
            var webCount = totalCount - mobileCount;

            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Now.Date.AddDays(-6 + i))
                .Select(day => new
                {
                    label = day.ToString("MMM dd"),
                    web = all.Count(v => v.RequestedAt.Date == day && !IsMobile(v.DeviceId)),
                    mobile = all.Count(v => v.RequestedAt.Date == day && IsMobile(v.DeviceId))
                }).ToList();

            return Json(new
            {
                totalCount,
                webCount,
                mobileCount,
                trend = last7Days
            });
        }
    }
}