using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DotNet_Belt.Models
{
    public class DojoActivity: BaseDataModel
    {
        [Key]
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public TimeSpan ActivityTime { get; set; }
        public DateTime ActivityDate { get; set; }
        public int Duration { get; set; }
        public string DurationTag { get; set; }
        public string Description { get; set; }

        public int CreatedBy {get; set;}
        public User User { get; set; }

        public List<UserActivity> UserActivity { get; set; } = new List<UserActivity>();

        public DojoActivity(){}
        public DojoActivity(NewActivity form)
        {
            Title = form.Title;
            ActivityTime = form.ActivityTime;
            ActivityDate = form.ActivityDate;
            Duration = form.Duration;
            DurationTag = form.DurationTag;
            Description = form.Description;
        }
    }
}