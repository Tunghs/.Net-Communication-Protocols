using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CommunicationServer.Protocols
{
    internal class RestApiClient
    {
        private HttpListener _listener;
        public Action<string> UpdatedMessage { get; set; }

        public void Initialize()
        {
            if (_listener == null)
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add(string.Format("http://+:9686/"));
                Start();
            }
        }

        private void Start()
        {
            if (!_listener.IsListening)
            {
                _listener.Start();
                Task.Factory.StartNew(() =>
                {
                    while (_listener != null)
                    {
                        HttpListenerContext context = this._listener.GetContext();

                        string rawurl = context.Request.RawUrl;
                        string httpmethod = context.Request.HttpMethod;
                        string text;
                        using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                        {
                            text = reader.ReadToEnd();
                        }
                        string result = "";

                        result += string.Format("httpmethod = {0}\r\n", httpmethod);
                        result += string.Format("rawurl = {0}\r\n", rawurl);

                        UpdatedMessage?.Invoke(result);

                        context.Response.Close();

                    }
                });
            }
        }
    }
}
