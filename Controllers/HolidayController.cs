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

        BookingService bookingService;
        HolidayService holidayService;


        public HolidayController(BookingContext context)
        {

            bookingService = new BookingService(context);
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
        public IActionResult Holidays()
        {
            var viewModel = new BookingConflictViewModel();
            viewModel.ConflictedBooking = new List<Booking>();
            viewModel.InputtedHoliday = new Holidays();
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Holidays(BookingConflictViewModel viewModel, bool confirmDelete = false)
        {
            if (viewModel.ConflictedBooking != null && viewModel.ConflictedBooking.Any())
            {
                if (confirmDelete)
                {
                   foreach(var deletedBooking in viewModel.ConflictedBooking)
                    {
                        bookingService.Delete(deletedBooking.Id);
                    }
                }
                else
                {
                    foreach (var item in ModelState)
                    {
                        if (item.Key.Contains("ConflictedBooking"))
                        {
                            ModelState.ClearValidationState(item.Key);
                            ModelState.MarkFieldValid(item.Key);
                        }
                    }                    
                }
            }


            var conflictions = holidayService.FindConflicts(viewModel.InputtedHoliday);
            if (conflictions.Any())
            {
                viewModel.ConflictedBooking = conflictions;
                return View(viewModel);
            }


            var status = holidayService.AddHolidayBooking(viewModel.InputtedHoliday);
            if (status == HolidayBookingStatus.OK)
            {
                return RedirectToAction("ListofHolidays");
            }

            ModelState.AddModelError(string.Empty, HolidayErrorMessageLookup[status]);

            return View(viewModel);


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
        


