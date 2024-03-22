using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CommunicationServer.Protocols
{
    internal class RestAPI
    {
        private HttpListener listener;

        public Action<string> UpdatedMessage { get; set; }

        public void Initialize()
        {
            if (listener == null)
            {
                listener = new HttpListener();
                listener.Prefixes.Add(string.Format("http://+:9686/"));
                Start();
            }
        }

        private void Start()
        {
            if (!listener.IsListening)
            {
                listener.Start();
                Task.Factory.StartNew(() =>
                {
                    while (listener != null)
                    {
                        HttpListenerContext context = this.listener.GetContext();

                        string rawurl = context.Request.RawUrl;
                        string httpmethod = context.Request.HttpMethod;

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
