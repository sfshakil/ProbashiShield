using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProbashiShield.Domain.Services.Admin.Contracts;
using ProbashiShield.Domain.ViewModels.Admin;
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

        public IActionResult Create()
        {
            return PartialView();
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

        [HttpPost]
        public async Task<IActionResult> SaveAgencyDataSet(AgencyDataSetViewModel viewModel)
        {
            var response = await _agencyDataSetsService.SaveAgencyDataSet(viewModel);
            return Json(new { success = response.Item1, message = response.Item2 });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var agencyDataSet = await _agencyDataSetsService.GetAgencyDataSetById(id.Value);
            if (agencyDataSet == null)
                return NotFound();

            return PartialView("Create", agencyDataSet);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAgencyDataSet(int id)
        {
            var response = await _agencyDataSetsService.DeleteAgencyDataSet(id);
            return Json(new { success = response.Item1, message = response.Item2 });
        }
    }
}
