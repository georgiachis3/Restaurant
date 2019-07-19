using System;
using System.Collections.Generic;
using System.Linq;
using Web.Models;
using Web.Services;

namespace Web.Data
{
    public class BookingService : GenericService<Booking>
    {
    
        private Dictionary<int, TimeSpan> BookingLengthDictionary = new Dictionary<int, TimeSpan>();

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
        public BookingService(BookingContext context) : base(context)
        {
            for (int i = 1; i <= 12; i++)
            {
                BookingLengthDictionary.Add(i, TimeSpan.FromMinutes(i * 30));
            }
        }

        public BookingStatus AddBooking(Booking entity)
        {
            var bookingLength = BookingLengthDictionary[entity.Guests];

            TimeSpan start = new TimeSpan(10, 0, 0);
            TimeSpan actualend = new TimeSpan(22, 0, 0);
            TimeSpan end = actualend - bookingLength;
            TimeSpan now = entity.Time.TimeOfDay;

            if ((now < start) || (now > end))
            {
                return BookingStatus.Closed;
            }

            if (entity.Time > DateTime.Now.AddMonths(18))
            {
                return BookingStatus.Future;
            }

            DayOfWeek closinghours = entity.Time.DayOfWeek;
            if (closinghours == DayOfWeek.Sunday)
            {
                return BookingStatus.Sunday;
            }

            entity.Table = GetTable(entity);


            if (entity.Table == null)
            {
                return BookingStatus.NoTable;

            }
            base.Add(entity);
            return BookingStatus.BookingMade;
        }

        public enum BookingStatus
        {
            BookingMade,
            Closed,
            Future,
            Sunday,
            NoTable,
            OnHoliday,
            InPast
        }

    }
}
 
    

