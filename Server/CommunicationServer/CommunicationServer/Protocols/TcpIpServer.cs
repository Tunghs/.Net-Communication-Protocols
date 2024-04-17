using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace CommunicationServer.Protocols
{
    internal class TcpIpServer : IServer
    {
        #region Fields
        private Socket _server;
        private string _address;
        private int _portNumber;
        #endregion

        public TcpIpServer()
        {

        }

        public TcpIpServer(string address, int portNumber)
        {
            _address = address;
            _portNumber = portNumber;
        }

        public Action<string> RecieveMessage { get; set; }

        public void Off()
        {
            if (_server == null)
                return;

            _server.Close();
        }

        public void On()
        {
            try
            {
                _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _server.Bind(new IPEndPoint(IPAddress.Any, 8788));
                _server.Listen(1);

                SocketAsyncEventArgs sockAsync = new SocketAsyncEventArgs();
                sockAsync.Completed += new EventHandler<SocketAsyncEventArgs>(sockAsync_Completed);
                _server.AcceptAsync(sockAsync);
            }
            catch (Exception ex)
            {
                _server.Close();
            }
        }

        /// <summary>
        /// 연결 될 시 발생되는 이벤트
        /// </summary>
        /// <param name="sender">이벤트 발생시킨 소켓 </param>
        /// <param name="e"></param>
        private void sockAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                Socket client = e.AcceptSocket;
                byte[] name = new byte[100];
                client.Receive(name);

                String dataInfo = Encoding.UTF8.GetString(name).Trim().Replace("\0", "");

                SocketAsyncEventArgs receiveAsync = new SocketAsyncEventArgs();
                receiveAsync.Completed += new EventHandler<SocketAsyncEventArgs>(receiveAsync_Completed);
                receiveAsync.SetBuffer(new byte[4096], 0, 4096);
                receiveAsync.UserToken = client;
                client.ReceiveAsync(receiveAsync);

                e.AcceptSocket = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Send(string data)
        {
            throw new NotImplementedException();
        }

        public bool IsRunning()
        {
            return false;
        }

        /// <summary>
        /// 응답이 올 경우 발생하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void receiveAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            Socket client = (Socket)sender;
            String Name = string.Empty;

            if (client.Connected && e.BytesTransferred > 0)
            {
                int length = e.Buffer.Length;
                byte[] data = new byte[length];

                client.Receive(data, length, SocketFlags.None);
                String dataInfo = Encoding.UTF8.GetString(data).Replace("\0", "");

                if (dataInfo == "")
                    return;

                RecieveMessage?.Invoke(dataInfo);

                client.ReceiveAsync(e);
            }
        }
    }
}
