using Domain.Entities;
using Domain.Entities.ConcretEntities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations;

public class MessageConfig : IEntityTypeConfiguration<Message>
{

    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder
        .HasOne(m => m.FromUser)
        .WithMany(u=>u.FromMessages)
        .HasForeignKey(m => m.FromUserId)
        .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(m => m.ToUser)
            .WithMany(u=>u.ToMessages)
            .HasForeignKey(m => m.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);




    }
}
