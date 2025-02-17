using System.Collections.Concurrent;
using Core.Contracts;
using Core.Enities;
using Core.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.Data;

public class UnitOfWork(StoreContext context) : IUnitOfWork
{
    private readonly ConcurrentDictionary<string, object> _repository = new();
    
    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        var type = typeof(TEntity).Name;

        return (IGenericRepository<TEntity>)_repository.GetOrAdd(type, t =>
        {
            var repositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
            return Activator.CreateInstance(repositoryType, context)
                   ?? throw new InvalidOperationException
                       ($"Could not create repository instance for {t}");
        });
    }

    public async Task<bool> Complete() =>
        await context.SaveChangesAsync() > 0;

    
    public void Dispose() =>
        context.Dispose();
}