using BloodDonationSystem.Repositories.HungHK.ModelExtensions;
using BloodDonationSystem.Repositories.HungHK.Models;
using BloodDonationSystem.Services.HungHK;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BloodDonationSystem.APIServices.BE.HungHK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsHungHkController : ControllerBase
    {
        
        private readonly IReportsHungHkService _reportsHungHkService;
        public ReportsHungHkController(IReportsHungHkService reportService)
        {
            _reportsHungHkService = reportService;
        }

        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IEnumerable<ReportsHungHk>> Get()
        {
            return await _reportsHungHkService.GetAllAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1,2")]
        public async Task<ReportsHungHk> Get(int id)
        {
            return await _reportsHungHkService.GetByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = "1,2")]
        public async Task<int> Post(ReportsHungHk post)
        {
            return await _reportsHungHkService.CreateAysnc(post);
        }

        [HttpPut]
        [Authorize(Roles = "1,2")]
        public async Task<int> Put(ReportsHungHk put)
        {
            return await _reportsHungHkService.UpdateAysnc(put);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<bool> Delete(int id)
        {
            return await _reportsHungHkService.DeleteAysnc(id);
        }

        [HttpGet("{totalUsers}/{totalDonors}/{generatedBy}")]
        [Authorize(Roles = "1,2")]
        public async Task<IEnumerable<ReportsHungHk>> Get(int totalUsers, int totalDonors, string generatedBy)
        {
            return await _reportsHungHkService.SearchAsync(totalUsers, totalDonors, generatedBy);
        }

        [HttpGet("{currentPage}/{pageSize}")]
        [Authorize(Roles = "1,2")]
        public async Task<PaginationResult<List<ReportsHungHk>>> Get(int currentPage, int pageSize)
        {
            return await _reportsHungHkService.GetAllWithPagingAsync(currentPage, pageSize);
        }

        [HttpGet("{totalUsers}/{totalDonors}/{generatedBy}/{currentPage}/{pageSize}")]
        [Authorize(Roles = "1,2")]
        public async Task<PaginationResult<List<ReportsHungHk>>> Get(int totalUsers, int totalDonors, string generatedBy, int currentPage, int pageSize)
        {
            return await _reportsHungHkService.SearchWithPagingAsync(totalUsers, totalDonors, generatedBy, currentPage, pageSize);    
        }

        [HttpPost("SearchWithPaging")]
        [Authorize(Roles = "1,2")]
        public async Task<PaginationResult<List<ReportsHungHk>>> Get(SearchReportsHungHkRequest request)
        {
            return await _reportsHungHkService.SearchWithPagingAsync(request);
        }
    }
}
