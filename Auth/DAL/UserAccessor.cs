using Auth.Data;
using Auth.DTO;
using Microsoft.EntityFrameworkCore;

namespace Auth.DAL
{
    public interface IUserAccessor
    {
        Task<UserDto> GetByEmailAsync(string email);
    }

    public class UserAccessor : IUserAccessor
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserAccessor(ApplicationDbContext applicationDbContext)
            => _applicationDbContext = applicationDbContext;

        public async Task<UserDto> GetByEmailAsync(string email)
            => await _applicationDbContext.AUTH_Users
                .AsNoTracking()
                .Where(u => u.Email.Equals(email))
                .Select(u => new UserDto(u.Email, u.PasswordHash))
                .SingleOrDefaultAsync();
    }
}
