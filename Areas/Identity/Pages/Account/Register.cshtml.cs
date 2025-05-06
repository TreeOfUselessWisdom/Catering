using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Catering.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Catering.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name / Catering Name")]
            public string FullName { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password), Compare("Password")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string RegisterAs { get; set; }
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync start");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid: {@Errors}",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = Input.FullName,
                Address = "",
                CancellationFeePercent = 0
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} created, assigning role {Role}", Input.Email, Input.RegisterAs);
                System.Diagnostics.Debug.WriteLine("User created");
                await _userManager.AddToRoleAsync(user, Input.RegisterAs);

                // sign in immediately
                
                return LocalRedirect(ReturnUrl ?? "/");
            }
            else
            {

                foreach (var error in result.Errors)
                {
                    _logger.LogError("Creation error {Code}: {Desc}", error.Code, error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                    System.Diagnostics.Debug.WriteLine($"CreateError: {error.Description}");
                }

                return Page();
            }
        }
    }
}
