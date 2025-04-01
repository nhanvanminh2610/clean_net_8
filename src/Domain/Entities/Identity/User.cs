using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity;

public class User : IdentityUser<int>
{
    public DateTime CreatedDateTime { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    public bool IsActive { get; set; }

    [Required]
    [MaxLength(50)]
    public string Avatar { get; set; }

    [MaxLength(256)]
    public string PasswordSalt { get; set; }

    public virtual ICollection<UserClaim> UserClaims { get; set; }
    public virtual ICollection<UserLogin> UserLogins { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<UserToken> UserTokens { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

    public DateTime? LastActivityTime { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string Address { get; set; }
    public string Bio { get; set; }
}
