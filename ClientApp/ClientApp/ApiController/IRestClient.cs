using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ApiController
{
    public interface IRestClient<T>
    {
        string Endpoint { get; set; }

        Task<T> GetSingle(string path);
        Task<IEnumerable<T>> GetCollection(string path);
        Task<T> Post(string path, T body);
        Task<T> Put(string path, T body);
        Task<T> Delete(string path);
    }
}
