using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using DatabaseLink.mapper;

namespace API.MessageHandlers
{
    public class ApiKeyMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            bool validKey = false;
            IEnumerable<string> requestHeaders;
            var KeyPresent = httpRequestMessage.Headers.TryGetValues("API_KEY", out requestHeaders);
            if (KeyPresent)
            {
                DBApiAuth db = new DBApiAuth();
                validKey = db.Validate(requestHeaders.FirstOrDefault());
            }
            if (!validKey)
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden, "Invalid API Key!");
            }
            var response = await base.SendAsync(httpRequestMessage, cancellationToken);
            return response;
        }
    }
}