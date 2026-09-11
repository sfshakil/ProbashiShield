using Microsoft.EntityFrameworkCore;
using ProbashiShield.DAL.UnitOfWork.Contracts;
using ProbashiShield.Database.DBEntities;
using ProbashiShield.Domain.Services.Admin.Contracts;
using ProbashiShield.Domain.ViewModels.Admin;
using ProbashiShield.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProbashiShield.Domain.Services.Admin.Concretes
{
    public class CountriesService : ICountriesService
    {
        private readonly IMasterUnitOfWork _unitOfWork;
        public CountriesService(IMasterUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ServiceResponse<List<CountryViewModel>>> GetAllCountries(DataTableSearchCriteria criteria)
        {
            var query = _unitOfWork.CountryRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(criteria.sSearch))
                query = query.Where(c => c.CountryName.Contains(criteria.sSearch) || c.CountryCode.Contains(criteria.sSearch));

            var filtered = await query.CountAsync();
            var data = await query.OrderBy(c => c.CountryName)
                .Skip(criteria.iDisplayStart).Take(criteria.iDisplayLength)
                .Select(c => new CountryViewModel { Id = (int)c.Id, CountryCode = c.CountryCode, CountryName = c.CountryName, IsActive = c.IsActive })
                .ToListAsync();

            return new ServiceResponse<List<CountryViewModel>> { TotalRecords = filtered, Data = data };
        }
        public async Task<(bool, string)> SaveCountry(CountryViewModel vm)
        {
            try
            {
                if (vm.Id > 0)
                {
                    var entity = await _unitOfWork.CountryRepository.GetByIdAsync(vm.Id);
                    if (entity == null) return (false, "Country not found");
                    entity.CountryCode = vm.CountryCode;
                    entity.CountryName = vm.CountryName;
                    entity.IsActive = vm.IsActive;
                    entity.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.CountryRepository.Update(entity);
                }
                else
                {
                    await _unitOfWork.CountryRepository.AddAsync(new Country
                    {
                        CountryCode = vm.CountryCode,
                        CountryName = vm.CountryName,
                        IsActive = vm.IsActive,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                await _unitOfWork.SaveAsync();
                return (true, "Country saved successfully");
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<CountryViewModel> GetCountryById(int id)
        {
            var c = await _unitOfWork.CountryRepository.GetByIdAsync(id);
            if (c == null) return null;
            return new CountryViewModel { Id = (int)c.Id, CountryCode = c.CountryCode, CountryName = c.CountryName, IsActive = c.IsActive };
        }

        public async Task<(bool, string)> DeleteCountry(int id)
        {
            try
            {
                var entity = await _unitOfWork.CountryRepository.GetByIdAsync(id);
                if (entity == null) return (false, "Country not found");
                _unitOfWork.CountryRepository.Remove(entity);
                await _unitOfWork.SaveAsync();
                return (true, "Country deleted successfully");
            }
            catch (Exception ex) { return (false, ex.Message); }
        }
    }

    public class JobCategoriesService : IJobCategoriesService
    {
        private readonly IMasterUnitOfWork _unitOfWork;
        public JobCategoriesService(IMasterUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ServiceResponse<List<JobCategoryViewModel>>> GetAllJobCategories(DataTableSearchCriteria criteria)
        {
            var query = _unitOfWork.JobCategoryRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(criteria.sSearch))
                query = query.Where(j => j.CategoryName.Contains(criteria.sSearch));

            var filtered = await query.CountAsync();
            var data = await query.OrderBy(j => j.CategoryName)
                .Skip(criteria.iDisplayStart).Take(criteria.iDisplayLength)
                .Select(j => new JobCategoryViewModel { Id = (int)j.Id, CategoryName = j.CategoryName, Description = j.Description, IsActive = j.IsActive })
                .ToListAsync();

            return new ServiceResponse<List<JobCategoryViewModel>> { TotalRecords = filtered, Data = data };
        }
        public async Task<(bool, string)> SaveJobCategory(JobCategoryViewModel vm)
        {
            try
            {
                if (vm.Id > 0)
                {
                    var entity = await _unitOfWork.JobCategoryRepository.GetByIdAsync(vm.Id);
                    if (entity == null) return (false, "Job category not found");
                    entity.CategoryName = vm.CategoryName;
                    entity.Description = vm.Description;
                    entity.IsActive = vm.IsActive;
                    entity.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.JobCategoryRepository.Update(entity);
                }
                else
                {
                    await _unitOfWork.JobCategoryRepository.AddAsync(new JobCategory
                    {
                        CategoryName = vm.CategoryName,
                        Description = vm.Description,
                        IsActive = vm.IsActive,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                await _unitOfWork.SaveAsync();
                return (true, "Job category saved successfully");
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<JobCategoryViewModel> GetJobCategoryById(long id)
        {
            var j = await _unitOfWork.JobCategoryRepository.GetByIdAsync(id);
            if (j == null) return null;
            return new JobCategoryViewModel { Id = (int)j.Id, CategoryName = j.CategoryName, Description = j.Description, IsActive = j.IsActive };
        }

        public async Task<(bool, string)> DeleteJobCategory(long id)
        {
            try
            {
                var entity = await _unitOfWork.JobCategoryRepository.GetByIdAsync(id);
                if (entity == null) return (false, "Job category not found");
                _unitOfWork.JobCategoryRepository.Remove(entity);
                await _unitOfWork.SaveAsync();
                return (true, "Job category deleted successfully");
            }
            catch (Exception ex) { return (false, ex.Message); }
        }
    }
    public class CountryFeeLimitsService : ICountryFeeLimitsService
    {
        private readonly IMasterUnitOfWork _unitOfWork;
        public CountryFeeLimitsService(IMasterUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ServiceResponse<List<CountryFeeLimitViewModel>>> GetAllCountryFeeLimits(DataTableSearchCriteria criteria)
        {
            var query = _unitOfWork.CountryFeeLimitRepository.GetAll()
                .Join(_unitOfWork.CountryRepository.GetAll(), f => f.CountryId, c => c.Id, (f, c) => new { f, c });

            if (!string.IsNullOrWhiteSpace(criteria.sSearch))
                query = query.Where(x => x.c.CountryName.Contains(criteria.sSearch));

            var filtered = await query.CountAsync();
            var data = await query.OrderBy(x => x.c.CountryName)
                .Skip(criteria.iDisplayStart).Take(criteria.iDisplayLength)
                .Select(x => new CountryFeeLimitViewModel
                {
                    Id = (int)x.f.Id,
                    CountryId = (int)x.f.CountryId,
                    CountryName = x.c.CountryName,
                    MaximumAllowedFee = x.f.MaximumAllowedFee,
                    EffectiveDate = (DateTime)x.f.EffectiveDate,
                    IsActive = x.f.IsActive
                }).ToListAsync();

            return new ServiceResponse<List<CountryFeeLimitViewModel>> { TotalRecords = filtered, Data = data };
        }
        public async Task<(bool, string)> SaveCountryFeeLimit(CountryFeeLimitViewModel vm)
        {
            try
            {
                if (vm.Id > 0)
                {
                    var entity = await _unitOfWork.CountryFeeLimitRepository.GetByIdAsync(vm.Id);
                    if (entity == null) return (false, "Fee limit not found");
                    entity.CountryId = vm.CountryId;
                    entity.MaximumAllowedFee = vm.MaximumAllowedFee;
                    entity.EffectiveDate = vm.EffectiveDate;
                    entity.IsActive = vm.IsActive;
                    entity.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.CountryFeeLimitRepository.Update(entity);
                }
                else
                {
                    await _unitOfWork.CountryFeeLimitRepository.AddAsync(new CountryFeeLimit
                    {
                        CountryId = vm.CountryId,
                        MaximumAllowedFee = vm.MaximumAllowedFee,
                        EffectiveDate = vm.EffectiveDate,
                        IsActive = vm.IsActive,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                await _unitOfWork.SaveAsync();
                return (true, "Fee limit saved successfully");
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<CountryFeeLimitViewModel> GetCountryFeeLimitById(long id)
        {
            var f = await _unitOfWork.CountryFeeLimitRepository.GetByIdAsync(id);
            if (f == null) return null;
            return new CountryFeeLimitViewModel { Id = f.Id, CountryId = f.CountryId, MaximumAllowedFee = f.MaximumAllowedFee, EffectiveDate = (DateTime)f.EffectiveDate, IsActive = f.IsActive };
        }

        public async Task<(bool, string)> DeleteCountryFeeLimit(long id)
        {
            try
            {
                var entity = await _unitOfWork.CountryFeeLimitRepository.GetByIdAsync(id);
                if (entity == null) return (false, "Fee limit not found");
                _unitOfWork.CountryFeeLimitRepository.Remove(entity);
                await _unitOfWork.SaveAsync();
                return (true, "Fee limit deleted successfully");
            }
            catch (Exception ex) { return (false, ex.Message); }
        }
    }

    public class SalaryReferencesService : ISalaryReferencesService
    {
        private readonly IMasterUnitOfWork _unitOfWork;
        public SalaryReferencesService(IMasterUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ServiceResponse<List<SalaryReferenceViewModel>>> GetAllSalaryReferences(DataTableSearchCriteria criteria)
        {
            var query = _unitOfWork.SalaryReferenceRepository.GetAll()
                .Join(_unitOfWork.CountryRepository.GetAll(), s => s.CountryId, c => c.Id, (s, c) => new { s, c })
                .Join(_unitOfWork.JobCategoryRepository.GetAll(), sc => sc.s.JobCategoryId, j => j.Id, (sc, j) => new { sc.s, sc.c, j });

            if (!string.IsNullOrWhiteSpace(criteria.sSearch))
                query = query.Where(x => x.c.CountryName.Contains(criteria.sSearch) || x.j.CategoryName.Contains(criteria.sSearch));

            var filtered = await query.CountAsync();
            var data = await query.OrderBy(x => x.c.CountryName)
                .Skip(criteria.iDisplayStart).Take(criteria.iDisplayLength)
                .Select(x => new SalaryReferenceViewModel
                {
                    Id = (int)x.s.Id,
                    CountryId = (int)x.s.CountryId,
                    CountryName = x.c.CountryName,
                    JobCategoryId = (int)x.s.JobCategoryId,
                    CategoryName = x.j.CategoryName,
                    MinSalary = x.s.MinSalary,
                    MaxSalary = x.s.MaxSalary,
                    Currency = x.s.Currency,
                    IsActive = x.s.IsActive
                }).ToListAsync();

            return new ServiceResponse<List<SalaryReferenceViewModel>> { TotalRecords = filtered, Data = data };
        }
        public async Task<(bool, string)> SaveSalaryReference(SalaryReferenceViewModel vm)
        {
            try
            {
                if (vm.Id > 0)
                {
                    var entity = await _unitOfWork.SalaryReferenceRepository.GetByIdAsync(vm.Id);
                    if (entity == null) return (false, "Salary reference not found");
                    entity.CountryId = vm.CountryId;
                    entity.JobCategoryId = vm.JobCategoryId;
                    entity.MinSalary = vm.MinSalary;
                    entity.MaxSalary = vm.MaxSalary;
                    entity.Currency = vm.Currency;
                    entity.IsActive = vm.IsActive;
                    entity.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.SalaryReferenceRepository.Update(entity);
                }
                else
                {
                    await _unitOfWork.SalaryReferenceRepository.AddAsync(new SalaryReference
                    {
                        CountryId = vm.CountryId,
                        JobCategoryId = vm.JobCategoryId,
                        MinSalary = vm.MinSalary,
                        MaxSalary = vm.MaxSalary,
                        Currency = vm.Currency,
                        IsActive = vm.IsActive,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                await _unitOfWork.SaveAsync();
                return (true, "Salary reference saved successfully");
            }
            catch (Exception ex) { return (false, ex.Message); }
        }

        public async Task<SalaryReferenceViewModel> GetSalaryReferenceById(long id)
        {
            var s = await _unitOfWork.SalaryReferenceRepository.GetByIdAsync(id);
            if (s == null) return null;
            return new SalaryReferenceViewModel { Id = (int)s.Id, CountryId = (int)s.CountryId, JobCategoryId = (int)s.JobCategoryId, MinSalary = s.MinSalary, MaxSalary = s.MaxSalary, Currency = s.Currency, IsActive = s.IsActive };
        }

        public async Task<(bool, string)> DeleteSalaryReference(long id)
        {
            try
            {
                var entity = await _unitOfWork.SalaryReferenceRepository.GetByIdAsync(id);
                if (entity == null) return (false, "Salary reference not found");
                _unitOfWork.SalaryReferenceRepository.Remove(entity);
                await _unitOfWork.SaveAsync();
                return (true, "Salary reference deleted successfully");
            }
            catch (Exception ex) { return (false, ex.Message); }
        }
    }
}