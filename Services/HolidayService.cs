using System;
using System.Collections.Generic;
using System.Linq;
using Web.Data;
using Web.Services;

namespace Web.Models

{
    public class HolidayService : GenericService<Holidays>
    {
        public HolidayService(BookingContext context) : base(context)
        {
        }

        public HolidayBookingStatus AddHolidayBooking(Holidays entity)
        {
            if (DateTime.Now > entity.StartDate)
            {
                return HolidayBookingStatus.IsInPast;
            }
            if (entity.EndDate > entity.StartDate.AddMonths(1))
            {
                return HolidayBookingStatus.GreaterThanMonth;
            }
            if (entity.EndDate < entity.StartDate)
            {
                return HolidayBookingStatus.BeforeStart;
            }
            base.Add(entity);
            return HolidayBookingStatus.OK;
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


        public IList<Booking> FindConflicts(Holidays conflictedholidays)
        {
            return context.Bookings.Where(x => x.Time > conflictedholidays.StartDate && x.Time < conflictedholidays.EndDate.AddDays(1)).ToList();
        }
    }

    public enum HolidayBookingStatus
    {
        OK,
        IsInPast,
        GreaterThanMonth,
        BeforeStart
    }
}          