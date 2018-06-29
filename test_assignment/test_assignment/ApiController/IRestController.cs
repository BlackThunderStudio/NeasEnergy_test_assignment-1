using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_assignment.ApiController
{
    public interface IRestController<T>
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task PersistAsync(T t);
        Task UpdateAsync(T t);
        Task DeleteAsync(T t);
    }
}
