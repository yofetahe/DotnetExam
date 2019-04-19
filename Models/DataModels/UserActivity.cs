using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DotNet_Belt.Models
{
    public class UserActivity: BaseDataModel
    {
        [Key]
        public int UserActivityId { get; set; }
        public int UserId { get; set; }
        public int ActivityId { get; set; }

        public User User { get; set; }
        public DojoActivity DojoActivity { get; set; }
    }
}