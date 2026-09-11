using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProbashiShield.DAL.UnitOfWork.Contracts;
using ProbashiShield.Domain.Services.Admin.Contracts;
using ProbashiShield.Domain.ViewModels.Admin;
using ProbashiShield.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
[Area("Admin")]
public class CountryFeeLimitsController : Controller
{
    private readonly ICountryFeeLimitsService _service;
    private readonly IMasterUnitOfWork _unitOfWork;
    public CountryFeeLimitsController(ICountryFeeLimitsService service, IMasterUnitOfWork unitOfWork)
    {
        _service = service;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index() => View();

    public async Task<IActionResult> Create()
    {
        await LoadCountriesDropdown();
        var vm = new CountryFeeLimitViewModel { EffectiveDate = DateTime.Today, IsActive = true };
        return PartialView(vm);
    }

    [HttpPost]
    public async Task<JsonResult> GetAllCountryFeeLimits(DataTableSearchCriteria criterias, int draw = 1)
    {
        var response = await _service.GetAllCountryFeeLimits(criterias);
        return Json(new { draw, recordsTotal = response.TotalRecords, recordsFiltered = response.TotalRecords, data = response.Data });
    }

    [HttpPost]
    public async Task<IActionResult> SaveCountryFeeLimit([FromBody] CountryFeeLimitViewModel viewModel)
    {
        var response = await _service.SaveCountryFeeLimit(viewModel);
        return Json(new { success = response.Item1, message = response.Item2 });
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id == 0) return NotFound();
        var vm = await _service.GetCountryFeeLimitById(id.Value);
        if (vm == null) return NotFound();
        await LoadCountriesDropdown();
        return PartialView("Create", vm);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCountryFeeLimit(int id)
    {
        var response = await _service.DeleteCountryFeeLimit(id);
        return Json(new { success = response.Item1, message = response.Item2 });
    }

    private async Task LoadCountriesDropdown()
    {
        var countries = await _unitOfWork.CountryRepository.GetAll().Where(c => c.IsActive).OrderBy(c => c.CountryName).ToListAsync();
        ViewBag.Countries = new SelectList(countries, "Id", "CountryName");
    }
}