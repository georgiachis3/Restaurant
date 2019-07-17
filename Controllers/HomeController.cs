using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        BookingService bookingService;
        HolidayService holidayService;

        public HomeController(BookingContext context)
        {
            bookingService = new BookingService(context);

            bool available = bookingService.CheckingForTable();

            if (!available)
            {
                bookingService.PopulateTables();
            }

        }
        [HttpGet]


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Booking()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Menu()
        {
            return View();
        }

        [Authorize]
        public IActionResult Holidays()
        {
            return View();
        }
        public IActionResult ListofHolidays()
        {
            return View(Context.Holidays.ToList());
        }
        public IActionResult FAQs()
        {

            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult Admin()
        {
            return View(Context.Bookings.ToList());
        }
        [Authorize]
        [HttpPost]
        public IActionResult Holidays(Holidays Holidays)
        {
            try
            {
                holidayService.CreateHoliday(Holidays);
                return View(Holidays);
            }
            catch (HolidayIsInPastException)
            {
                ModelState.AddModelError(string.Empty, "This date is in the past.");
            }
            catch (GreaterThanMonthException)
            {
                ModelState.AddModelError(string.Empty, "You cannot take holiday for longer than a month");
            }
            return View(Holidays);
     
            
        }
        [HttpPost]
        public IActionResult Booking(Booking booking, DateTime date, DateTime time, Booking newBooking)
        {

            booking.Time = date + time.TimeOfDay;

            if (DateTime.Now > booking.Time)
                ModelState.AddModelError(string.Empty, "This date is in the past.");

            try
            {
                bookingService.CreateBooking(booking, newBooking);
                return View("Confirmation", booking);
            }
            catch (RestrauntClosedException)
            {
                ModelState.AddModelError(string.Empty, "The restaurant is closed with the times you have entered.");
            }
            catch (BookingOutOfRangeException)
            {
                ModelState.AddModelError(string.Empty, "You cannot book more than a year and a half in advanced.");
            }
            catch (ClosedOnSundaysException)
            {
                ModelState.AddModelError(string.Empty, "The restaurant is closed on Sundays.");
            }
            catch (NoTableException)
            {
                ModelState.AddModelError(string.Empty, "There are no tables available at this time/date");
            }
            return View(booking);
            /////////

            foreach (var Holidays in Context.Holidays)
            {
                if (booking.Time > Holidays.StartDate && booking.Time < Holidays.EndDate.AddDays(1))
                {
                    ModelState.AddModelError(string.Empty, "The restaurant is closed due to the owner being on holiday");
                    return View(booking);
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
        


