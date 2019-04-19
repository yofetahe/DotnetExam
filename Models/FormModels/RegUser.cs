using System.ComponentModel.DataAnnotations;

namespace DotNet_Belt.Models
{
    public class RegUser 
    {
       [Display(Name="Name")]
       [Required(ErrorMessage="name is required.")]
       [MinLength(4, ErrorMessage="Min of 2 character")]
       public string Name { get; set; }
       
       [Display(Name="Email")]
       [Required(ErrorMessage="Email is required.")]
       [EmailAddress]
       public string RegEmail { get; set; }
       
       [Display(Name="Password")]
       [Required(ErrorMessage="Password is required.")]
       [MinLength(8, ErrorMessage="Min of 8 character allowed.")]
       [MaxLength(20, ErrorMessage="Max of 20 character allowed.")]
       [DataType(DataType.Password)]
       public string RegPassword { get; set; }

       [Display(Name="Confirm Password")]
       [Compare("RegPassword", ErrorMessage="Password do not match.")]
       [DataType(DataType.Password)]
       public string RegConfirm {get; set; }
    }
}