using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.IdentityConfigs;

public class AppUserConfig : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
       

        builder.HasMany(u => u.UserRoles)
               .WithOne(ur => ur.User)
               .HasForeignKey(ur => ur.UserId)
               .IsRequired();

        builder.HasIndex(u => u.NormalizedUserName).IsUnique(false);
        builder.HasIndex(u => u.NormalizedEmail).IsUnique(false);
        builder.HasIndex(u => u.Email).IsUnique(false);
        builder.HasIndex(u => u.UserName).IsUnique(false);


        builder
            .HasIndex(u => new { u.UserName, u.IsDeleted })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder
            .HasIndex(u => new { u.NormalizedUserName, u.IsDeleted })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder
            .HasIndex(u => new { u.Email, u.IsDeleted })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");


        builder
            .HasIndex(u => new { u.NormalizedEmail, u.IsDeleted })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.ToTable("Users");


        //seed datas
        var adminUser = new AppUser
        {
            Id = 1,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            LockoutEnabled = false,
            AccessFailedCount = 0,
        };

        var hasher = new PasswordHasher<AppUser>();
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "AdminPassword123!");

        builder.HasData(adminUser);

    }
}
