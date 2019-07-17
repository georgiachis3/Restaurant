using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;


namespace Web.Data
{
    public class BookingService
    {
        public BookingContext Context { get; set; }

        public Dictionary<int, TimeSpan> BookingLengthDictionary = new Dictionary<int, TimeSpan>();

        public void CreateBooking(BookingContext context)
        {
            Context = context;

            for (int i = 1; i <= 6; i++)
            {
                BookingLengthDictionary.Add(i, TimeSpan.FromMinutes(i * 30));
            }



           Table GetTable(Booking newBooking)
            {
                var bookingLength = BookingLengthDictionary[newBooking.Guests];

                var endNewBookingTime = newBooking.Time + bookingLength;

                foreach (var table in Context.Tables.Where(x => x.Location == newBooking.RequestedLocation && x.Chairs >= newBooking.Guests))
                {
                    var conflictsWithExistingBooking = false;
                    foreach (var existingBooking in Context.Bookings.Where(x => x.Table == table))
                    {
                        var existingBookingLength = BookingLengthDictionary[existingBooking.Guests];

                        var existingBookingEndTime = existingBooking.Time + existingBookingLength;
                        //a.start < b.end && b.start < a.end;

                        var conflicts = newBooking.Time < existingBookingEndTime && existingBooking.Time < endNewBookingTime;
                        if (conflicts)
                        {
                            // Conflicts
                            conflictsWithExistingBooking = true;
                            break;
                        }
                    }
                    if (!conflictsWithExistingBooking)
                    {
                        return table;
                    }
                }
                return null;
            }
        }
    }
}
 
    

