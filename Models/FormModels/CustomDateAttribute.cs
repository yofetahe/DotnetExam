using System;
using System.ComponentModel.DataAnnotations;

namespace DotNet_Belt.Models
{
    public class CustomDateAttribute : ValidationAttribute
    {
        public CustomDateAttribute()
        {
        }
        public override bool IsValid(object value)
        {
            var dt = (DateTime)value;
            if(dt >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }

    public class CustomTimeSpanAttribute : ValidationAttribute
    {
        public CustomTimeSpanAttribute()
        {
        }
        public override bool IsValid(object value)
        {
            System.Console.WriteLine(">>>>>> " + value);
            if(value.ToString().Length > 0)
            {
                return true;
            }
            return false;
        }
    }
}