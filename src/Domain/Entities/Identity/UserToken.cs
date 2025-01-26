using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class UserToken : IdentityUserToken<int>
{
    public virtual User User { get; set; }
}
