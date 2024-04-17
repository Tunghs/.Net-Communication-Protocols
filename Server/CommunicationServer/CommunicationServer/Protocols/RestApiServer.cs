using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace CommunicationServer.Protocols
{
    internal class RestApiServer: IServer
    {
        private HttpListener _listener;
        public Action<string> RecieveMessage { get; set; }
        private bool _isRunning;

        public RestApiServer()
        {

        }

        public bool IsRunning()
        {
            return _isRunning;
        }

        public void Off()
        {
            _listener = null;
            _isRunning = false;
        }

        public void On()
        {
            if (_listener == null)
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add(string.Format("http://+:8788/"));
                _isRunning = true;
                Start();
            }
        }

        public void Send(string data)
        {
            throw new NotImplementedException();
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
                        HttpListenerContext context = _listener.GetContext();

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

                        string originalText = HttpUtility.UrlDecode(text.Split('=')[1], System.Text.Encoding.UTF8);
                        RecieveMessage?.Invoke(originalText);

                        // 클라이언트에게 응답 보내기
                        HttpListenerResponse response = context.Response;
                        response.StatusCode = 202;

                        context.Response.Close();
                    }
                });
            }
        }
    }
}