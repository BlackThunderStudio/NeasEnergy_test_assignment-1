using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Web;

namespace ClientApp.ApiController
{

    class RestClient<TResult>
    {
        public string Endpoint { get; set; }

        public RestClient()
        {
            Endpoint = String.Empty;
        }

        public async Task<TResult> MakeRequest(string path)
        {
            HttpClient client = new HttpClient();
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
    }
}
