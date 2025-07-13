using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodDonationSystem.Repositories.HungHK.ModelExtensions;
using BloodDonationSystem.Repositories.HungHK.Models;

namespace BloodDonationSystem.Services.HungHK
{
    public interface IReportsHungHkService
    {
        Task<List<ReportsHungHk>> GetAllAsync();
        Task<PaginationResult<List<ReportsHungHk>>> GetAllWithPagingAsync(int currentPage, int pageSize);
        Task<ReportsHungHk> GetByIdAsync(int id);
        Task<List<ReportsHungHk>> SearchAsync(int totalUsers, int totalDonors, string generatedBy);
        Task<PaginationResult<List<ReportsHungHk>>> SearchWithPagingAsync(int totalUsers, int totalDonors, string generatedBy, int currentPage, int pageSize);
        Task<int> CreateAysnc(ReportsHungHk create);
        Task<int> UpdateAysnc(ReportsHungHk update);
        Task<bool> DeleteAysnc(int id);
        Task<PaginationResult<List<ReportsHungHk>>> SearchWithPagingAsync(SearchReportsHungHkRequest request);
    }
}
