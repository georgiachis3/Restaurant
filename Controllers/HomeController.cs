using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;
using Web.Services;
using static Web.Data.BookingService;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        BookingService bookingService;
        HolidayService holidayService;
        TableService tableService;

        public HomeController(BookingContext context)
        {
            bookingService = new BookingService(context);
            holidayService = new HolidayService(context);
            tableService = new TableService(context);

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

        [HttpGet]
        public IActionResult ViewTable(int Id)
        {
            var table = tableService.Get(Id);
            if (table == null)
            {
                return NotFound();
            }
            return View(table);
        }
        [HttpPost]
        public IActionResult DeleteTable(int Id)
        {
            tableService.Delete(Id);
            return RedirectToAction("ListofTables");
        }

        [HttpGet]
        public IActionResult AddingTables()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddingTables(Table table)
        {
            tableService.Add(table);
            return View(table);
        }

        [Authorize]
        public IActionResult Holidays()
        {
            return View();
        }
        public IActionResult ListofHolidays()
        {
            return View(holidayService.GetAll());
        }
        public IActionResult ListofTables()
        {
            return View(tableService.GetAll());
        }
        public IActionResult FAQs()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult Admin()
        {
            return View(bookingService.GetAll());
        }
        [Authorize]
        [HttpPost]
        public IActionResult Holidays(Holidays holidays)
        {

            var error = holidayService.AddHolidayBooking(holidays);
            if (error == HolidayBookingStatus.OK)
            {
                return View(holidays);
            }
            if (error == HolidayBookingStatus.IsInPast)
            {
                ModelState.AddModelError(string.Empty, "This date is in the past.");
            }
            if (error == HolidayBookingStatus.GreaterThanMonth)
            {
                ModelState.AddModelError(string.Empty, "You cannot take holiday for longer than a month");
            }
            if (error == HolidayBookingStatus.BeforeStart)
            {
                ModelState.AddModelError(string.Empty, "Your holiday cannot end before it starts.");
            }
            return View(holidays);


        }
        [HttpPost]
        public IActionResult Booking(Booking booking, DateTime date, DateTime time)
        {
            booking.Time = date + time.TimeOfDay;

            if (DateTime.Now > booking.Time)
            {
                ModelState.AddModelError(string.Empty, "This date is in the past.");
            }

            var conflictsWithHolidays = holidayService.IsConflict(booking);
            if (conflictsWithHolidays)
            {
                ModelState.AddModelError(string.Empty, "The restaurant is closed with the times you have entered.");
            }
            var error2 = bookingService.AddBooking(booking);

            if (error2 == BookingStatus.BookingMade)
            {
                return View("Confirmation", booking);
            }


            if (error2 == BookingStatus.Closed)
            {
                ModelState.AddModelError(string.Empty, "The restaurant is closed with the times you have entered.");
            }
            if (error2 == BookingStatus.Future)
            {
                ModelState.AddModelError(string.Empty, "You cannot book more than a year and a half in advanced.");
            }
            if (error2 == BookingStatus.Sunday)
            {
                ModelState.AddModelError(string.Empty, "The restaurant is closed on Sundays.");
            }
            if (error2 == BookingStatus.NoTable)
            {
                ModelState.AddModelError(string.Empty, "There are no tables available at this time/date");
            }
            return View(booking);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
        


