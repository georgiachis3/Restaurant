using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class TableConflictViewModel
    {
        public Models.Table DeletedTable
        { get; set; }

        public IList<Booking> ConflictedBooking2
        { get; set; }
    }
}