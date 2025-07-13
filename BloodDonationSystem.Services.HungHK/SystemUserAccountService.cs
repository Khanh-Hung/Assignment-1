using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodDonationSystem.Repositories.HungHK;
using BloodDonationSystem.Repositories.HungHK.Models;

namespace BloodDonationSystem.Services.HungHK
{
    public class SystemUserAccountService : ISystemUserAccountService
    {
        private readonly SystemUserAccountRepository _systemUserAccountRepository;
        public SystemUserAccountService() => _systemUserAccountRepository ??= new SystemUserAccountRepository();

        public async Task<List<SystemUserAccount>> GetAllAsync()
        {
            return await _systemUserAccountRepository.GetAllAsync();    
        }

        public async Task<SystemUserAccount> GetUserAccountAsync(string username, string password)
        {
            return await _systemUserAccountRepository.GetUserAccountAsync(username, password);
        }
    }
}
