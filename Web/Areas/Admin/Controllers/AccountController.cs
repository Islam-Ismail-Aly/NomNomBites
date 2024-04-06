using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Web.Areas.Admin.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : AdminBaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork<VwUsers> _vwUsers;
        private readonly IUnitOfWork<Customer> _customerUnitOfWork;
        public AccountController
            (RoleManager<IdentityRole> RoleManager, UserManager<ApplicationUser> UserManager, 
            IUnitOfWork<VwUsers> vwUsers, SignInManager<ApplicationUser> signInManager, 
            IUnitOfWork<Customer> customerUnitOfWork)
        {
            _roleManager = RoleManager;
            _userManager = UserManager;
            _vwUsers = vwUsers;
            _signInManager = signInManager;
            _customerUnitOfWork = customerUnitOfWork;
        }

        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(LoginViewModel viewModel)
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

                        // Redirect to home index upon successful login
                        return RedirectToAction("Index", "Home");
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

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Roles()
        {
            var model = new RolesViewModel
            {
                NewRole = new NewRole(),
                Roles = _roleManager.Roles.AsNoTracking().ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Roles(RolesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Id = viewModel.NewRole.RoleId,
                    Name = viewModel.NewRole.RoleName
                };

                // Create New role if the model is null
                if (string.IsNullOrEmpty(role.Id)) // Check if Id is null or empty
                {
                    role.Id = Guid.NewGuid().ToString();
                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        return Json(new { success = true, message = $"Role of {viewModel.NewRole.RoleName} created successfully." });
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Role of {viewModel.NewRole.RoleName} creation failed.");
                    }
                }
                // Update if there is role in db
                else
                {
                    var existingRole = await _roleManager.FindByIdAsync(role.Id);
                    if (existingRole != null)
                    {
                        existingRole.Name = role.Name; // Update role name
                        var result = await _roleManager.UpdateAsync(existingRole);
                        if (result.Succeeded)
                        {
                            return Json(new { success = true, message = $"Role of {viewModel.NewRole.RoleName} updated successfully." });
                        }
                        else
                        {
                            ModelState.AddModelError("", $"Role of {viewModel.NewRole.RoleName} update failed.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Role of {viewModel.NewRole.RoleName} not found.");
                    }
                }
            }

            return View(nameof(Roles), viewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Users()
        {
            return View(_vwUsers.Entity.GetAll().Where(vw => vw.Status));
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            var existingCustomer = _customerUnitOfWork.UserRepository.GetByUserId(userId);

            if (existingCustomer == null)
            {
                return NotFound(new { message = "Customer not found." });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // Construct error message
                var errorMessage = string.Join("\n", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = errorMessage });
            }

            return RedirectToAction(nameof(Users));
        }



        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = _roleManager.Roles.FirstOrDefault(r => r.Id == Id);
            var deleteRole = await _roleManager.DeleteAsync(role);

            if (deleteRole.Succeeded)
            {
                return RedirectToAction(nameof(Roles));
            }

            return RedirectToAction(nameof(Roles));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var customer = _customerUnitOfWork.UserRepository.GetByUserId(currentUser.Id);

                if (customer != null)
                {
                    ViewBag.Phone = customer.Phone;
                    ViewBag.City = customer.City;
                    ViewBag.DateAdded = customer.CreationDate.ToShortDateString();
                    ViewBag.Status = customer.Status;
                }
            }
            return View();
        }
    }
}
