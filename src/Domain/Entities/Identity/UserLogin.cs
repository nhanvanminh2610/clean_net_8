﻿using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class UserLogin : IdentityUserLogin<int>
{
    public virtual User User { get; set; }
}
