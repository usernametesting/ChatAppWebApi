using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.IdentityConfigs;

public class AppUserTokenConfig : IEntityTypeConfiguration<AppUserToken>
{
    public void Configure(EntityTypeBuilder<AppUserToken> builder)
    {

        builder.HasKey(e=>e.Id);
        builder.HasIndex(e => new { e.UserId, e.LoginProvider, e.Name }).IsUnique();
        builder.ToTable("UserTokens");

    }
}
