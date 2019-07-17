using System;
using System.Collections.Generic;
using System.Linq;
using Web.Data;
using Web.Models;

namespace Web.Models

{
    public class HolidayService
    {
        private BookingContext context;

        public HolidayService(BookingContext context)
        {
            this.context = context;
        }
        public void CreateHoliday(Holidays Holidays)
        {
            if (DateTime.Now > Holidays.StartDate)
            {
                throw new HolidayIsInPastException();
            }
            if (Holidays.EndDate > Holidays.StartDate.AddMonths(1))
            {
                throw new GreaterThanMonthException();
            }
        }
        public bool IsConflict(Booking booking)
            {
                foreach (var Holidays in context.Holidays)
                {
                    if (booking.Time > Holidays.StartDate && booking.Time < Holidays.EndDate.AddDays(1))
                    {
                    return true;
                    }
                }
            return false;
        }
           
        public IEnumerable<Holidays> GetAll()
        {
            return context.Holidays.ToList();
        }

    }
}          