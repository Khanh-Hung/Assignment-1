using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodDonationSystem.Repositories.HungHK.Basic;
using BloodDonationSystem.Repositories.HungHK.DBContext;
using BloodDonationSystem.Repositories.HungHK.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationSystem.Repositories.HungHK
{
    public class SystemUserAccountRepository : GenericRepository<SystemUserAccount>
    {
        public SystemUserAccountRepository() => _context ??= new DBContext.SE1725_PRN232_SE170547_G5_BloodDonationSystemContext();
        public SystemUserAccountRepository(SE1725_PRN232_SE170547_G5_BloodDonationSystemContext context) => _context = context;

        public async Task<SystemUserAccount> GetUserAccountAsync(string username, string password)
        {
            return await _context.SystemUserAccounts.FirstOrDefaultAsync(x=> x.UserName == username && x.Password == password && x.IsActive == true);
        }

        public async Task<List<SystemUserAccount>> GetAllAsync()
        {
            return await _context.SystemUserAccounts.ToListAsync();
        }
    }
}
