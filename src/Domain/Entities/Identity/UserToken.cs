using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Tables;

public class UserToken : IdentityUserToken<int>
{
    public virtual User User { get; set; }
}
