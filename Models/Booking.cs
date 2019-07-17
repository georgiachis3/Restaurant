using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    [Table("Booking")]
    public class Booking : Identifiable

    {
        [Display(Name = "First Name")]
        [Required]
        [MinLength(1)]
        public string FirstName
        { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [MinLength(1)]
        public string LastName
        { get; set; }
        public Table Table
        { get; set; }
        public DateTime Time
        { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber
        { get; set; }

        [Display(Name = "Guests")]
        [Required]
        public int Guests
        { get; set; }

        public Location RequestedLocation
        { get; set; }

        public string EmailAddress
        { get; set; }

        public string FAQs
        { get; set; }

        
        public override string ToString()
        {
            return $"{Time} on Table {Table} which is {Table.Location}";
        }
    }
}