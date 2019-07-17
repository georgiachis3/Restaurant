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
        /*public BookingContext Context { get; set; }

        public Dictionary<int, TimeSpan> BookingLengthDictionary = new Dictionary<int, TimeSpan>();*/



        public HomeController(BookingContext context)
        {
            bookingService = new BookingService();

            /*Context = context;

            for (int i = 1; i <= 6 ; i++)
            {
                BookingLengthDictionary.Add(i, TimeSpan.FromMinutes(i * 30));
            }
            */

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
            if (DateTime.Now > Holidays.StartDate)
            {
                ModelState.AddModelError(string.Empty, "This date is in the past.");
                return View(Holidays);
            }
            if (Holidays.EndDate > Holidays.StartDate.AddMonths(1))
            {
                ModelState.AddModelError(string.Empty, "You cannot take holiday for longer than a month");
                return View(Holidays);
            }


            Context.Holidays.Add(Holidays);
            Context.SaveChanges();
            return View(Holidays);
        }
        [HttpPost]
        public IActionResult Booking(Booking booking, DateTime date, DateTime time, Booking newBooking)
        {

            booking.Time = date + time.TimeOfDay;

            if (DateTime.Now > booking.Time)
                ModelState.AddModelError(string.Empty, "This date is in the past.");

            var bookingLength = BookingLengthDictionary[newBooking.Guests];

            TimeSpan start = new TimeSpan(10, 0, 0);
            TimeSpan actualend = new TimeSpan(22, 0, 0);
            TimeSpan end = actualend - bookingLength;
            TimeSpan now = booking.Time.TimeOfDay;

            if ((now < start) || (now > end))
            {
                ModelState.AddModelError(string.Empty, "The restaurant is closed with the times you have entered.");
            }

            if (booking.Time > DateTime.Now.AddMonths(18))
            {
                ModelState.AddModelError(string.Empty, "You cannot book more than a year and a half in advanced.");
            }
            DayOfWeek closinghours = booking.Time.DayOfWeek;
            if (closinghours == DayOfWeek.Sunday)
            {
                ModelState.AddModelError(string.Empty, "We are closed on Sundays");
            }

            foreach (var Holidays in Context.Holidays)
            {
                if (booking.Time > Holidays.StartDate && booking.Time < Holidays.EndDate.AddDays(1))
                {
                    ModelState.AddModelError(string.Empty, "The restaurant is closed due to the owner being on holiday");
                    return View(booking);
                }
            }


            if (ModelState.IsValid)
            {

                booking.Table = BookingService.GetTable(booking);


                if (booking.Table == null)
                {
                    ModelState.AddModelError(string.Empty, "No table available, please select another time/date.");
                    return View(booking);

                }
                Context.Bookings.Add(booking);
                Context.SaveChanges();
                return View("Confirmation", booking);
            }

            return View(booking);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        


