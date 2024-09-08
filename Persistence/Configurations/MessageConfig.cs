using Domain.Entities.ConcretEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class MessageConfig : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasOne<UsersMessages>(us => us.UserMessages)
            .WithOne(m => m.Message)
            .HasForeignKey<Message>(m=>m.UserMessagesId);
    }
}
