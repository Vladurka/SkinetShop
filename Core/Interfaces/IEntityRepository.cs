using Core.Enities;

namespace Core.Interfaces
{
    public interface IEntityRepository<T> where T : BaseEntity
    {
        public Task<T> GetByIdAsync(Guid id);
        public Task<IReadOnlyList<T>> ListAllAsync();
        public Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec);
        public Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        public Task<TResult?> GetEntityWithSpecAsync<TResult>(ISpecification<T, TResult> spec);
        public Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task RemoveAsync(Guid id);
        public Task<int> CountAsync(ISpecification<T> spec);
    }
}
