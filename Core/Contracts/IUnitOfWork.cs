using Core.Enities;
using Core.Interfaces;

namespace Core.Contracts;

public interface IUnitOfWork : IDisposable
{
    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    public Task<bool> Complete();
}