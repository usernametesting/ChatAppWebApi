using Domain.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Commons;

public interface IGenericReadRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : IBaseEntity<TKey>

{
    Task<IQueryable<TEntity>> GetAllAsQueryableAsync();

    Task<IEnumerable<TEntity>> GetAllAsync(bool IsTracking=true);

    Task<TEntity> GetByIdAsync(TKey Id,bool IsTracking=true);

    Task<List<TEntity>> GetByExpression(Expression<Func<TEntity, bool>> expression,bool IsTracking=true);

}
