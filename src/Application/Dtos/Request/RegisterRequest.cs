using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Avatar { get; set; }
        public bool IsActive { get; set; }
        public List<int> RoleIds { get; set; } = [];
        public string? PhoneNumber { get; set; }

        public DateTime? LastActivityTime { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
