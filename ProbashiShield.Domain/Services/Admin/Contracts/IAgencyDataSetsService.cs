using ProbashiShield.Domain.ViewModels.Admin;
using ProbashiShield.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Contracts
{
    public interface IAgencyDataSetsService
    {
        Task<ServiceResponse<List<AgencyDataSetViewModel>>> GetAllAgencyDataSets(DataTableSearchCriteria criterias);
        Task<AgencyDataSetViewModel> GetAgencyDataSetById(int id);
        Task<(bool, string)> SaveAgencyDataSet(AgencyDataSetViewModel viewModel);
        Task<(bool, string)> DeleteAgencyDataSet(int id);
    }
}
