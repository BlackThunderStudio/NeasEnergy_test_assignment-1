using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace test_assignment.ApiController
{
    public enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class RestClient<TResult>
    {
        public string Endpoint { get; set; }
        public HttpVerb Method { get; set; }

        private HttpClient client;

        public RestClient()
        {
            Endpoint = String.Empty;
            Method = HttpVerb.GET;
            client = new HttpClient();
        }

        public async Task<TResult> MakeRequest(string path)
        {
            TResult result = default(TResult);

            HttpResponseMessage response = await client.GetAsync(Endpoint + path);
            if (response.IsSuccessStatusCode)
            {
                string serialized = await response.Content.ReadAsStringAsync();
                var serializer = new XmlSerializer(typeof(TResult));
                using(TextReader reader = new StringReader(serialized))
                {
                    result = (TResult)serializer.Deserialize(reader);
                }
            }
            return result;
        }

        public async Task SendRequest(string path, TResult value)
        {
            throw new NotImplementedException();
        }

    }
}
