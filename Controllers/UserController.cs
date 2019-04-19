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

                int[] num = new int[]{1,2,3,4,5,6,7,8,9,0};
                char[] ch = new char[]{'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k'};
                char[] special = new char[]{'!','@','#'};

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

            // User UserInfo = dbContext.Users.SingleOrDefault(u => u.UserId == UserID);
            
            // return RedirectToAction("Dashboard", "DojoActivity");
            return RedirectToAction("GetActivities");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserID");
            return View("Index");
        }




        public bool IsUserInSession()
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            if(UserID is null){
                return false;
            }
            return true;
        }


        [HttpGet("Activities")]
        public IActionResult GetActivities()
        {
            if(!IsUserInSession())
                return RedirectToAction("Index", "User");

            List<User> users = dbContext.Users.ToList();

            DateTime t = DateTime.Now;

            List<DojoActivity> Activites = dbContext.Activities
                .Include(a => a.UserActivity)
                .ThenInclude(b => b.User)
                .Where(d => d.ActivityDate > t)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
            
            UserActivityRel model = new UserActivityRel();
            model.activities = Activites;
            model.Users = users;

            return View("ActivitiesList", model);
        }

        [HttpGet("GetActivityRegForm")]
        public IActionResult GetActivityRegForm()
        {
            if(!IsUserInSession())
                return RedirectToAction("Index", "User");

            return View("NewActivity");
        }

        [HttpPost("NewActivity")]
        public IActionResult NewActivity(NewActivity form)
        {
            if(!IsUserInSession())
                return RedirectToAction("Index", "User");

            System.Console.WriteLine(form.Duration);
            System.Console.WriteLine(form.ActivityTime);
            System.Console.WriteLine(form.ActivityDate);
            System.Console.WriteLine(form.Duration == 0);
            if(form.Duration == 0){
                ModelState.AddModelError("Duration", "Invalid User");
                return View("NewActivity");
            }
           

            if(ModelState.IsValid){

                int? UserID = HttpContext.Session.GetInt32("UserID");
                DojoActivity activity = new DojoActivity(form);
                activity.CreatedBy = (int)UserID;
                User user = dbContext.Users.FirstOrDefault(u => u.UserId == UserID);
                activity.User = user;
                
                dbContext.Add(activity);
                dbContext.SaveChanges();

                return RedirectToAction("GetActivities");
            }

            return View("NewActivity");
        }

        [HttpGet("ActivityDetail/{ActivityId}")]
        public IActionResult ActivityDetail(int ActivityId)
        {
            if(!IsUserInSession())
                return RedirectToAction("Index", "User");
            
            List<User> users = dbContext.Users.ToList();

            DojoActivity activity = dbContext.Activities
                .Include(a => a.UserActivity)
                .ThenInclude(a => a.User)
                .FirstOrDefault(a => a.ActivityId == ActivityId);

            UserActivityRel model = new UserActivityRel();
            model.Activity = activity;
            model.Users = users;

            return View(model);
        }

        [HttpGet("JoinActivity/{ActivityId}")]
        public IActionResult JoinActivity(int ActivityId)
        {
            if(!IsUserInSession())
                return RedirectToAction("Index", "User");

            int? UserID = HttpContext.Session.GetInt32("UserID");

            // List<UserActivity> activityList = dbContext.UserActivity
            //     .Include(a => a.DojoActivity)
            //     .ThenInclude(b => b.UserActivity)
            //     .Where(c => c.UserId == UserID)
            //     .ToList();

            // foreach(var i in activityList)
            // {
            //     if(i.DojoActivity.ActivityDate )
            // }
            
            User userAdd = dbContext.Users.FirstOrDefault(u => u.UserId == UserID);
            UserActivity user = new UserActivity();
            user.UserId = (int)UserID;
            user.ActivityId = ActivityId;

            dbContext.UserActivity.Add(user);
            dbContext.SaveChanges();
            return RedirectToAction("GetActivities");
        }

        [HttpGet("DeleteActivity/{ActivityId}")]
        public IActionResult DeleteActivity(int ActivityId)
        {
            if(!IsUserInSession())
                return RedirectToAction("Index", "User");

            DojoActivity activity = dbContext.Activities
                .FirstOrDefault(a => a.ActivityId == ActivityId);

            dbContext.Activities.Remove(activity);
            dbContext.SaveChanges();
            return RedirectToAction("GetActivities");
        }

        [HttpGet("DeleteJoin/{ActivityId}")]
        public IActionResult DeleteJoin(int ActivityId)
        {
            if(!IsUserInSession())
                return RedirectToAction("Index", "User");

            int? UserID = HttpContext.Session.GetInt32("UserID");
            UserActivity user = dbContext.UserActivity
                .FirstOrDefault(u => u.ActivityId == ActivityId && u.UserId == UserID);

            dbContext.UserActivity.Remove(user);
            dbContext.SaveChanges();
            return RedirectToAction("GetActivities");
        }

    }
}
