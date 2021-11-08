using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlueMountainPalace.Models
{
    public class Booking
    {
        // Primary Key
        public int ID { get; set; }

        // Foreign Key
        public int RoomID { get; set; }

        // Foreign Key
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; }

        [Display(Name = "Check-In Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckIn { get; set; }

        [Display(Name = "Check-Out Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckOut { get; set; }

        [Display(Name = "Amount Due")]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }


        // Navigation Properies
        public Room TheRoom { get; set; }
        public Customer TheCustomer { get; set; }
    }
}
