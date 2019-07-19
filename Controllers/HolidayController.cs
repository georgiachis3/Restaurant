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
    [Authorize]
    public class HolidayController : BaseController<Holidays>
    {

        private Dictionary<HolidayBookingStatus, string> HolidayErrorMessageLookup = new Dictionary<HolidayBookingStatus, string>()
        {
            {HolidayBookingStatus.BeforeStart, "Your holiday cannot end before it starts." },
            {HolidayBookingStatus.GreaterThanMonth, "You cannot take holiday for longer than a month" },
            {HolidayBookingStatus.IsInPast, "This date is in the past." },
        };

        BookingService bookingService;
        HolidayService holidayService;

        public HolidayController(BookingContext holidayContext) : base(new GenericService<Holidays>(holidayContext))
        {
            bookingService = new BookingService(holidayContext);
            holidayService = new HolidayService(holidayContext);
        }
        [HttpGet]
        

        public IActionResult Holidays()
        {
            var viewModel = new BookingConflictViewModel();
            viewModel.ConflictedBooking = new List<Booking>();
            viewModel.InputtedHoliday = new Holidays();
            return View(viewModel);
        }

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


            var holidayConflictions = holidayService.FindConflicts(viewModel.InputtedHoliday);
            if (holidayConflictions.Any())
            {
                viewModel.ConflictedBooking = holidayConflictions;
                return View(viewModel);
            }


            var status = holidayService.AddHolidayBooking(viewModel.InputtedHoliday);
            if (status == HolidayBookingStatus.OK)
            {
                return RedirectToAction("List");
            }

            ModelState.AddModelError(string.Empty, HolidayErrorMessageLookup[status]);

            return View(viewModel);


        }
    }
}
        


