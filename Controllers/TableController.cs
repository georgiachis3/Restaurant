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
    public class TableController : Controller
    {

  
        TableService tableService;

        public TableController(BookingContext context)
        {
            
            tableService = new TableService(context);

        }
        [HttpGet]


       
        [HttpGet]
        public IActionResult ViewTable(int Id)
        {
            var table = tableService.Get(Id);
            if (table == null)
            {
                return NotFound();
            }
            return View(table);
        }
        [HttpPost]
        public IActionResult DeleteTable(int Id)
        {
            tableService.Delete(Id);
            return RedirectToAction("ListofTables");
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
       
        [HttpGet]
        public IActionResult ListofTables()
        {
            return View(tableService.GetAll());
        }
    }
}
