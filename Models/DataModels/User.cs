using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DotNet_Belt.Models
{
    public class User: BaseDataModel
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }        
        public List<UserActivity> UserActivity { get; set; } = new List<UserActivity>();

        public User() { }
        public User(RegUser form)
        {
            Name = form.Name;
            Email = form.RegEmail;
            Password = form.RegPassword;
        }
    }
}