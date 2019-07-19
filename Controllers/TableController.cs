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
        

        public TableController(BookingContext tableContext) : base(new GenericService<Table>(tableContext))
        {
            bookingService = new BookingService(tableContext);

            tableService = new TableService(tableContext);
        }

        [HttpGet]
        public IActionResult AddingTables()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddingTables(Table reservedTable)
        {
            service.Add(reservedTable);
            return View(reservedTable);
        }
        [HttpGet]
        public IActionResult ViewTable(int id)
        {
            var viewModel2 = new TableConflictViewModel();
            viewModel2.ConflictedBooking2 = new List<Booking>();
            viewModel2.DeletedTable = service.Get(id);
            return View(viewModel2);
        }
        
        [HttpPost]
        public IActionResult ViewTable(TableConflictViewModel tableViewModel, bool deleteTable = false)
        {
            if (tableViewModel.ConflictedBooking2 != null && tableViewModel.ConflictedBooking2.Any())
            {
                if (deleteTable)
                {
                    foreach (var deletedBooking in tableViewModel.ConflictedBooking2)
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

            var tableConflictions = tableService.FindConflicts(tableViewModel.DeletedTable);
            if (tableConflictions.Any())
            {
                tableViewModel.ConflictedBooking2 = tableConflictions;
                return View(tableViewModel);
            }

            service.Delete(tableViewModel.DeletedTable.Id);

            return RedirectToAction("List");
        }
    }
}
