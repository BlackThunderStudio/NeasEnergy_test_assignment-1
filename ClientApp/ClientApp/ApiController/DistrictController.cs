using ClientApp.Models;
using ClientApp.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ApiController
{
    public class DistrictController : IRestController<District>, ISecondarySalesman
    {
        public string Endpoint { get; set; }
        public const string DEFAULT_PATH = "/api/district";
        private IRestClient<District> client;

        public DistrictController()
        {
            client = new RestClient<District>();
        }

        public async Task DeleteAsync(int id)
        {
            string path = $"{DEFAULT_PATH}/{id}";
            client.Endpoint = Endpoint;
            var district = await client.Delete(path);
            if (district.IsFaulted) throw new ApiException(district.DataLayerArgumentException == null ? district.DataLayerException : district.DataLayerArgumentException);
        }

        public async Task<IEnumerable<District>> GetAllAsync()
        {
            client.Endpoint = Endpoint;
            IEnumerable<District> districts = await client.GetCollection(DEFAULT_PATH);
            if (districts.ToList()[0].IsFaulted) throw new ApiException(districts.ToList()[0].DataLayerArgumentException == null ? districts.ToList()[0].DataLayerException : districts.ToList()[0].DataLayerArgumentException);
            return districts;
        }

        public async Task<District> GetAsync(int id)
        {
            string path = $"{DEFAULT_PATH}/{id}";
            client.Endpoint = Endpoint;
            var district = await client.GetSingle(path);
            if (district.IsFaulted) throw new ApiException(district.DataLayerArgumentException == null ? district.DataLayerException : district.DataLayerArgumentException);
            return district;
        }

        public async Task PersistAsync(District t)
        {
            client.Endpoint = Endpoint;
            var district = await client.Post(DEFAULT_PATH, t);
            if (district.IsFaulted) throw new ApiException(district.DataLayerArgumentException == null ? district.DataLayerException : district.DataLayerArgumentException);
        }

        public async Task UpdateAsync(District t)
        {
            string path = $"{DEFAULT_PATH}/{t.Id}";
            client.Endpoint = Endpoint;
            var district = await client.Put(path, t);
            if (district.IsFaulted) throw new ApiException(district.DataLayerArgumentException == null ? district.DataLayerException : district.DataLayerArgumentException);
        }

        public async Task AssignSecondaryAsync(Salesperson person, District district)
        {
            string path = $"{DEFAULT_PATH}/{district.Id}/secondary-sales/add/{person.Id}";
            client.Endpoint = Endpoint;
            var response = await client.Post(path, new District());
            if (response.IsFaulted) throw new ApiException(response.DataLayerArgumentException == null ? response.DataLayerException : response.DataLayerArgumentException);
        }

        public async Task DeleteSecondaryAsync(Salesperson person, District district)
        {
            string path = $"{DEFAULT_PATH}/{district.Id}/secondary-sales/delete/{person.Id}";
            client.Endpoint = Endpoint;
            var response = await client.Post(path, new District());
            if (response.IsFaulted) throw new ApiException(response.DataLayerArgumentException == null ? response.DataLayerException : response.DataLayerArgumentException);
        }
    }
}
