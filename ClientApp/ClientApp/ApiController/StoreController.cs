using ClientApp.Models.DatabaseModels;
using ClientApp.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ApiController
{
    public class StoreController : IRestController<Store>
    {
        public string Endpoint { get; set; }
        public const string DEFAULT_PATH = "/api/store";
        private IRestClient<Store> client;

        public StoreController()
        {
            client = new HttpRestHandler<Store>();
        }

        public async Task DeleteAsync(int id)
        {
            string path = $"{DEFAULT_PATH}/{id}";
            client.Endpoint = Endpoint;
            var store = await client.Delete(path);
            if (store.IsFaulted) throw new ApiException(store.DataLayerArgumentException ?? store.DataLayerException);
        }

        public async Task<IEnumerable<Store>> GetAllAsync()
        {
            client.Endpoint = Endpoint;
            IEnumerable<Store> stores = await client.GetCollection(DEFAULT_PATH);
            if (stores.ToList()[0].IsFaulted) throw new ApiException(stores.ToList()[0].DataLayerArgumentException ?? stores.ToList()[0].DataLayerException);
            return stores;
        }

        public async Task<Store> GetAsync(int id)
        {
            string path = $"{DEFAULT_PATH}/{id}";
            client.Endpoint = Endpoint;
            var store = await client.GetSingle(path);
            if (store.IsFaulted) throw new ApiException(store.DataLayerArgumentException ?? store.DataLayerException);
            return store;
        }

        public async Task PersistAsync(Store t)
        {
            client.Endpoint = Endpoint;
            var store = await client.Post(DEFAULT_PATH, t);
            if (store.IsFaulted) throw new ApiException(store.DataLayerArgumentException ?? store.DataLayerException);
        }

        public async Task UpdateAsync(Store t)
        {
            string path = $"{DEFAULT_PATH}/{t.Id}";
            client.Endpoint = Endpoint;
            var store = await client.Put(path, t);
            if (store.IsFaulted) throw new ApiException(store.DataLayerArgumentException ?? store.DataLayerException);
        }
    }
}
