using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class AccountController : AdminBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController
            (UserManager<ApplicationUser> UserManager
            , SignInManager<ApplicationUser> SignInManager)
        {
            _userManager = UserManager;
            _signInManager = SignInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
    }
}
