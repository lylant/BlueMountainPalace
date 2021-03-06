using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlueMountainPalace.Models
{
    public class CustomerStatistics
    {
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }

        [Display(Name = "Number of Customers")]
        public int CustomerCount { get; set; }
    }
}
