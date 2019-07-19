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

        HolidayService holidayService;
        BookingService bookingService;


        public BookingController(BookingContext bookingContext) : base(new GenericService<Booking>(bookingContext))
        {
            holidayService = new HolidayService(bookingContext);
            bookingService = new BookingService(bookingContext);
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
        public IActionResult Booking(Booking createdBooking, DateTime date, DateTime time)
        {
            createdBooking.Time = date + time.TimeOfDay;

            BookingStatus status;
            var goat = "goat";
            if (DateTime.Now > createdBooking.Time)
            {
                status = BookingStatus.InPast;
            }
            else if (holidayService.IsConflict(createdBooking))
            {
                status = BookingStatus.OnHoliday;
            }
            else
            {
                status = bookingService.AddBooking(createdBooking);
            }

            if (status == BookingStatus.BookingMade)
            {
                return View("Confirmation", createdBooking);
            }

            ModelState.AddModelError(string.Empty, BookingErrorMessageLookup[status]);
            return View(createdBooking);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
        


