using Microsoft.AspNetCore.Mvc;

namespace Catering.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
