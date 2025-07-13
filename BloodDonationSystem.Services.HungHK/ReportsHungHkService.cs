using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodDonationSystem.Repositories.HungHK;
using BloodDonationSystem.Repositories.HungHK.ModelExtensions;
using BloodDonationSystem.Repositories.HungHK.Models;

namespace BloodDonationSystem.Services.HungHK
{
    public class ReportsHungHkService : IReportsHungHkService
    {
        private readonly ReportsHungHkRepository _reportsHungHkRepository;
        public ReportsHungHkService() => _reportsHungHkRepository ??= new ReportsHungHkRepository();

        public async Task<int> CreateAysnc(ReportsHungHk create)
        {
            return await _reportsHungHkRepository.CreateAsync(create);
        }

        public async Task<bool> DeleteAysnc(int id)
        {
            var reportId = await _reportsHungHkRepository.GetByIdAsync(id);
            return await _reportsHungHkRepository.RemoveAsync(reportId);
        }

        public async Task<List<ReportsHungHk>> GetAllAsync()
        {
            return await _reportsHungHkRepository.GetAllAsync();
        }

        public async Task<PaginationResult<List<ReportsHungHk>>> GetAllWithPagingAsync(int currentPage, int pageSize)
        {
            return await _reportsHungHkRepository.GetAllAsync(currentPage, pageSize);
        }

        public async Task<ReportsHungHk> GetByIdAsync(int id)
        {
            return await _reportsHungHkRepository.GetByIdAsync(id);
        }

        public async Task<List<ReportsHungHk>> SearchAsync(int totalUsers, int totalDonors, string generatedBy)
        {
            return await _reportsHungHkRepository.SearchAsync(totalUsers, totalDonors, generatedBy);
        }

        public async Task<PaginationResult<List<ReportsHungHk>>> SearchWithPagingAsync(int totalUsers, int totalDonors, string generatedBy, int currentPage, int pageSize)
        {
            return await _reportsHungHkRepository.SearchAsync(totalUsers, totalDonors, generatedBy, currentPage, pageSize);
        }

        public async Task<PaginationResult<List<ReportsHungHk>>> SearchWithPagingAsync(SearchReportsHungHkRequest request)
        {
            return await _reportsHungHkRepository.SearchWithPagingAsync(request);
        }

        public async Task<int> UpdateAysnc(ReportsHungHk update)
        {
            return await _reportsHungHkRepository.UpdateAsync(update);
        }
    }
}
