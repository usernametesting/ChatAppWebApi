using Application.Repositories.Commons;
using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Persistence.Repositories.Generics;

public class GenericReadRepository<TEntity, TKey> : GenericRepository<TEntity, TKey>, IGenericReadRepository<TEntity, TKey>
    where TEntity :class, IBaseEntity<TKey>
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _entity;
    public GenericReadRepository(DbContext context) : base(context)
    {
        _context = context;
        _entity = _context.Set<TEntity>();

    }
    public async Task<IQueryable<TEntity>> GetAllAsQueryableAsync() =>
        _entity;

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool IsTracking = true) =>
       await (IsTracking ?  _entity.ToListAsync() :  _entity.AsNoTracking().ToListAsync());

    public async Task<List<TEntity>> GetByExpression(Expression<Func<TEntity, bool>> expression, bool IsTracking = true) =>
        await (IsTracking ? _entity.Where(expression).ToListAsync()
            : _entity.Where(expression).AsNoTracking().ToListAsync());

    public async Task<TEntity?> GetByIdAsync(TKey Id, bool IsTracking = true) =>
         (IsTracking ? await _entity.FindAsync(Id)
            : await _entity.AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(Id)));

}
