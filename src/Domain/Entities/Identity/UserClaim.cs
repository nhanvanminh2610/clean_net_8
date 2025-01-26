using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class UserClaim : IdentityUserClaim<int>
{
    public virtual User User { get; set; }
}
