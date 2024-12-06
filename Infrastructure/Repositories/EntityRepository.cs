using Core.Enities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

namespace Infrastructure.Repositories
{
    public class EntityRepository<T>(StoreContext context) : IEntityRepository<T> where T : BaseEntity
    {
        public async Task AddAsync(T entity)
        {
            context.Set<T>().Add(entity);
            await SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync() =>
             await context.Set<T>().ToListAsync();

        public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec) =>
            await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec) =>
            await ApplySpecification(spec).ToListAsync();

        public async Task<TResult?> GetEntityWithSpecAsync<TResult>(ISpecification<T, TResult> spec) =>
            await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec) =>
            await ApplySpecification(spec).ToListAsync();

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await context.Set<T>().FindAsync(id);

            if (entity == null || !await ExistsIdAsync(id))
                throw new InvalidOperationException("Entity was not found");

            return entity;
        }
        public async Task UpdateAsync(T entity)
        {
            if (!await ExistsIdAsync(entity.Id))
                throw new InvalidOperationException("Can not update this entity");

            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;

            await SaveChangesAsync();
        }
        public async Task RemoveAsync(Guid id)
        {
            var product = await GetByIdAsync(id);
            context.Set<T>().Remove(product);
            await SaveChangesAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var query = context.Set<T>().AsQueryable();
            query = spec.ApplyCriteria(query);

            return await query.CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec) =>
            SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);

        private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec) =>
            SpecificationEvaluator<T>.GetQuery<T, TResult>(context.Set<T>().AsQueryable(), spec);

        private async Task<bool> ExistsIdAsync(Guid id) =>
           await context.Set<T>().AnyAsync(x => x.Id == x.Id);

        private async Task SaveChangesAsync()
        {
            if (await context.SaveChangesAsync() <= 0)
                throw new InvalidOperationException("Could not save changes");
        }
    }
}
