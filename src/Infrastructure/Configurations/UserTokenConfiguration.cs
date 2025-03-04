﻿using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.HasOne(userToken => userToken.User)
            .WithMany(userToken => userToken.UserTokens)
            .HasForeignKey(userRole => userRole.UserId);

        builder.ToTable("UserTokens");
    }
}
