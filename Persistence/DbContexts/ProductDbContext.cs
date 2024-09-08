using Domain.Entities;
using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using Domain.Entities.ConcretEntities;
using Domain.Entities.Identity;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Persistence.DbContexts;

//public class ProductDbContext : DbContext
public class ProductDbContext : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, AppUserToken>
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(cancellationToken);
    }
    private void OnBeforeSave()
    {
        foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
        {
            if (entry.Entity is ISoftDelete softDeleteEntity)
            {
                entry.State = EntityState.Modified;
                softDeleteEntity.IsDeleted = true;

                if (entry.Entity.GetType().GetProperty("UpdatedAt") != null)
                {
                    entry.Entity.GetType()?.GetProperty("UpdatedAt")?.SetValue(entry.Entity, DateTime.UtcNow);
                }
            }
        }
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {

        foreach (var entity in builder.Model.GetEntityTypes())
        {
            if (ShouldBeFilter(entity))
            {
                var expression = GetExpressionByEntity(entity);
                builder.Entity(entity.ClrType).HasQueryFilter(expression);
            }
        }
        builder.Entity<AppUser>().HasQueryFilter(u => !u.IsDeleted);

        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


    }

    private LambdaExpression GetExpressionByEntity(IMutableEntityType entity)
    {
        var parameter = Expression.Parameter(entity.ClrType, "e");
        var property = Expression.Property(parameter, "IsDeleted");
        var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);
        return filter;
    }

    private bool ShouldBeFilter(IMutableEntityType entity) =>
        typeof(ISoftDelete).IsAssignableFrom(entity.ClrType);

    public virtual DbSet<UsersMessages> UsersMessages { get; set; }
    public virtual DbSet<Message> Messages { get; set; }



}

