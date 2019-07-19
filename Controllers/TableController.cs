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
    [Authorize]
    public class TableController : BaseController<Table>
    {  
        TableService tableService;

        public TableController(BookingContext context) : base(new GenericService<Table>(context))
        {
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
       
       
    }
}
