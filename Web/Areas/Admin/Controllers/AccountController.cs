using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Areas.Admin.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    public class AccountController : AdminBaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork<VwUsers> _vwUsers;
        public AccountController
            (RoleManager<IdentityRole> RoleManager, IUnitOfWork<VwUsers> vwUsers)
        {
            _roleManager = RoleManager;
            _vwUsers = vwUsers;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Roles()
        {
            var model = new RolesViewModel
            {
                NewRole = new NewRole(),
                Roles = _roleManager.Roles.AsNoTracking().OrderBy(r => r.Name).ToList()
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

            return View("Roles", viewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Users()
        {
            return View(_vwUsers.Entity.GetAll());
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
    }
}
