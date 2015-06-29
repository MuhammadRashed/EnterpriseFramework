using EnterpriseFramework.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseFramework.Data
{
        public class FakeDbContext<TFake> : IDbContext
            where TFake : class
        {
            protected class SetMap : KeyedCollection<Type, object>
            {
                public HashSet<T> Use<T>(IEnumerable<T> sourceData)
                {
                    var set = new HashSet<T>(sourceData);
                    if (Contains(typeof(T)))
                    {
                        Remove(typeof(T));
                    }
                    Add(set);
                    return set;
                }
                public HashSet<T> Get<T>() { return (HashSet<T>)this[typeof(T)]; }
                protected override Type GetKeyForItem(object item)
                {
                    return item.GetType().GetGenericArguments().Single();
                }
            }
            public DbContextTransaction BeginTransaction()
            {
                throw new NotImplementedException();
            }

            public IDbSet<T> Set<T>() where T : class
            {
                foreach (PropertyInfo property in typeof(TFake).GetProperties())
                {
                    if (property.PropertyType == typeof(IDbSet<T>) | property.PropertyType.GetInterfaces().Contains(typeof(IDbSet<T>)))
                    {
                        var result = property.GetValue(this, null) as IDbSet<T>;
                        if (result == null)
                        {
                            var obj = new FakeDbSet<T>();
                            property.SetValue(this, obj);
                            result = property.GetValue(this, null) as IDbSet<T>;
                        }
                        return result;

                    }
                }
                throw new Exception("Type collection not found , please make sure that there DbSet or FakeDbSet for the type in context");
            }

            public int SaveChanges()
            {
                return 0;
            }

            public void Dispose()
            {
            }


            public EntityState GetEntryState<T>(T entity) where T : class
            {
                if (Set<T>().Contains<T>(entity) ==false )
                {
                    return EntityState.Detached;
                }
                else
                {
                    return EntityState.Unchanged;
                }
            }

            public void SetEntryState<T>(T entity, EntityState State) where T : class
            {
            }
        }
}
