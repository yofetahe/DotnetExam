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
    public class UserController : Controller
    {        
        private DotnetBeltContext dbContext;
        public UserController(DotnetBeltContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login(LogUser form)
        {            
            if(ModelState.IsValid)
            {
                User UserInfo = dbContext.Users.SingleOrDefault(u => u.Email == form.LoginEmail);
                if(UserInfo is null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid User");
                    return View("Index");
                }

                PasswordHasher<LogUser> Hasher = new PasswordHasher<LogUser>();
                var result = Hasher.VerifyHashedPassword(form, UserInfo.Password, form.LoginPassword);
                
                if(!result.ToString().Equals("Success"))
                {
                    ModelState.AddModelError("LoginEmail", "Invalid User");
                    return View("Index");
                }
                
                HttpContext.Session.SetInt32("UserID", UserInfo.UserId);
                
                return RedirectToAction("Success", "User");
            } 
            return View("Index");
        }

        public bool IsPasswordValid(string password)
        {
            int[] num = new int[]{1,2,3,4,5,6,7,8,9,0};
            char[] ch = new char[]{'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k'};
            char[] special = new char[]{'!','@','#'};
            int numCounter = 0, charCounter = 0, specialCounter = 0;

            foreach(char c in password)
            {
                if(num.Contains(c)){
                    numCounter++;
                }
                if(ch.Contains(c)){
                    charCounter++;
                }
                if(special.Contains(c)){
                    specialCounter++;
                }
            }
            if(numCounter > 0 && charCounter > 0 && specialCounter > 0)
            {
                return true;
            }
            return false;
        }

        [HttpPost("registration")]
        public IActionResult Registration(RegUser form)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == form.RegEmail))
                {
                    ModelState.AddModelError("RegEmail", "This Email already exist");
                    return View("Index");
                }

                if(!IsPasswordValid(form.RegPassword)){
                    ModelState.AddModelError("RegPassword", "The password is not strong enough.");
                    return View("Index");
                }

                PasswordHasher<RegUser> Hasher = new PasswordHasher<RegUser>();
                form.RegPassword = Hasher.HashPassword(form, form.RegPassword);

                User newUser = new User(form);
                dbContext.Add(newUser);
                dbContext.SaveChanges();

                User UserInfo = dbContext.Users.SingleOrDefault(u => u.Email == form.RegEmail);
                HttpContext.Session.SetInt32("UserID", UserInfo.UserId);
                
                return RedirectToAction("Success");
            }
            return View("Index");
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            if(UserID is null)
                return RedirectToAction("Index");
            
            return RedirectToAction("GetActivities", "Activity");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserID");
            return View("Index");
        }

    }
}
