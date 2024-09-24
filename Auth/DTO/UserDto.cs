namespace Auth.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Jwt { get; set; }

        public UserDto() { }
        public UserDto(Guid id, string email, string jwt)
            => (Id, Email, Jwt) = (id, email, jwt);
    }
}
