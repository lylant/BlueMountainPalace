using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlueMountainPalace.Models
{
    public class BookingManageView
    {
        [Required]
        [Range(1, 16)]
        public int RoomID { get; set; }

        [Required]
        [Display(Name = "Customer")]
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckOut { get; set; }

        [Required]
        [Display(Name = "Amount Due")]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }
    }

}