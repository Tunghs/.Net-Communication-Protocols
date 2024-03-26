using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CommunicationClient.Protocols
{
    public class RestAPI
    {
        private HttpClient _client;
        private string _url = "http://127.0.0.1:9686/";

        public RestAPI() 
        {
            _client = new HttpClient();
        }

        public async Task<HttpResponseMessage> PostAsync(Dictionary<string, string> data)
        {
            //var values =
            //    new Dictionary<string, string> { { "username", "myUser" }, { "password", "myPassword" } };

            var postData = new FormUrlEncodedContent(data);
            var response = await _client.PostAsync(_url, postData);
            return response;
        }
    }
}
