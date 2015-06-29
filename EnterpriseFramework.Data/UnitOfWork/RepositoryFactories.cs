using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace EnterpriseFramework.Data
{
    /// <summary>
    /// A maker of EnterpriseFramework.Data Repositories.
    /// </summary>
    /// <remarks>
    /// An instance of this class contains repository factory functions for different types.
    /// Each factory function takes an EF <see cref="R"/> and returns
    /// a repository bound to that R.
    /// <para>
    /// Designed to be a "Singleton", configured at web application start with
    /// all of the factory functions needed to create any type of repository.
    /// Should be thread-safe to use because it is configured at app start,
    /// before any request for a factory, and should be immutable thereafter.
    /// </para>
    /// </remarks>
    public class RepositoryFactories<R>
        where R : IDbContext
    {
        /// <summary>
        /// Return the runtime EnterpriseFramework.Data repository factory functions,
        /// each one is a factory for a repository of a particular type.
        /// </summary>
        /// <remarks>
        /// MODIFY THIS METHOD TO ADD CUSTOM EnterpriseFramework.Data FACTORY FUNCTIONS
        /// </remarks>
        //private IDictionary<Type, Func<R, object>> GetEnterpriseFrameworkFactories()
        //{
        //    return new Dictionary<Type, Func<R, object>>
        //        {
        //           //{typeof(IAttendanceRepository), R => new AttendanceRepository(R)},
        //           //{typeof(IPersonsRepository), R => new PersonsRepository(R)},
        //           //{typeof(ISessionsRepository), R => new SessionsRepository(R)},
        //        };
        //}

        private IDictionary<Type, Func<R, object>> GetEnterpriseFrameworkFactories;
        

        /// <summary>
        /// Constructor that initializes with runtime EnterpriseFramework.Data repository factories
        /// </summary>
        //public RepositoryFactories()  
        //{
        //    //_repositoryFactories = GetEnterpriseFrameworkFactories();
            
        //}

        /// <summary>
        /// Constructor that initializes with an arbitrary collection of factories
        /// </summary>
        /// <param name="factories">
        /// The repository factory functions for this instance. 
        /// </param>
        /// <remarks>
        /// This ctor is primarily useful for testing this class
        /// </remarks>
        public RepositoryFactories(IDictionary<Type, Func<R, object>> factories )
        {
            _repositoryFactories = factories;
        }

        /// <summary>
        /// Get the repository factory function for the type.
        /// </summary>
        /// <typeparam name="T">Type serving as the repository factory lookup key.</typeparam>
        /// <returns>The repository function if found, else null.</returns>
        /// <remarks>
        /// The type parameter, T, is typically the repository type 
        /// but could be any type (e.g., an entity type)
        /// </remarks>
        public Func<R, object> GetRepositoryFactory<T>()
        {
       
            Func<R, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            return factory;
        }

        /// <summary>
        /// Get the factory for <see cref="IDataRepository{T}"/> where T is an entity type.
        /// </summary>
        /// <typeparam name="T">The root type of the repository, typically an entity type.</typeparam>
        /// <returns>
        /// A factory that creates the <see cref="IDataRepository{T}"/>, given an EF <see cref="R"/>.
        /// </returns>
        /// <remarks>
        /// Looks first for a custom factory in <see cref="_repositoryFactories"/>.
        /// If not, falls back to the <see cref="DefaultEntityRepositoryFactory{T}"/>.
        /// You can substitute an alternative factory for the default one by adding
        /// a repository factory for type "T" to <see cref="_repositoryFactories"/>.
        /// </remarks>
        public Func<R, object> GetRepositoryFactoryForEntityType<T>() where T : class
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        }

        /// <summary>
        /// Default factory for a <see cref="IDataRepository{T}"/> where T is an entity.
        /// </summary>
        /// <typeparam name="T">Type of the repository's root entity</typeparam>
        protected virtual Func<R, object> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return R => new EFRepository<T>(R);
        }

        /// <summary>
        /// Get the dictionary of repository factory functions.
        /// </summary>
        /// <remarks>
        /// A dictionary key is a System.Type, typically a repository type.
        /// A value is a repository factory function
        /// that takes a <see cref="R"/> argument and returns
        /// a repository object. Caller must know how to cast it.
        /// </remarks>
        private readonly IDictionary<Type, Func<R, object>> _repositoryFactories;

    }
}
