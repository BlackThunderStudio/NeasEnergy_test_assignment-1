using ClientApp.Models;
using ClientApp.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ApiController
{
    public class SalespersonController : IRestController<Salesperson>
    {
        public const string DEFAULT_PATH = "/api/salesperson";
        public string Endpoint { get; set; }
        private IRestClient<Salesperson> client;

        public SalespersonController()
        {
            client = new RestClient<Salesperson>();
        }

        public async Task<Salesperson> GetAsync(int id)
        {
            string path = $"{DEFAULT_PATH}/{id}";
            client.Endpoint = Endpoint;
            Salesperson person = await client.GetSingle(path);
            if (person.IsFaulted) throw new ApiException(person.DataLayerArgumentException == null ? person.DataLayerException : person.DataLayerArgumentException);
            return person;
        }

        public async Task<IEnumerable<Salesperson>> GetAllAsync()
        {
            client.Endpoint = Endpoint;
            IEnumerable<Salesperson> people = await client.GetCollection(DEFAULT_PATH);
            if (people.ToList()[0].IsFaulted) throw new ApiException(people.ToList()[0].DataLayerArgumentException == null ? people.ToList()[0].DataLayerException : people.ToList()[0].DataLayerArgumentException);
            return people;
        }

        public async Task PersistAsync(Salesperson t)
        {
            client.Endpoint = Endpoint;
            Salesperson person = await client.Post(DEFAULT_PATH, t);
            if (person.IsFaulted) throw new ApiException(person.DataLayerArgumentException == null ? person.DataLayerException : person.DataLayerArgumentException);
        }

        public async Task UpdateAsync(Salesperson t)
        {
            string path = $"{DEFAULT_PATH}/{t.Id}";
            client.Endpoint = Endpoint;
            var person = await client.Put(path, t);
            if (person.IsFaulted) throw new ApiException(person.DataLayerArgumentException == null ? person.DataLayerException : person.DataLayerArgumentException);
        }

        public async Task DeleteAsync(int id)
        {
            string path = $"{DEFAULT_PATH}/{id}";
            client.Endpoint = Endpoint;
            var person = await client.Delete(path);
            if (person.IsFaulted) throw new ApiException(person.DataLayerArgumentException == null ? person.DataLayerException : person.DataLayerArgumentException);
        }
    }
}
