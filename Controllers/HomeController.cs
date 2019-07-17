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
        public IActionResult ViewTable(int id)
        {
            //var table = service.GetTable(id);
            return View(table);
        }

        [HttpGet]
        public IActionResult AddingTables()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddingTables(Table table)
        {
            tableService.AddTables(table);
            return View(table);
        }

        [Authorize]
        public IActionResult Holidays()
        {
            return View();
        }
        public IActionResult ListofHolidays()
        {
            return View(bookingService.GetAll());
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
            try
            {
                holidayService.CreateHoliday(holidays);
                return View(holidays);
            }
            catch (HolidayIsInPastException)
            {
                ModelState.AddModelError(string.Empty, "This date is in the past.");
            }
            catch (GreaterThanMonthException)
            {
                ModelState.AddModelError(string.Empty, "You cannot take holiday for longer than a month");
            }
            return View(holidays);
           
            
        }
        [HttpPost]
        public IActionResult Booking(Booking booking, DateTime date, DateTime time, Booking newBooking)
        {

            booking.Time = date + time.TimeOfDay;

            if (DateTime.Now > booking.Time)
                ModelState.AddModelError(string.Empty, "This date is in the past.");

            try
            {
                var conflictsWithHolidays = holidayService.IsConflict(booking);
                if (conflictsWithHolidays)
                {
                    throw new RestrauntClosedException();
                }
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
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
        


