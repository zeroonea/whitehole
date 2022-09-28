﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WhiteHole.Repository
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool isTracking = false);
        IQueryable<T> All();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
    }
}
