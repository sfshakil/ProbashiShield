using ProbashiShield.Domain.ViewModels.Admin;
using ProbashiShield.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Contracts
{
    public interface ICountriesService
    {
        Task<ServiceResponse<List<CountryViewModel>>> GetAllCountries(DataTableSearchCriteria criterias);
        Task<CountryViewModel> GetCountryById(int id);
        Task<(bool, string)> SaveCountry(CountryViewModel viewModel);
        Task<(bool, string)> DeleteCountry(int id);
    }
}
