using Core.Enities;

namespace Infrastructure.Repositories
{
    public interface IEntityRepository<T> where T : BaseEntity
    {
        public Task<T> GetByIdAsync(Guid id);
        public Task<IReadOnlyList<T>> GetListAsync();
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task RemoveAsync(Guid id);
        public Task SaveChangesAsync();
        public Task<bool> ExistsIdAsync(Guid id);
    }
}
