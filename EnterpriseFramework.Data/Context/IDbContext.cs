using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseFramework.Data
{
    public interface IDbContext : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        //DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        //Database Database { get; }

        //public virtual void Delete<T>(T entity) where T : class;
        //public virtual void Update<T>(T entity) where T : class;
        //public virtual T Add<T>(T entity) where T : class;

        EntityState GetEntryState<T>(T entity) where T : class;
        void SetEntryState<T>(T entity, EntityState State) where T : class;


        int SaveChanges();

        DbContextTransaction BeginTransaction();

    }
}
