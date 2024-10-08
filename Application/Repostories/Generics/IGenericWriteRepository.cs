﻿using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Commons;

public interface IGenericWriteRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey>
                                                            where TEntity : IBaseEntity<Tkey>
{
    Task AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task Update(Dictionary<string, object> obj, Tkey Id);
    public Task UpdateMultipleAsync(Dictionary<string, object> updateValues, Expression<Func<TEntity, bool>> filter);
 
    Task Update(Tkey Id,TEntity model);
    Task Patch(TEntity entity);

    Task Delete(TEntity entity);
    Task DeleteById(Tkey Id);
}
