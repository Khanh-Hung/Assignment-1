using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodDonationSystem.Repositories.HungHK.Models;

namespace BloodDonationSystem.Services.HungHK
{
    public interface ISystemUserAccountService
    {
        Task<SystemUserAccount> GetUserAccountAsync(string username, string password);
        Task<List<SystemUserAccount>> GetAllAsync();
    }
}
