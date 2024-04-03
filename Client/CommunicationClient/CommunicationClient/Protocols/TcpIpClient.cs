using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace CommunicationClient.Protocols
{
    internal class TcpIpClient : IClient
    {
        private Socket _client;
        static bool isRunning = false;
        public Action<string> RecieveMessage { get; set; }

        public bool IsRunning()
        {
            return isRunning;
        }

        public void Off()
        {
            _client.Close();
            isRunning = false;
        }

        public void On()
        {
            try
            {
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _client.Connect(IPAddress.Parse("127.0.0.1"), 12000);

                SocketAsyncEventArgs receiveAsync = new SocketAsyncEventArgs();
                receiveAsync.Completed += new EventHandler<SocketAsyncEventArgs>(receiveAsync_Completed);
                receiveAsync.SetBuffer(new byte[4], 0, 4);
                receiveAsync.UserToken = _client;

                _client.ReceiveAsync(receiveAsync);
                isRunning = true;
            }
            catch (Exception ex)
            {
                _client.Close();
            }
        }

        private void receiveAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                Socket clientSock = (Socket)sender;
                if (clientSock.Connected && e.BytesTransferred > 0)
                {
                    byte[] lengthByte = e.Buffer;
                    int length = BitConverter.ToInt32(lengthByte, 0);
                    byte[] data = new byte[length];
                    clientSock.Receive(data, length, SocketFlags.None);

                    string dataInfo = Encoding.UTF8.GetString(data);

                    RecieveMessage?.Invoke(dataInfo);
                    clientSock.ReceiveAsync(e);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        public void Send(string data)
        {
            if(_client == null || _client.Connected == false)
            {
                MessageBox.Show("서버 접속이 필요합니다.");
                return;
            }
            try
            {
                //JObject data = new JObject();

                //data.Add("userID", tb_userid.Text);
                //data.Add("type", "op");
                //data.Add("text", rtb_send_Text.Text);

                //string make_data = JsonConvert.SerializeObject(data);

                // byte[] send_text = Encoding.UTF8.GetBytes(crypto.AESEncrypt256(make_data, cryptoKey));
                byte[] send_text = Encoding.UTF8.GetBytes(data);

                _client.Send(send_text);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
