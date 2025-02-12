using System.ComponentModel.DataAnnotations;

namespace Core.Interfaces.DTOs
{
    public class RegisterDto
    {
        [StringLength(50)]
        public string FirstName {  get; set; } = string.Empty;

        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(24, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
