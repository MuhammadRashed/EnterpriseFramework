using EnterpriseFramework.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseFramework.Data
{
    public class FakeDbSet<T> : IDbSet<T>
    where T : class //, IModel  //, new()
    {
        ObservableCollection<T> _data;
        IQueryable _query;

        public FakeDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();
        }

        public T Find(params object[] keyValues)
        {
            var id = (int)keyValues[0];
            var result = _data.FirstOrDefault<T>(r => ((BaseModel)(object)r).Id == id);
            //var result = _data.FirstOrDefault<T>(r => ((dynamic)r).Id == id);
            return result;
        }

        public T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Remove(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Attach(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Detach(T item)
        {
            _data.Remove(item);
            return item;
        }

        //TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity;
        public TDerived Create<TDerived>()
        where TDerived : class , T //, new()
        {
            throw new Exception();
            //return new TDerived();
        }


        public T Create()
        {
            return Activator.CreateInstance<T>();
        }


        public ObservableCollection<T> Local
        {
            get { return _data; }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _query.Provider; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

         
    }


}
