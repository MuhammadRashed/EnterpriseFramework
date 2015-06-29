using System;
using System.ComponentModel;
using System.Collections.Generic;
//using EnterpriseFramework.Model;

namespace EnterpriseFramework.Data
{
    /// <summary>
    /// The EnterpriseFramework.Data "Unit of Work"
    ///     1) decouples the repos from the controllers
    ///     2) decouples the DbContext and EF from the controllers
    ///     3) manages the UoW
    /// </summary>
    /// <remarks>
    /// This class implements the "Unit of Work" pattern in which
    /// the "UoW" serves as a facade for querying and saving to the database.
    /// Querying is delegated to "repositories".
    /// Each repository serves as a container dedicated to a particular
    /// root entity type such as a <see cref="Person"/>.
    /// A repository typically exposes "Get" methods for querying and
    /// will offer add, update, and delete methods if those features are supported.
    /// The repositories rely on their parent UoW to provide the interface to the
    /// data layer (which is the EF DbContext in EnterpriseFramework.Data).
    /// </remarks>
    public abstract class BaseUow<TContext> : IDisposable, IUow, EnterpriseFramework.Data.UnitOfWork.IBaseUow<TContext>
        where TContext : IDbContext
    {
        public BaseUow()
        {
            RepositoryProvider<TContext> repositoryProvider = new RepositoryProvider<TContext>(new RepositoryFactories<TContext>(GetEnterpriseFrameworkFactories()));
            Initiate();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;       
        }

        public virtual IDictionary<Type, Func<TContext, object>> GetEnterpriseFrameworkFactories()
        {
            return new Dictionary<Type, Func<TContext , object>>
                {
                    //{typeof(IAttendanceRepository), dbContext => new AttendanceRepository(dbContext)},
                    //{typeof(IPersonsRepository), dbContext => new PersonsRepository(dbContext)},
                    //{typeof(ISessionsRepository), dbContext => new SessionsRepository(dbContext)},
                };
        }

        // EnterpriseFramework.Data repositories

        //public IRepository<Room> Rooms { get { return GetStandardRepository<Room>(); } }
        //public IRepository<TimeSlot> TimeSlots { get { return GetStandardRepository<TimeSlot>(); } }
        //public IRepository<Track> Tracks { get { return GetStandardRepository<Track>(); } }
        //public ISessionsRepository Sessions { get { return GetRepo<ISessionsRepository>(); } }
        //public IPersonsRepository Persons { get { return GetRepo<IPersonsRepository>(); } }
        //public IAttendanceRepository Attendance { get { return GetRepo<IAttendanceRepository>(); } }

        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public void SaveChanges()
        {
            //System.Diagnostics.Debug.WriteLine("Committed");
            DbContext.SaveChanges();
        }

        //public enum IntiateStrategy
        //{
        //    None=0,
        //    SupportSerialization = 1,
        //    ValidateOnSaveEnabled = 2,

        //}

        //[DefaultValue(IntiateStrategy.None)]
        //public IntiateStrategy InitiationStrategy { get; set; }
        
        
        /// <summary>
        /// This is intitate DbContext and configure it
        /// </summary>
        protected void Initiate()
        {
            DbContext = Activator.CreateInstance<TContext>();
        }

        //{
            //DbContext = default(R);

            //if (((int)InitiationStrategy & (int)IntiateStrategy.SupportSerialization)==(int)IntiateStrategy.SupportSerialization )
            //{

            //    // Do NOT enable proxied entities, else serialization fails
            //    DbContext.Configuration.ProxyCreationEnabled = false;

            //    // Load navigation properties explicitly (avoid serialization trouble)
            //    DbContext.Configuration.LazyLoadingEnabled = false;
            //}
            //else
            //{

            //    DbContext.Configuration.ProxyCreationEnabled = true ;
            //    DbContext.Configuration.LazyLoadingEnabled = true ;
            //}

            //if (((int)InitiationStrategy & (int)IntiateStrategy.ValidateOnSaveEnabled) == (int)IntiateStrategy.ValidateOnSaveEnabled)
            //{
            //    // Because Web API will perform validation, we don't need/want EF to do so
            //    DbContext.Configuration.ValidateOnSaveEnabled = true ;
            //}
            //else
            //{
            //    DbContext.Configuration.ValidateOnSaveEnabled = false;
            //}




            
            ////DbContext.Configuration.AutoDetectChangesEnabled = false;
            //// We won't use this performance tweak because we don't need 
            //// the extra performance and, when autodetect is false,
            //// we'd have to be careful. We're not being that careful.
        //}

        protected RepositoryProvider<TContext> RepositoryProvider { get; set; }

        public IDataRepository<T> GetStandardRepository<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }
        public T GetRepositoryFromFactory<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        TContext _DbContext { get; set; }
        public TContext DbContext
        {
            get
            {
                return _DbContext;
            }
            set
            { _DbContext = value; }
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion

        public virtual System.Data.Entity.DbContextTransaction BeginTransaction()
        {
            var result = DbContext.BeginTransaction();
            return result;
        }
    }
}