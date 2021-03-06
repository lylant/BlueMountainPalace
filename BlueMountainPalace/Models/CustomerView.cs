using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueMountainPalace.Models
{
    public class CustomerView
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName => $"{LastName} {FirstName}";

        public string Postcode { get; set; }
    }
}
