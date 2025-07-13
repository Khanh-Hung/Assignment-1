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
    public class ReportTypesHungHkRepository : GenericRepository<ReportTypesHungHk>
    {
        public ReportTypesHungHkRepository() => _context ??= new DBContext.SE1725_PRN232_SE170547_G5_BloodDonationSystemContext();

        public ReportTypesHungHkRepository(SE1725_PRN232_SE170547_G5_BloodDonationSystemContext context)
        {
            _context = context;
        }

        public async Task<List<ReportTypesHungHk>> GetAllAsync()
        {
            return await _context.ReportTypesHungHks.ToListAsync();
        }
    }
}
