using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class SpecificationRunner<TEntity>
    {
        public IEnumerable<TEntity> Find(Specification<TEntity> specification, IQueryable<TEntity> list)
        {
            return list
            .Where(specification.ToExpression())
            .ToList();
        }
    }
}
