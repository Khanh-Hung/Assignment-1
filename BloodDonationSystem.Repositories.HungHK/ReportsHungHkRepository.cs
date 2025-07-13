using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodDonationSystem.Repositories.HungHK.Basic;
using BloodDonationSystem.Repositories.HungHK.DBContext;
using BloodDonationSystem.Repositories.HungHK.ModelExtensions;
using BloodDonationSystem.Repositories.HungHK.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationSystem.Repositories.HungHK
{
    public class ReportsHungHkRepository : GenericRepository<ReportsHungHk>
    {
        public ReportsHungHkRepository() => _context ??= new DBContext.SE1725_PRN232_SE170547_G5_BloodDonationSystemContext();

        public ReportsHungHkRepository(SE1725_PRN232_SE170547_G5_BloodDonationSystemContext context)
        {
            _context = context;
        }

        public async Task<List<ReportsHungHk>> GetAllAsync()
        {
            return await _context.ReportsHungHks.Include(x => x.ReportType).Include(x => x.GeneratedByNavigation).ToListAsync();
        }

        public async Task<ReportsHungHk> GetByIdAsync (int id)
        {
            var reportId = await _context.ReportsHungHks.Include(x => x.ReportType).Include(x => x.GeneratedByNavigation).FirstOrDefaultAsync(x => x.ReportHungHkid == id);

            return reportId ?? new ReportsHungHk();
        }
        
        public async Task<List<ReportsHungHk>> SearchAsync(int totalUsers, int GeneratedBy, string MostNeededBloodType)
        {
            var a = await _context.ReportsHungHks.Include(x => x.ReportType).Include(x => x.GeneratedByNavigation)
                .Where(x =>
                    (x.TotalUsers == totalUsers || totalUsers == null || totalUsers == 0)
                    && (x.GeneratedBy == GeneratedBy || GeneratedBy == null || GeneratedBy == 0)
                    && (x.MostNeededBloodType.Contains(MostNeededBloodType) || string.IsNullOrEmpty(MostNeededBloodType))
                    ).ToListAsync();
            return a ?? new List<ReportsHungHk>();
        }

        public async Task<PaginationResult<List<ReportsHungHk>>> SearchAsync(int totalUsers, int generatedBy, string mostNeededBloodType, int currentPage, int pageSize)
        {
            var termReports = await this.SearchAsync(totalUsers, generatedBy, mostNeededBloodType);

            var totalCount = termReports.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            termReports = termReports
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PaginationResult<List<ReportsHungHk>>
            {
                TotalItems = totalCount,
                TotalPage = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Items = termReports
            };

            return result;
        }

        public async Task<PaginationResult<List<ReportsHungHk>>> GetAllAsync(int currentPage, int pageSize)
        {
            var termReports = await this.GetAllAsync();

            var totalCount = termReports.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            termReports = termReports
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PaginationResult<List<ReportsHungHk>>
            {
                TotalItems = totalCount,
                TotalPage = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Items = termReports
            };

            return result;
        }
        public async Task<PaginationResult<List<ReportsHungHk>>> SearchWithPagingAsync(SearchReportsHungHkRequest request)
        {
            var termReports = await this.SearchAsync(request.TotalUsers.GetValueOrDefault(), request.GeneratedBy.GetValueOrDefault(), request.MostNeededBloodType);

            var totalCount = termReports.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize.GetValueOrDefault());

            termReports = termReports
                .Skip((request.CurrentPage.GetValueOrDefault() - 1) * request.PageSize.GetValueOrDefault())
                .Take(request.PageSize.GetValueOrDefault())
                .ToList();

            var result = new PaginationResult<List<ReportsHungHk>>
            {
                TotalItems = totalCount,
                TotalPage = totalPages,
                CurrentPage = request.CurrentPage.GetValueOrDefault(),
                PageSize = request.PageSize.GetValueOrDefault(),
                Items = termReports
            };

            return result;
        }
    }
}
