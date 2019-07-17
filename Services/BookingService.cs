using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;


namespace Web.Data
{
    public class BookingService
    {
        private BookingContext context;
        private Dictionary<int, TimeSpan> BookingLengthDictionary = new Dictionary<int, TimeSpan>();

        public BookingService(BookingContext context)
        {
            this.context = context;
            for (int i = 1; i <= 6; i++)
            {
                BookingLengthDictionary.Add(i, TimeSpan.FromMinutes(i * 30));
            }

        }

        public void CreateBooking(Booking booking, Booking newBooking)
        {

            var bookingLength = BookingLengthDictionary[booking.Guests];

            TimeSpan start = new TimeSpan(10, 0, 0);
            TimeSpan actualend = new TimeSpan(22, 0, 0);
            TimeSpan end = actualend - bookingLength;
            TimeSpan now = booking.Time.TimeOfDay;

            if ((now < start) || (now > end))
            {
                throw new RestrauntClosedException();
            }

            if (booking.Time > DateTime.Now.AddMonths(18))
            {
                throw new BookingOutOfRangeException();
            }

            DayOfWeek closinghours = booking.Time.DayOfWeek;
            if (closinghours == DayOfWeek.Sunday)
            {
                throw new ClosedOnSundaysException();
            }

            booking.Table = BookingService.GetTable(newBooking);


            if (booking.Table == null)
            {
                throw new NoTableException();

            }
            Context.Bookings.Add(booking);
            Context.SaveChanges();
        }

        public void PopulateTables()
        {
            //Context.Tables.Add();

            context.Tables.Add(new Table()
            {
                Chairs = 2,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 2,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 2,
                Location = Location.Roof

            });

            context.Tables.Add(new Table()
            {
                Chairs = 4,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 4,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 4,
                Location = Location.Outside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 6,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 6,
                Location = Location.Inside

            });

            context.Tables.Add(new Table()
            {
                Chairs = 6,
                Location = Location.Outside

            });

            context.SaveChanges();

        }

        Table GetTable(Booking newBooking)
        {
            var bookingLength = BookingLengthDictionary[newBooking.Guests];

            var endNewBookingTime = newBooking.Time + bookingLength;

            foreach (var table in context.Tables.Where(x => x.Location == newBooking.RequestedLocation && x.Chairs >= newBooking.Guests))
            {
                var conflictsWithExistingBooking = false;
                foreach (var existingBooking in context.Bookings.Where(x => x.Table == table))
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

        internal bool CheckingForTable()
        {
            if (context.Tables.Any())
            {
                return true;
            }
            return false;    
        }
       
    }
}
 
    

