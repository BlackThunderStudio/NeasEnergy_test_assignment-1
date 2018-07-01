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
        private IRestClient<Models.DatabaseModels.Salesperson> client;

        public SalespersonController()
        {
            client = new HttpRestHandler<Models.DatabaseModels.Salesperson>();
            Endpoint = Auth.BASE_ADDRESS;
        }

        public async Task<Salesperson> GetAsync(int id)
        {
            string path = $"{DEFAULT_PATH}/{id}";
            client.Endpoint = Endpoint;
            Models.DatabaseModels.Salesperson person = await client.GetSingle(path);
            if (person.IsFaulted) throw new ApiException(person.DataLayerArgumentException ?? person.DataLayerException);
            return new Salesperson().FromDatabaseModel(person);
        }

        public async Task<IEnumerable<Salesperson>> GetAllAsync()
        {
            client.Endpoint = Endpoint;
            IEnumerable<Models.DatabaseModels.Salesperson> people = await client.GetCollection(DEFAULT_PATH);
            if (people.ToList()[0].IsFaulted) throw new ApiException(people.ToList()[0].DataLayerArgumentException ?? people.ToList()[0].DataLayerException);
            return people.Select(x => new Salesperson().FromDatabaseModel(x)).AsEnumerable();
        }

        public async Task PersistAsync(Salesperson t)
        {
            var converted = t.ToDatabaseModel(t);
            client.Endpoint = Endpoint;
            var person = await client.Post(DEFAULT_PATH, converted);
            if (person.IsFaulted) throw new ApiException(person.DataLayerArgumentException ?? person.DataLayerException);
        }

        public async Task UpdateAsync(Salesperson t)
        {
            string path = $"{DEFAULT_PATH}/{t.Id}";
            client.Endpoint = Endpoint;
            var person = await client.Put(path, t.ToDatabaseModel(t));
            if (person.IsFaulted) throw new ApiException(person.DataLayerArgumentException ?? person.DataLayerException);
        }

        public async Task DeleteAsync(int id)
        {
            string path = $"{DEFAULT_PATH}/{id}";
            client.Endpoint = Endpoint;
            var person = await client.Delete(path);
            if (person.IsFaulted) throw new ApiException(person.DataLayerArgumentException ?? person.DataLayerException);
        }
    }
}
