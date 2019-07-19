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
    public class TableController : BaseController<Table>
    {  
      
        BookingService bookingService;
        TableService tableService;
        

        public TableController(BookingContext context) : base(new GenericService<Table>(context))
        {
            bookingService = new BookingService(context);

            tableService = new TableService(context);
        }

        [HttpGet]
        public IActionResult AddingTables()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddingTables(Table table)
        {
            service.Add(table);
            return View(table);
        }
        [HttpGet]
        public IActionResult Table()
        {
            var viewModel2 = new TableConflictViewModel();
            viewModel2.ConflictedBooking2 = new List<Booking>();
            viewModel2.DeletedTable = new Table();
            return View(viewModel2);
        }
        [HttpPost]
        public IActionResult DeleteGoat(TableConflictViewModel viewModel2, bool confirmDelete2 = false)
        {
            if (viewModel2.ConflictedBooking2 != null && viewModel2.ConflictedBooking2.Any())
            {
                if (confirmDelete2)
                {
                    foreach (var deletedBooking in viewModel2.ConflictedBooking2)
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

            var conflictions2 = tableService.FindConflicts(viewModel2.DeletedTable);
            if (conflictions2.Any())
            {
                viewModel2.ConflictedBooking2 = conflictions2;
                return View(viewModel2);
            }

            service.Delete(viewModel2.DeletedTable.Id);

            return RedirectToAction("List");
        }
    }
}
