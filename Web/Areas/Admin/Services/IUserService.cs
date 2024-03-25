using Web.Areas.Admin.ViewModels;

namespace Web.Areas.Admin.Services
{
    public interface IUserService
    {
        Task<IQueryable<AdminUserViewModel>> GetAllAsync();
        Task<IQueryable<AdminUserViewModel>> SearchAsync(string termCriteria);
        Task<AdminUserViewModel> UserRegistrationCountAsync();
    }
}
