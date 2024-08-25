using Application.Repositories.Commons;
using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitOfWorks;

public interface IUnitOfWork
{
    public IGenericReadRepository<TEntity, TKey> GetReadRepository<TEntity, TKey>()
        where TEntity : class,IBaseEntity<TKey>;

    public IGenericWriteRepository<TEntity, TKey> GetWriteRepository<TEntity, TKey>()
        where TEntity : class, IBaseEntity<TKey>;

    Task Commit();
}
