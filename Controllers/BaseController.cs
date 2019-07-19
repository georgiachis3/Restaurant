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
    public class BaseController<T> : Controller where T : Identifiable
    {
        GenericService<T> service;

        public BaseController(GenericService<T> service)
        {
            this.service = service;
        }

        [Authorize]
        [HttpGet]
        public IActionResult View(int Id)
        {
            var deleteBooking = service.Get(Id);
            if (deleteBooking == null)
            {
                return NotFound();
            }
            return View(deleteBooking);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            service.Delete(Id);
            return RedirectToAction("Admin");
        }

        [Authorize]
        [HttpGet]
        public IActionResult List()
        {
            return View(service.GetAll());
        }
    }
}
        


