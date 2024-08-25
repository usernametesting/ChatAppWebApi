using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Commons;

public interface IGenericRepository<TEntity,TKey>
    where TEntity : IBaseEntity<TKey>

{
    Task SaveAllChanges();
}
