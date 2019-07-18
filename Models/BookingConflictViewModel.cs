using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class BookingConflictViewModel
    {
        public Models.Holidays InputtedHoliday
        { get; set; }

        public IList<Booking> ConflictedBooking
        { get; set; }
    }
}