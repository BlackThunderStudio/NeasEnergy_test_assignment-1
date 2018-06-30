using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLink
{
    public interface IDataAccessObject<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Persist(T t);
        void Update(T t);
        void Delete(int id);
    }
}
