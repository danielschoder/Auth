using Auth.DAL;
using Auth.DTO;
using Auth.Helpers;
using Auth.Models;

namespace Auth.BLL;

public interface IUserManager
{
    Task<UserDto> LoginAsync(LoginDto loginDto);
}

public class UserManager : IUserManager
{
    private readonly IUserAccessor _userAccessor;
    private readonly TokenProvider _tokenProvider;

    public UserManager(IUserAccessor userAccessor, TokenProvider tokenProvider)
    {
        _userAccessor = userAccessor;
        _tokenProvider = tokenProvider;
    }

    public async Task<UserDto> LoginAsync(LoginDto loginDto)
    {
        string email = loginDto.Email?.Trim().ToLower();
        string password = loginDto.Password?.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Provide email/password");
        }

        string passwordHash = PasswordHelper.HashPassword(password);

        var user = await _userAccessor.GetByEmailAsync(email);

        if (user == null)
        {
            bool anyUsersExist = await _userAccessor.AnyAsync();
            if (!anyUsersExist)
            {
                var newUser = new User
                {
                    Email = email,
                    PasswordHash = passwordHash
                };

                await _userAccessor.InsertAsync(email, passwordHash);

                var jwt = _tokenProvider.Create(newUser);

                return new UserDto
                {
                    Id = newUser.Id,
                    Email = newUser.Email,
                    Jwt = jwt
                };
            }
            else
            {
                throw new UnauthorizedAccessException("Credentials are wrong");
            }
        }

        bool passwordMatches = PasswordHelper.VerifyPassword(user.PasswordHash, password);

        if (!passwordMatches)
        {
            throw new UnauthorizedAccessException("Credentials are wrong");
        }

        var token = _tokenProvider.Create(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Jwt = token
        };
    }
}
