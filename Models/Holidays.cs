using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    [Table("Holidays")]
    public class Holidays
    {
         [Key]
        public int Id { get; set; }

        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate
        { get; set; }

        [Display(Name = "End Date")]
        [Required]
        public DateTime EndDate
        { get; set; }

        [Display(Name = "Reason")]
        [Required]
        [MinLength(2)]
        public String Reason
        { get; set; }

    }
}
