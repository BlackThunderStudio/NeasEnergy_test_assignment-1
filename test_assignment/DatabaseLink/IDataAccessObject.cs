using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLink
{
    interface IDataAccessObject<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        T Persist(T t);
        void Update(T t);
        void Delete(T t);
    }
}
