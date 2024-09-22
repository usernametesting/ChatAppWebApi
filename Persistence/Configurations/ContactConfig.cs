using Domain.Entities.ConcretEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Persistence.Configurations;

public class ContactConfig : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasOne(c => c.User)
        .WithMany()
        .HasForeignKey(c => c.UserId)
        .OnDelete(DeleteBehavior.Restrict); 

        builder.HasOne(c => c.ContactUser)
            .WithMany()
            .HasForeignKey(c => c.ContactUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
