using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {

        IEnumerable<T> GetALL(Expression<Func<T, bool>>? filter = null, string? incluedProperties = null);

        T Get(Expression<Func<T, bool>> filter, string? incluedProperties = null,bool tracked = true);

        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }

}
