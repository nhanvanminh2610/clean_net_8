using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Tables;

public class UserLogin : IdentityUserLogin<int>
{
    public virtual User User { get; set; }
}
