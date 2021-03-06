using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlueMountainPalace.Models
{
    public class Customer
    {
        [Key] // Primary Key
        [Required]
        [DataType(DataType.EmailAddress)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Email { get; set; }

        [Required]
        [MinLength(2), MaxLength(20)]
        // RegEx: must start with a capital letter, then allows lower-case letters, apostrophe or hyphen
        [RegularExpression(@"^[A-Z][a-z'-]*$")]
        public string LastName { get; set; }

        [Required]
        [MinLength(2), MaxLength(20)]
        // RegEx: must start with a capital letter, then allows lower-case letters, apostrophe or hyphen
        [RegularExpression(@"^[A-Z][a-z'-]*$")]
        public string FirstName { get; set; }

        [NotMapped] // not to map this property to the database
        public string FullName => $"{LastName} {FirstName}";

        [Required]
        [MinLength(4), MaxLength(4)]
        // RegEx: only numeric characters are allowed
        [RegularExpression(@"^[0-9]*$")]
        public string Postcode { get; set; }

        // Navigation Properties
        public ICollection<Booking> TheBookings { get; set; }
    }
}
