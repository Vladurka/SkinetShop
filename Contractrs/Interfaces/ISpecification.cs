using System;
using System.Linq.Expressions;
namespace Contracts.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}
