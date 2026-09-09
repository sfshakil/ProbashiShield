using ProbashiShield.DAL.UnitOfWork.Contracts;
using ProbashiShield.Domain.Services.Admin.Contracts;
using ProbashiShield.Domain.ViewModels.Admin;
using ProbashiShield.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Concretes
{
    public class AgencyDataSetsService : IAgencyDataSetsService
    {
        private readonly IMasterUnitOfWork _unitOfWork;

        public AgencyDataSetsService(IMasterUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<List<AgencyDataSetViewModel>>> GetAllAgencyDataSets(DataTableSearchCriteria criterias)
        {
            try
            {
                var query = _unitOfWork.AgencyRepository.GetAll();

                if (!string.IsNullOrEmpty(criterias.sSearch))
                {
                    query = query.Where(x => x.AgencyName.Contains(criterias.sSearch) || 
                                            x.LicenseNumber.Contains(criterias.sSearch) ||
                                            x.Email.Contains(criterias.sSearch));
                }

                var totalCount = query.Count();
                var agencies = query.Skip(criterias.iDisplayStart)
                                   .Take(criterias.iDisplayLength)
                                   .ToList();

                var agencyDataSets = agencies.Select(x => new AgencyDataSetViewModel
                {
                    Id = (int)x.Id,
                    LicenseNumber = x.LicenseNumber,
                    AgencyName = x.AgencyName,
                    Address = x.Address,
                    Phone = x.Phone,
                    Email = x.Email,
                    AgencyStatus = x.AgencyStatus,
                    ValidityDate = x.ValidityDate,
                    LastSyncDate = x.LastSyncDate
                }).ToList();

                return new ServiceResponse<List<AgencyDataSetViewModel>>
                {
                    IsSuccess = true,
                    Data = agencyDataSets,
                    TotalRecords = totalCount,
                    DisplayedRecords = agencyDataSets.Count
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<AgencyDataSetViewModel>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<AgencyDataSetViewModel> GetAgencyDataSetById(int id)
        {
            try
            {
                var agency = await _unitOfWork.AgencyRepository.GetByIdAsync(id);
                if (agency == null)
                    return null;

                return new AgencyDataSetViewModel
                {
                    Id = (int)agency.Id,
                    LicenseNumber = agency.LicenseNumber,
                    AgencyName = agency.AgencyName,
                    Address = agency.Address,
                    Phone = agency.Phone,
                    Email = agency.Email,
                    AgencyStatus = agency.AgencyStatus,
                    ValidityDate = agency.ValidityDate,
                    LastSyncDate = agency.LastSyncDate
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching agency data set: {ex.Message}", ex);
            }
        }

        public async Task<(bool, string)> SaveAgencyDataSet(AgencyDataSetViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                    return (false, "Agency data set cannot be null");

                if (string.IsNullOrWhiteSpace(viewModel.AgencyName))
                    return (false, "Agency name is required");

                if (string.IsNullOrWhiteSpace(viewModel.LicenseNumber))
                    return (false, "License number is required");

                var existingAgency = await _unitOfWork.AgencyRepository.GetByIdAsync(viewModel.Id);

                if (viewModel.Id == 0)
                {
                    var duplicateCheck = _unitOfWork.AgencyRepository.GetAll()
                        .FirstOrDefault(x => x.LicenseNumber == viewModel.LicenseNumber);

                    if (duplicateCheck != null)
                        return (false, "License number already exists");
                }

                return (true, "Agency data set saved successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error saving agency data set: {ex.Message}");
            }
        }

        public async Task<(bool, string)> DeleteAgencyDataSet(int id)
        {
            try
            {
                var agency = await _unitOfWork.AgencyRepository.GetByIdAsync(id);
                if (agency == null)
                    return (false, "Agency data set not found");

                await _unitOfWork.AgencyRepository.RemoveByIdAsync(id);
                await _unitOfWork.SaveAsync();
                return (true, "Agency data set deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting agency data set: {ex.Message}");
            }
        }
    }
}
