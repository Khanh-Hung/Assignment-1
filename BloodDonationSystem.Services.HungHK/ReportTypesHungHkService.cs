using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodDonationSystem.Repositories.HungHK;
using BloodDonationSystem.Repositories.HungHK.Models;

namespace BloodDonationSystem.Services.HungHK
{
    public class ReportTypesHungHkService : IReportTypesHungHkService
    {
        private readonly ReportTypesHungHkRepository _reportTypesHungHkRepository;
        public ReportTypesHungHkService() => _reportTypesHungHkRepository ??= new ReportTypesHungHkRepository();

        public async Task<List<ReportTypesHungHk>> GetAllAsync()
        {
            return await _reportTypesHungHkRepository.GetAllAsync();
        }
        public async Task<ReportTypesHungHk> GetByIdAsync(int id)
        {
            return await _reportTypesHungHkRepository.GetByIdAsync(id);
        }
        public async Task<int> CreateAysnc(ReportTypesHungHk create)
        {
            return await _reportTypesHungHkRepository.CreateAsync(create);
        }
        public async Task<int> UpdateAysnc(ReportTypesHungHk update)
        {
            return await _reportTypesHungHkRepository.UpdateAsync(update);
        }
        public async Task<bool> DeleteAysnc(int id)
        {
            var reportId = await _reportTypesHungHkRepository.GetByIdAsync(id);
            return await _reportTypesHungHkRepository.RemoveAsync(reportId);
        }
    }
}
