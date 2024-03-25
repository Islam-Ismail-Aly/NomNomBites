using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    public class AccountController : Controller
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            // Check if the provided model is valid
            if (ModelState.IsValid)
            {
                // Find the user by email
                ApplicationUser user = await _userManager.FindByEmailAsync(viewModel.Email);

                // If user exists
                if (user != null)
                {
                    // Check if the provided password matches the user's password
                    bool isValidPassword = await _userManager.CheckPasswordAsync(user, viewModel.Password);

                    // If password is correct
                    if (isValidPassword)
                    {
                        // Sign in the user
                        await _signInManager.SignInAsync(user, viewModel.RememberMe);

                        // Return success response
                        return Json(new { success = true });
                    }
                }

                // If user does not exist or password is incorrect, return error response
                return Json(new { success = false });
            }

            // If ModelState is not valid or login fails, return the view with the provided viewModel
            return View("Login", viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            // Check if the provided model is valid
            if (ModelState.IsValid)
            {
                // Create a new ApplicationUser object with the provided information
                var user = new ApplicationUser
                {
                    UserName = viewModel.Email,
                    Address = viewModel.Address
                };

                // Attempt to create the user in the database
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                // If user creation is successful
                if (result.Succeeded)
                {
                    // Assign the "Customer" role to the user
                    await _userManager.AddToRoleAsync(user, "Customer");

                    // Sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Return success response
                    return Json(new { success = true });
                }
                else
                {
                    // If there are errors during user creation, return error response
                    var errors = result.Errors.Select(e => e.Description);
                    return Json(new { success = false, error = string.Join(", ", errors) });
                }
            }

            // If ModelState is not valid or registration fails, return error response
            return Json(new { success = false, error = "Invalid registration data" });
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
