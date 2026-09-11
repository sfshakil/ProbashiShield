using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProbashiShield.Domain.Services.Admin.Contracts;
using ProbashiShield.Domain.ViewModels.Admin;
using ProbashiShield.Shared.Models;
using System.Threading.Tasks;

[Authorize]
[Area("Admin")]
public class JobCategoriesController : Controller
{
    private readonly IJobCategoriesService _service;
    public JobCategoriesController(IJobCategoriesService service) => _service = service;

    public IActionResult Index() => View();
    public IActionResult Create() => PartialView();

    [HttpPost]
    public async Task<JsonResult> GetAllJobCategories(DataTableSearchCriteria criterias, int draw = 1)
    {
        var response = await _service.GetAllJobCategories(criterias);
        return Json(new { draw, recordsTotal = response.TotalRecords, recordsFiltered = response.TotalRecords, data = response.Data });
    }

    [HttpPost]
    public async Task<IActionResult> SaveJobCategory([FromBody] JobCategoryViewModel viewModel)
    {
        if (viewModel == null)
            return Json(new { success = false, message = "Invalid or missing form data." });

        var response = await _service.SaveJobCategory(viewModel);
        return Json(new { success = response.Item1, message = response.Item2 });
    }

    public async Task<IActionResult> Edit(long id)
    {
        if (id == null || id == 0) return NotFound();
        var vm = await _service.GetJobCategoryById(id);
        if (vm == null) return NotFound();
        return PartialView("Create", vm);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteJobCategory(long id)
    {
        var response = await _service.DeleteJobCategory(id);
        return Json(new { success = response.Item1, message = response.Item2 });
    }
}