using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodDonationSystem.Repositories.HungHK.ModelExtensions
{
    public class SearchRequest
    {
        public int? CurrentPage { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }

    public class SearchReportsHungHkRequest : SearchRequest
    {
        public int? TotalUsers { get; set; }
        public int? GeneratedBy { get; set; }
        public string? MostNeededBloodType { get; set; }
    }
}
