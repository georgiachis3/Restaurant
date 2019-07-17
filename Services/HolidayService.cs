using System;
using Web.Models;

namespace Web.Data
{
    public class HolidayService
    {
        public HolidayService()
        {
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

    }
}          