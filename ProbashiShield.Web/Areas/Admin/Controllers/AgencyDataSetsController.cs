using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProbashiShield.Domain.Services.Admin.Contracts;
using ProbashiShield.Shared.Models;
using System.Threading.Tasks;

namespace ProbashiShield.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AgencyDataSetsController : Controller
    {
        private readonly IAgencyDataSetsService _agencyDataSetsService;

        public AgencyDataSetsController(IAgencyDataSetsService agencyDataSetsService)
        {
            _agencyDataSetsService = agencyDataSetsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetAllAgencyDataSets(DataTableSearchCriteria criterias, int draw = 1)
        {
            var response = await _agencyDataSetsService.GetAllAgencyDataSets(criterias);

            return Json(new
            {
                draw = draw,
                recordsTotal = response.TotalRecords,
                recordsFiltered = response.TotalRecords,
                data = response.Data
            });
        }
    }
}
