using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.ViewModels
{
    public class RolesViewModel
    {
        public List<IdentityRole>? Roles { get; set; }
        public NewRole NewRole { get; set; }
    }

    public class NewRole
    {
        public string? RoleId { get; set; }

        [Required(ErrorMessage = "Role Name is required.")]
        public string RoleName { get; set; }
    }
}
