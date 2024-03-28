using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Validators;
using Web.ViewModels;

namespace Web.Controllers
{
    [AllowAnonymous]
    [AuthenticatedUsersAttribute]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork<Customer> _customer;

        public AccountController
            (UserManager<ApplicationUser> UserManager
            , SignInManager<ApplicationUser> SignInManager, IUnitOfWork<Customer> customer)
        {
            _userManager = UserManager;
            _signInManager = SignInManager;
            _customer = customer;
        }

        [HttpGet]
        public IActionResult Login()
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
                ApplicationUser user = await _userManager.FindByNameAsync(viewModel.Email);

                // if user exist
                if (user != null)
                {
                    // Check if the provided password matches the user's password
                    bool isValidPassword = await _userManager.CheckPasswordAsync(user, viewModel.Password);

                    if (isValidPassword)
                    {
                        // Sign in the user
                        await _signInManager.SignInAsync(user, viewModel.RememberMe);

                        // Return success response
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Incorrect Email or Password!" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Email not found" });
                }
            }

            // If ModelState is not valid or login fails, return the view with the provided viewModel
            return View("Login", viewModel);
        }


        [HttpGet]
        public IActionResult Register()
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
                var user = new ApplicationUser
                {
                    Name = viewModel.Name,
                    UserName = viewModel.Email,
                    Address = viewModel.Address
                };

                var result = await _userManager.CreateAsync(user, viewModel.Password);

                // If user creation is successful
                if (result.Succeeded)
                {
                    var customer = new Customer
                    {
                        Name = viewModel.Name,
                        Address = viewModel.Address,
                        Phone = viewModel.Phone,
                        City = viewModel.City,
                        Status = true,
                        CreationDate = DateTime.UtcNow,
                        ApplicationUserId = user.Id,
                    };

                    _customer.Entity.Insert(customer);

                    _customer.Save();

                    await _userManager.AddToRoleAsync(user, "Customer");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return Json(new { success = true });
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }
            }

            return Json(new { success = false, message = "Invalid registration data!" });
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
