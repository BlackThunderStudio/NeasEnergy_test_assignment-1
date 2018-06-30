using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Web;
using System.Net.Http.Headers;

namespace ClientApp.ApiController
{

    class HttpRestHandler<TResult> : IRestClient<TResult>
    {
        public string Endpoint { get; set; }

        public HttpRestHandler()
        {
            Endpoint = String.Empty;
        }

        public async Task<TResult> GetSingle(string path)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("API_KEY", Auth.API_KEY);
            TResult result = default(TResult);
            client.BaseAddress = new Uri(Endpoint);

            HttpResponseMessage response = await client.GetAsync(Endpoint + path);
            if (response.IsSuccessStatusCode)
            {
                //string serialized = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(serialized);
                //var serializer = new XmlSerializer(typeof(TResult));
                //using (TextReader reader = new StringReader(serialized))
                //{
                //    result = (TResult)serializer.Deserialize(reader);
                //}
                result = await response.Content.ReadAsAsync<TResult>();
            }
            return result;
        }

        public async Task<IEnumerable<TResult>> GetCollection(string path)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("API_KEY", Auth.API_KEY);
            IEnumerable<TResult> result = new List<TResult>();
            client.BaseAddress = new Uri(Endpoint);
            HttpResponseMessage response = await client.GetAsync(Endpoint + path);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<IEnumerable<TResult>>();
            }
            return result.AsEnumerable();
        }

        public async Task<TResult> Post(string path, TResult body)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("API_KEY", Auth.API_KEY);
            TResult result = default(TResult);
            client.BaseAddress = new Uri(Endpoint);
            HttpResponseMessage response = await client.PostAsJsonAsync(Endpoint + path, body);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<TResult>();
            }
            response.EnsureSuccessStatusCode();
            return result;
        }

        public async Task<TResult> Put(string path, TResult body)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("API_KEY", Auth.API_KEY);
            TResult result = default(TResult);
            client.BaseAddress = new Uri(Endpoint);
            HttpResponseMessage response = await client.PutAsJsonAsync(Endpoint + path, body);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<TResult>();
            }
            response.EnsureSuccessStatusCode();
            return result;
        }

        public async Task<TResult> Delete(string path)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("API_KEY", Auth.API_KEY);
            TResult result = default(TResult);
            client.BaseAddress = new Uri(Endpoint);
            HttpResponseMessage response = await client.DeleteAsync(Endpoint + path);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<TResult>();
            }
            response.EnsureSuccessStatusCode();
            return result;
        }
    }
}
