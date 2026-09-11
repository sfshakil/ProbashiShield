using ProbashiShield.Domain.ViewModels.Admin;
using ProbashiShield.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Contracts
{
    public interface IJobCategoriesService
    {
        Task<ServiceResponse<List<JobCategoryViewModel>>> GetAllJobCategories(DataTableSearchCriteria criterias);
        Task<JobCategoryViewModel> GetJobCategoryById(long id);
        Task<(bool, string)> SaveJobCategory(JobCategoryViewModel viewModel);
        Task<(bool, string)> DeleteJobCategory(long id);
    }

    public interface ICountryFeeLimitsService
    {
        Task<ServiceResponse<List<CountryFeeLimitViewModel>>> GetAllCountryFeeLimits(DataTableSearchCriteria criteria);
        Task<(bool, string)> SaveCountryFeeLimit(CountryFeeLimitViewModel vm);
        Task<CountryFeeLimitViewModel> GetCountryFeeLimitById(long id);
        Task<(bool, string)> DeleteCountryFeeLimit(long id);
    }

    public interface ISalaryReferencesService
    {
        Task<ServiceResponse<List<SalaryReferenceViewModel>>> GetAllSalaryReferences(DataTableSearchCriteria criteria);
        Task<(bool, string)> SaveSalaryReference(SalaryReferenceViewModel vm);
        Task<SalaryReferenceViewModel> GetSalaryReferenceById(long id);
        Task<(bool, string)> DeleteSalaryReference(long id);
    }
}
