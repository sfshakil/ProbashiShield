using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProbashiShield.Domain.Services.Admin.Contracts;
using ProbashiShield.Shared.Models;
using System.Threading.Tasks;

[Authorize]
[Area("Admin")]
public class CountriesController : Controller
{
    private readonly ICountriesService _service;
    public CountriesController(ICountriesService service) => _service = service;

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<JsonResult> GetAllCountries(DataTableSearchCriteria criterias, int draw = 1)
    {
        var response = await _service.GetAllCountries(criterias);
        return Json(new { draw, recordsTotal = response.TotalRecords, recordsFiltered = response.TotalRecords, data = response.Data });
    }
}