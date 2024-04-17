using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationClient.Protocols
{
    internal class RestApiClient : IClient
    {
        private HttpClient _client;
        private string _url = "http://127.0.0.1:8788/";
        static bool isRunning = false;

        public Action<string> RecieveMessage { get; set; }

        public bool IsRunning()
        {
            return isRunning;
        }

        public void Off()
        {
            _client.Dispose();
            isRunning = false;
        }

        public void On()
        {
            _client = new HttpClient();
            isRunning = true;
        }

        public async void Send(string data)
        {
            await PostAsync(new Dictionary<string, string> { { "text", data } });
        }

        public async Task<HttpResponseMessage> PostAsync(Dictionary<string, string> data)
        {
            var postData = new FormUrlEncodedContent(data);
            var response = await _client.PostAsync(_url, postData);

            return response;
        }
    }
}
