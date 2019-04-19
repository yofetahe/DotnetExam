using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNet_Belt.Models;

namespace DotNet_Belt.Controllers
{
    public class ActivityController : Controller
    {
        private DotnetBeltContext dbContext;
        public ActivityController(DotnetBeltContext context)
        {
            dbContext = context;
        }

        [HttpGet("Dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
