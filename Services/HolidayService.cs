using System;
using System.Collections.Generic;
using System.Linq;
using Web.Data;
using Web.Models;
using Web.Services;

namespace Web.Models

{
    public class HolidayService : GenericService<Holidays>
    {
        public HolidayService(BookingContext context) : base(context)
        {
            int error = (int)HolidayBookingStatus.OK;
        }

        public override void Add(Holidays entity)
        {
            if (DateTime.Now > entity.StartDate)
            {
                int error = (int)HolidayBookingStatus.IsInPast;
            }
            if (entity.EndDate > entity.StartDate.AddMonths(1))
            {
                int error = (int)HolidayBookingStatus.GreaterThanMonth;
            }
            if (entity.EndDate < entity.StartDate)
            {
                int error = (int)HolidayBookingStatus.BeforeStart;
            }
            base.Add(entity);
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
    }

    public enum HolidayBookingStatus
    {
        OK,
        IsInPast,
        GreaterThanMonth,
        BeforeStart
    }
}          