using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProbashiShield.DAL.UnitOfWork.Contracts;
using ProbashiShield.Domain.Services.Admin.Contracts;
using ProbashiShield.Domain.ViewModels.Admin;
using ProbashiShield.Shared.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
[Area("Admin")]
public class SalaryReferencesController : Controller
{
    private readonly ISalaryReferencesService _service;
    private readonly IMasterUnitOfWork _unitOfWork;
    public SalaryReferencesController(ISalaryReferencesService service, IMasterUnitOfWork unitOfWork)
    {
        _service = service;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index() => View();

    public async Task<IActionResult> Create()
    {
        await LoadDropdowns();
        return PartialView();
    }

    [HttpPost]
    public async Task<JsonResult> GetAllSalaryReferences(DataTableSearchCriteria criterias, int draw = 1)
    {
        var response = await _service.GetAllSalaryReferences(criterias);
        return Json(new { draw, recordsTotal = response.TotalRecords, recordsFiltered = response.TotalRecords, data = response.Data });
    }

    [HttpPost]
    public async Task<IActionResult> SaveSalaryReference([FromBody] SalaryReferenceViewModel viewModel)
    {
        var response = await _service.SaveSalaryReference(viewModel);
        return Json(new { success = response.Item1, message = response.Item2 });
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id == 0) return NotFound();
        var vm = await _service.GetSalaryReferenceById(id.Value);
        if (vm == null) return NotFound();
        await LoadDropdowns();
        return PartialView("Create", vm);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSalaryReference(int id)
    {
        var response = await _service.DeleteSalaryReference(id);
        return Json(new { success = response.Item1, message = response.Item2 });
    }

    private async Task LoadDropdowns()
    {
        var countries = await _unitOfWork.CountryRepository.GetAll().Where(c => c.IsActive).OrderBy(c => c.CountryName).ToListAsync();
        var categories = await _unitOfWork.JobCategoryRepository.GetAll().Where(j => j.IsActive).OrderBy(j => j.CategoryName).ToListAsync();
        ViewBag.Countries = new SelectList(countries, "Id", "CountryName");
        ViewBag.JobCategories = new SelectList(categories, "Id", "CategoryName");
    }
}