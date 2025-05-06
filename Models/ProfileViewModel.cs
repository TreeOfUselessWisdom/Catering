using System.ComponentModel.DataAnnotations;

namespace Catering.Models
{
    public class ProfileViewModel
    {
        [Required, Display(Name = "Full Name / Catering Name")]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
