using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNet_Belt.Models
{
    public class UserActivityRel 
    {

        public User User { get; set; }
        public DojoActivity Activity { get; set; }
        public UserActivity UserActivity {get; set;}
        public List<User> Users { get; set; } = new List<User>();
        public List<DojoActivity> activities {get; set;} = new List<DojoActivity>();
        public List<UserActivity> userActivities {get; set;} = new List<UserActivity>();

    }
}