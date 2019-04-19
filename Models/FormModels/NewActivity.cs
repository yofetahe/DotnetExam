using System;
using System.ComponentModel.DataAnnotations;
using DotNet_Belt.Models;

namespace DotNet_Belt.Models
{
    public class NewActivity
    {
        [Display(Name="Title")]
        [Required(ErrorMessage="Title is required.")]
        public string Title { get; set; }

        [Display(Name="Time")]
        [Required(ErrorMessage="Time is required.")]
        [CustomTimeSpan(ErrorMessage="Some value is required")]
        public TimeSpan ActivityTime { get; set; }

        [Display(Name="Date")]
        [CustomDate(ErrorMessage="Date must be in the future")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage="Date is required.")]
        [CustomTimeSpan(ErrorMessage="Some value is required")]
        public DateTime ActivityDate { get; set; }

        [Display(Name="Duration")]
        [Required(ErrorMessage="Duration is required.")]
        [Range(1, int.MaxValue, ErrorMessage="Must be greater than 0.")]
        public int Duration { get; set; }

        public string DurationTag { get; set; }

        [Display(Name="Description")]
        [Required(ErrorMessage="Description is required.")]
        public string Description { get; set; }
    }
}