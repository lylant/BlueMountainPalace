using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlueMountainPalace.Models
{
    public class Room
    {
        // Primary Key
        public int ID { get; set; }

        [Required]
        [MaxLength(1)]
        // RegEx: must be one character of 'G', '1', '2', or '3'
        [RegularExpression(@"^[G1-3]$")]
        public string Level { get; set; }

        [Range(1, 3)]
        public int BedCount { get; set; }

        [DataType(DataType.Currency)]
        [Range(50, 300)]
        public decimal Price { get; set; }

        // Navigation Properties
        public ICollection<Booking> TheBookings { get; set; }
    }
}
