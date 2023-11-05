using Microsoft.EntityFrameworkCore;
using PlantPalace.DataAccess.Data;
using PlantPalace.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            _db.Products.Include(u => u.Category).Include(u => u.categoryId);
        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? incluedProperties = null, bool tracked = true)
        {
            IQueryable<T> query;

            if(tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            query =  query.Where(filter);
            if (!string.IsNullOrEmpty(incluedProperties))
            {
                foreach (var incluedProp in incluedProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluedProp);
                }
            }
            return query.FirstOrDefault();
        }

        //Category,ProductType
        public IEnumerable<T> GetALL(Expression<Func<T, bool>>? filter, string? incluedProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(incluedProperties))
            {
                foreach(var incluedProp in incluedProperties.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluedProp);
                }
            }
           
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
