using Application.Repositories.Commons;
using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Generics;

public class GenericWriteRepositor<TEntity, TKey> : GenericRepository<TEntity, TKey>,
    IGenericWriteRepository<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _entity;
    public GenericWriteRepositor(DbContext context) : base(context)
    {
        _context = context;
        _entity = _context.Set<TEntity>();

    }
    public async Task AddAsync(TEntity entity) =>
        await _entity.AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities) =>
        await _entity.AddRangeAsync(entities);

    public async Task Delete(TEntity entity) =>
        _entity.Remove(entity);

    public async Task DeleteById(TKey Id)
    {
        var entity = await _entity.FirstOrDefaultAsync(e => e.Id.Equals(Id));
        _entity.Remove(entity!);
    }

    public async Task Patch(TEntity entity) =>
        _entity.Update(entity);

    public async Task Update(TKey Id, TEntity model)
    {
        var entity = await _entity.FirstOrDefaultAsync(e => e.Id.Equals(Id));
        foreach (var prop in model.GetType().GetProperties())
        {
            var result = entity?.GetType().GetProperty(prop.Name);
            if (result != null)
            {
                var entityPropertyType = result.PropertyType;

                if (typeof(IEnumerable).IsAssignableFrom(entityPropertyType) && entityPropertyType != typeof(string))
                {
                    var entityList = result.GetValue(entity) as IList;

                    if (entityList != null && prop.GetValue(model) is IEnumerable modelList)
                        foreach (var item in modelList)
                            entityList.Add(item);
                }
                else
                    result.SetValue(entity, prop.GetValue(model));
            }
        }
    }
    public async Task Update(Dictionary<string, object> obj, TKey Id)
    {
        var entity = await _entity.FirstOrDefaultAsync(e => e.Id.Equals(Id));

        foreach (var keyValue in obj)
        {
            var propertyInfo = entity.GetType().GetProperty(keyValue.Key);
            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(entity, keyValue.Value);
            }
        }

        _entity.Update(entity!);

    }


}
