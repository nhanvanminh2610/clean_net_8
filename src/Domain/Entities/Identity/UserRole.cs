﻿using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class UserRole : IdentityUserRole<int>
{
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}
