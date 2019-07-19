using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;
using Web.Services;
using static Web.Data.BookingService;

namespace Web.Controllers
{
    
    public class BookingController : BaseController<Booking>
    {
        
        private Dictionary<BookingStatus, string> BookingErrorMessageLookup = new Dictionary<BookingStatus, string>()
        {
            {BookingStatus.Closed, "The restaurant is closed with the times you have entered." },
            {BookingStatus.Future, "You cannot book more than a year and a half in advanced." },
            {BookingStatus.NoTable, "There are no tables available at this time/date" },
            {BookingStatus.Sunday, "The restaurant is closed on Sundays." },
            {BookingStatus.OnHoliday, "The restaurant is closed as the owner is on holiday." },
            {BookingStatus.InPast, "This date is in the past." },
        };

        BookingService bookingService;
        HolidayService holidayService;

        public BookingController(BookingContext context) : base(new GenericService<Booking>(context))
        {
        }

        public IActionResult Booking()
        {
            return View();
        }
        public IActionResult Confirmation()
        {
            return View();
        }
        // taken out here
        // taken out here
        // taken out here
        
        
        [HttpPost]
        public IActionResult Booking(Booking booking, DateTime date, DateTime time)
        {
            booking.Time = date + time.TimeOfDay;

            BookingStatus status;
            var goat = "goat";
            if (DateTime.Now > booking.Time)
            {
                status = BookingStatus.InPast;
            }
            else if (holidayService.IsConflict(booking))
            {
                status = BookingStatus.OnHoliday;
            }
            else
            {
                status = bookingService.AddBooking(booking);
            }

            if (status == BookingStatus.BookingMade)
            {
                return View("Confirmation", booking);
            }

            ModelState.AddModelError(string.Empty, BookingErrorMessageLookup[status]);
            return View(booking);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
        


