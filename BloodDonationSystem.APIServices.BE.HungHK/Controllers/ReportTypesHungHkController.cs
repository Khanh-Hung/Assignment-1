using BloodDonationSystem.Repositories.HungHK.Models;
using BloodDonationSystem.Services.HungHK;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BloodDonationSystem.APIServices.BE.HungHK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ReportTypesHungHkController : ControllerBase
    {
        private readonly IReportTypesHungHkService _reportTypesHungHkService;
        public ReportTypesHungHkController(IReportTypesHungHkService reportTypesHungHkService)
        {
            _reportTypesHungHkService = reportTypesHungHkService;
        }

        [HttpGet]
        public async Task<List<ReportTypesHungHk>> Get()
        {
            return await _reportTypesHungHkService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ReportTypesHungHk> Get(int id)
        {
            return await _reportTypesHungHkService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<int> Post(ReportTypesHungHk post)
        {
            return await _reportTypesHungHkService.CreateAysnc(post);
        }

        [HttpPut]
        public async Task<int> Put(ReportTypesHungHk put)
        {
            return await _reportTypesHungHkService.UpdateAysnc(put);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _reportTypesHungHkService.DeleteAysnc(id);
        }
    }
}
