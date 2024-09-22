using Domain.Entities.ConcretEntities;
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

        builder.HasMany(u => u.UserTokens)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId);

        builder.HasMany(u => u.Contacts)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);

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
        var hasher = new PasswordHasher<AppUser>();
        var users = new List<AppUser>();
        for (int i = 1; i < 4; i++)
        {
            var user = new AppUser
            {
                Id = i,
                UserName = "user" + i,
                NormalizedUserName = "USER" + i,
                Email = "user"+i,
                NormalizedEmail = "USER" + i,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                AccessFailedCount = 0,
            };
            user.PasswordHash = hasher.HashPassword(user, "user"+i);
            users.Add(user);
        }
       
        builder.HasData(users);

    }
}
