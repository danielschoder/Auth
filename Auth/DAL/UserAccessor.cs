using Auth.Data;
using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.DAL
{
    public interface IUserAccessor
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> AnyAsync();
    }

    public class UserAccessor : IUserAccessor
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserAccessor(ApplicationDbContext applicationDbContext)
            => _applicationDbContext = applicationDbContext;

        public async Task<User> GetByEmailAsync(string email)
            => await _applicationDbContext.AUTH_Users
                .AsNoTracking()
                .Where(u => u.Email.Equals(email))
                .FirstOrDefaultAsync();

        public async Task<bool> AnyAsync()
            => await _applicationDbContext.AUTH_Users.AnyAsync();
    }
}
