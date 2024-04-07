using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Areas.Admin.Controllers;
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

        [HttpGet]
        public async Task<IActionResult> Profile(CustomerOrdersViewModel viewModel)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var customer = _customer.UserRepository.GetByUserId(currentUser.Id);
                var orderlist = _customer.UserRepository.GetOrdersByCustomerId(customer.Id);

                if (customer != null)
                {
                    ViewBag.Phone = customer.Phone;
                    ViewBag.City = customer.City;
                    ViewBag.DateAdded = customer.CreationDate.ToShortDateString();
                    ViewBag.Status = customer.Status;

                    var orderslist = new CustomerOrdersViewModel()
                    {
                        CustomerId = customer.Id,
                        CustomerOrders = orderlist,
                    };
                }
            }

            return View();
        }

        public IActionResult ExternalLogin(string provider)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, "/Account/ExternalResponse");

            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalResponse()
        {
            // Get the external login info
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                // Redirect to some error page or handle as needed
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            // Attempt to sign in the user with the external provider
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
            {
                // Redirect to the appropriate page after successful login
                return RedirectToAction(nameof(Profile));
            }
            else
            {
                // If the user doesn't have an account, create a new one
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                var address = info.Principal.FindFirstValue(ClaimTypes.Country);

                var user = new ApplicationUser
                {
                    Name = name,
                    UserName = email,
                    Address = address
                };

                var createResult = await _userManager.CreateAsync(user);

                if (createResult.Succeeded)
                {
                    var externalLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (externalLoginResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false, info.LoginProvider);
                        return RedirectToAction(nameof(HomeController.Index), nameof(Index));
                    }
                    var customer = new Customer
                    {
                        Name = name,
                        Address = user.Address,
                        Phone = user.PhoneNumber,
                        City = user.Address,
                        Status = true,
                        CreationDate = DateTime.UtcNow,
                        ApplicationUserId = user.Id,
                    };

                    _customer.Entity.Insert(customer);

                    _customer.Save();

                    await _userManager.AddToRoleAsync(user, "Customer");

                    return Json(new { success = true });
                }
                else
                {
                    // Handle user creation failure
                    // You may choose to redirect to an error page or handle differently
                }

                return RedirectToAction(nameof(Register));
            }
        }
    }
}
