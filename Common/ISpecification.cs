using System;
using System.Linq.Expressions;

namespace Common
{
    public interface ISpecification<TEntity>
    {
        Expression<Func<TEntity, bool>> ToExpression();

        bool IsSatisfiedBy(TEntity entity);
    }
}
