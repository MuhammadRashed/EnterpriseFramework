using System;
using System.Collections.Generic;
using System.Data.Entity;
namespace EnterpriseFramework.Data.UnitOfWork
{
   public interface IBaseUow<TContext>
     where TContext : global::EnterpriseFramework.Data.IDbContext
    {
        DbContextTransaction BeginTransaction();
        TContext DbContext { get; set; }
        IDictionary<Type, Func<TContext, object>> GetEnterpriseFrameworkFactories();
        T GetRepositoryFromFactory<T>() where T : class;
        IDataRepository<T> GetStandardRepository<T>() where T : class;
        void SaveChanges();
    }
}
