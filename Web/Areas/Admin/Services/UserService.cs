using Infrastructure.Data;
using Web.Areas.Admin.ViewModels;

namespace Web.Areas.Admin.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<IQueryable<AdminUserViewModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<AdminUserViewModel>> SearchAsync(string termCriteria)
        {
            throw new NotImplementedException();
        }

        public Task<AdminUserViewModel> UserRegistrationCountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
