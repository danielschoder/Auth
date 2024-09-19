using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(1023)")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(1023)")]
        public string PasswordHash { get; set; }
    }
}
