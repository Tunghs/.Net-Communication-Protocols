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
    internal class TcpIpServer
    {
        #region Fields
        private Socket _server;
        private string _address;
        private int _portNumber;
        #endregion

        public TcpIpServer(string address, int portNumber)
        {
            _address = address;
            _portNumber = portNumber;
        }

        public void Connect()
        {
            try
            {
                _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                _server.Bind(new IPEndPoint(IPAddress.Parse(_address), _portNumber));
                _server.Listen(1);

                SocketAsyncEventArgs sockAsync = new SocketAsyncEventArgs();
                sockAsync.Completed += new EventHandler<SocketAsyncEventArgs>(Completed);
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
        private void Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                Socket server = (Socket)sender;
                Socket client = e.AcceptSocket;

                byte[] name = new byte[100];
                client.Receive(name);

                //16진수 -> string형으로 parsing
                String str_data = Encoding.UTF8.GetString(name).Trim().Replace("\0", "");

                //string -> JObject parsing
                //JObject jobj = JObject.Parse(str_data);

                ////json형식에서 key가 text인 value값을 string형에 담기
                //string user = jobj["text"].ToString();

                SocketAsyncEventArgs receiveAsync = new SocketAsyncEventArgs();
                receiveAsync.Completed += new EventHandler<SocketAsyncEventArgs>(receiveAsync_Completed);
                receiveAsync.SetBuffer(new byte[4096], 0, 4096);
                receiveAsync.UserToken = client;
                client.ReceiveAsync(receiveAsync);

                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine("*********** " + user + "- Connected ***********");
                //sb.AppendLine();

                //Invoke((MethodInvoker)delegate
                //{
                //    rtb_text.AppendText(sb.ToString());
                //    lb_client_list.Items.Add(user);
                //});

                e.AcceptSocket = null;
                server.AcceptAsync(e);
            }
            catch (Exception ex)
            {

            }
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

                //dataInfo = crypto.AESDecrypt256(dataInfo, cryptoKey);
                //JObject jobj = JObject.Parse(dataInfo);

                //if (searchSocket(client) == "")
                //{
                //    MessageBox.Show("접속 클라이언트를 찾을 수 없습니다.");
                //    return;
                //}
                //else
                //    Name = searchSocket(client);


                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine("[" + Name + "]" + " -----> " + jobj["text"].ToString());

                //Invoke((MethodInvoker)delegate
                //{
                //    rtb_text.AppendText(sb.ToString());
                //});

                client.ReceiveAsync(e);

            }
        }
    }
}
