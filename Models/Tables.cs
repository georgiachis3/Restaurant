using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    [Table("Table")]
    public class Table : Identifiable
    {
    public int Chairs
    // number of chairs, time spent in restaurant
        { get; set; }
    public Location Location
    // outside or inside
        { get; set; }
    public ICollection<Booking> Bookings { get; set; }
    }

}
