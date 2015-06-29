using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;


namespace EnterpriseFramework.Data
{
    /// <summary>
    /// The EF-dependent, generic repository for data access
    /// </summary>
    /// <typeparam name="T">Type of entity for this Repository.</typeparam>
    public class EFRepository<T> : IDataRepository<T> where T : class
    {
        public EFRepository(IDbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        protected IDbContext DbContext { get; set; }

        protected IDbSet<T> DbSet { get; set; }

        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public virtual T Find(int id)
        {
            //return DbSet.FirstOrDefault(PredicateBuilder.GetByIdPredicate<T>(id));
            return DbSet.Find(id);
        }

        public virtual T Add(T entity)
        {
            //DbContext.SetEntryState<T>(entity , EntityState.Added);
            DbContext.Set<T>().Add(entity);
            //if (dbEntityEntry.State != EntityState.Detached)
            //{
            //    dbEntityEntry.State = EntityState.Added;
            //}
            //else
            //{
            //    return entity;
            //}
            return entity;
        }

        public virtual void Update(T entity)
        {
            var state = DbContext.GetEntryState<T>(entity);
            if (state == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbContext.SetEntryState<T>(entity, EntityState.Modified);
        }

        public virtual void Delete(T entity)
        {
            var state = DbContext.GetEntryState<T>(entity);
            //if (state != EntityState.Deleted)
            //{
            //    DbContext.SetEntryState<T>(entity, EntityState.Deleted);
            //}
            //else
            //{
            //    DbSet.Attach(entity);
            //    DbSet.Remove(entity);
            //}
            if (state == EntityState.Detached)
            {
                DbContext.SetEntryState<T>(entity, EntityState.Deleted);
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        public virtual void Delete(int id)
        {
            var entity = Find(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }


        public IQueryable<T> Where(Func<T, bool> func)
        {
            var result = DbSet.Where(func);
            return result.AsQueryable();

        }

        public int Count()
        {
            return DbSet.Count();
        }
    }
}
