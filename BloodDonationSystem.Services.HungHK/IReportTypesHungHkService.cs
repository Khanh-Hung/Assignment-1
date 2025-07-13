using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodDonationSystem.Repositories.HungHK.Models;

namespace BloodDonationSystem.Services.HungHK
{
    public interface IReportTypesHungHkService
    {
        Task<List<ReportTypesHungHk>> GetAllAsync();
        Task<ReportTypesHungHk> GetByIdAsync(int id);
        Task<int> CreateAysnc(ReportTypesHungHk create);
        Task<int> UpdateAysnc(ReportTypesHungHk update);
        Task<bool> DeleteAysnc(int id);
    }
}
