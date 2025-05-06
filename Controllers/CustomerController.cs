using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Catering.Models;
using System.Threading.Tasks;

[Authorize(Roles = "Customer")]
public class CustomerController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var vm = new ProfileViewModel
        {
            FullName = user.FullName,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(ProfileViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        user.FullName = vm.FullName;
        user.Address = vm.Address;
        user.PhoneNumber = vm.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var e in result.Errors)
                ModelState.AddModelError("", e.Description);
            return View(vm);
        }

        ViewBag.StatusMessage = "Profile updated";
        return View(vm);
    }
}
