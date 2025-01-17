using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface ISpecification<T>
    {
        public Expression<Func<T, bool>>? Criteria { get; }
        public Expression<Func<T, object>>? OrderBy { get; }
        public Expression<Func<T, object>>? OrderByDesc { get; }

        public bool IsDistinct { get; }
        public int Take { get; }
        public int Skip { get; }
        public bool IsPagingEnabled { get; }

        public IQueryable<T> ApplyCriteria(IQueryable<T> query);
    }

    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        public Expression<Func<T, TResult>>? Select { get; }
    }
}
