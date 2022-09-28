using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WhiteHole.Repository
{
    public abstract class RepositoryBase<T, K> : IRepositoryBase<T> where T : class where K : DbContext
    {
        protected K RepositoryContext { get; set; }

        public RepositoryBase(K repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool isTracking = false)
        {
            if (!isTracking)
            {
                return this.RepositoryContext.Set<T>().AsNoTracking().Where(expression);
            }
            return this.RepositoryContext.Set<T>().Where(expression);
        }

        public IQueryable<T> All()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public void Add(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        public async Task SaveAsync()
        {
            await this.RepositoryContext.SaveChangesAsync();
        }
    }
}
