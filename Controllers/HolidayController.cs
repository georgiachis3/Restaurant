using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;
using Web.Services;
using static Web.Data.BookingService;

namespace Web.Controllers
{
    public class HolidayController : Controller
    {
        private Dictionary<HolidayBookingStatus, string> HolidayErrorMessageLookup = new Dictionary<HolidayBookingStatus, string>()
        {
            {HolidayBookingStatus.BeforeStart, "Your holiday cannot end before it starts." },
            {HolidayBookingStatus.GreaterThanMonth, "You cannot take holiday for longer than a month" },
            {HolidayBookingStatus.IsInPast, "This date is in the past." },
        };

        HolidayService holidayService;
       

        public HolidayController(BookingContext context)
        {
            
            holidayService = new HolidayService(context);
           
        }
        [HttpGet]
        public IActionResult ViewHoliday(int Id)
        {
            var holiday = holidayService.Get(Id);
            if (holiday == null)
            {
                return NotFound();
            }
            return View(holiday);
        }
        [HttpPost]
        public IActionResult DeleteHoliday(int Id)
        {
            holidayService.Delete(Id);
            return RedirectToAction("ListofHolidays");
        }

        public IActionResult ListofHolidays()
        {
            return View(holidayService.GetAll());
        }
       
      
        [Authorize]
        [HttpPost]
        public IActionResult Holidays(Holidays holidays)
        {
            var conflictions = holidayService.FindConflicts(holidays);
            if (conflictions.Any())
            {
                var viewModel = new BookingConflictViewModel();
                viewModel.InputtedHoliday = holidays;
                viewModel.ConflictedBooking = conflictions;
                return View("ConflictedBookings", viewModel);
            }

            var status = holidayService.AddHolidayBooking(holidays);
            if (status == HolidayBookingStatus.OK)
            {
                return View(holidays);
            }

            ModelState.AddModelError(string.Empty, HolidayErrorMessageLookup[status]);

            return View(holidays);


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
        


