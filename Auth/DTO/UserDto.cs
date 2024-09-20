namespace Auth.DTO
{
    public class UserDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public UserDto() { }
        public UserDto(string email, string passwordHash)
            => (Email, PasswordHash) = (email, passwordHash);
    }
}
