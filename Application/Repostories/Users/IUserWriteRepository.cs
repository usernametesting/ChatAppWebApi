using Application.Repositories.Commons;
using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Users;

public interface IUserWriteRepository<TEntity, TKey> : IGenericWriteRepository<TEntity, TKey>
    where TEntity : IBaseEntity<TKey>
{
}
