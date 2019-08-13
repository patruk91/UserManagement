using System.Collections.Generic;

namespace UserManagement.AL
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(string id);
        bool Add(T t);
        bool Delete(T t);
        bool Edit(T t);

    }
}