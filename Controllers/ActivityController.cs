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

            // if(ModelState.IsValid){

            //     int? UserID = HttpContext.Session.GetInt32("UserID");
            //     DojoActivity activity = new DojoActivity(form);
            //     activity.CreatedBy = (int)UserID;
            //     User user = dbContext.Users.FirstOrDefault(u => u.UserId == UserID);
            //     activity.User = user;
                
            //     dbContext.Add(activity);
            //     dbContext.SaveChanges();

            //     return RedirectToAction("GetActivities");
            // }

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
