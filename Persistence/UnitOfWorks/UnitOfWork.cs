using Application.Repositories.Commons;
using Application.UnitOfWorks;
using Domain.Entities.BaseEntities;
using Domain.Entities.Commons;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using Persistence.Repositories.Generics;




namespace Persistence.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{

    //Context
    private readonly ProductDbContext? _context;

    private Dictionary<Type, object> _readRepositories;
    private Dictionary<Type, object> _writeRepositories;


    public UnitOfWork(DbContext context)
    {
        _context = (ProductDbContext?)context;
        _readRepositories = new();
        _writeRepositories = new();
    }

    public IGenericReadRepository<TEntity, TKey> GetReadRepository<TEntity, TKey>()
                where TEntity : class, IBaseEntity<TKey>
    {
        if (_readRepositories.ContainsKey(typeof(TEntity)))
            return (GenericReadRepository<TEntity, TKey>)_readRepositories[typeof(TEntity)];


        var repository = new GenericReadRepository<TEntity, TKey>(_context!);
        _readRepositories.Add(typeof(TEntity), repository);

        return repository;
    }

    public IGenericWriteRepository<TEntity, TKey> GetWriteRepository<TEntity, TKey>()
               where TEntity : class, IBaseEntity<TKey>
    {
        if (_writeRepositories.ContainsKey(typeof(TEntity)))
            return (GenericWriteRepositor<TEntity, TKey>)_writeRepositories[typeof(TEntity)];


        var repository = new GenericWriteRepositor<TEntity, TKey>(_context);
        _writeRepositories.Add(typeof(TEntity), repository);

        return repository;
    }

    public async Task Commit()
    {
        try
        {
            await _context!.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException?.Message);
        }

    }
}
