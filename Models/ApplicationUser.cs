using Microsoft.AspNetCore.Identity;

namespace Catering.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Profile fields
        public string? FullName { get; set; }
        public string? Address { get; set; }

        // Cancellation policy, as percentage fee deducted
        public int CancellationFeePercent { get; set; }
 
        // Maximum number of guests this caterer can handle
        public int Capacity { get; set; } = 50;
    }
}