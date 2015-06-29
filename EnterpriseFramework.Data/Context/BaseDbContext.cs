using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseFramework.Data
{
    public abstract class BaseDbContext : DbContext , IDbContext
    {
        public BaseDbContext (): base(){}
        protected BaseDbContext(DbCompiledModel model) : base(model) { }
        public BaseDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public BaseDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) { }

        public BaseDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext) { }
        public BaseDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model) { }
        public BaseDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection) { }


        public virtual void SetEntryState<T>(T entity, EntityState State) where T : class
        {
            DbEntityEntry dbEntityEntry = Entry(entity);
            dbEntityEntry.State = State;
        }
        public virtual EntityState GetEntryState<T>(T entity) where T : class
        {
            DbEntityEntry dbEntityEntry = Entry(entity);
            return dbEntityEntry.State ;
        }
        

        public DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }



        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}
