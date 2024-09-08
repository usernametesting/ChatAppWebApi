using Domain.Entities;
using Domain.Entities.ConcretEntities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations;

public class UserMessageConfig : IEntityTypeConfiguration<UsersMessages>
{

    public void Configure(EntityTypeBuilder<UsersMessages> builder)
    {

        builder
          .HasOne(um => um.FromUser)
          .WithMany(u => u.Messages) 
          .HasForeignKey(um => um.FromUserId)
          .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(um => um.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}
