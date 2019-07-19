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
    public class HomeController : Controller
    {
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Menu()
        {
            return View();
        }

       
        [HttpGet]
        public IActionResult FAQs()
        {
            return View();
        }
       
    }
}
        


