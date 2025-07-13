using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodDonationSystem.Repositories.HungHK.ModelExtensions
{
    public class PaginationResult<T> where T : class
    {
        public int TotalItems { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public T Items { get; set; }


    }
}
