using System;
using System.Collections.Generic;

namespace UserManagement.AL
{
    public class RepositorySQL<T> : IRepository<T>
    {
        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T Get(string id)
        {
            throw new NotImplementedException();
        }

        public bool Add(T t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T t)
        {
            throw new NotImplementedException();
        }

        public bool Edit(T t)
        {
            throw new NotImplementedException();
        }
    }
}
