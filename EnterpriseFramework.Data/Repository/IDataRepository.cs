using System.Linq;

namespace EnterpriseFramework.Data
{
    public interface IDataRepository<T> : IRepository//<T>
        where T : class
    {
        int Count();
        IQueryable<T> GetAll();
        T Find(int id);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);

        IQueryable<T> Where(System.Func<T, bool> func);
        

    }
}
