using CommunicationClient.Protocols;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CommunicationClient
{
    /// <summary>
    /// CommunicationView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CommunicationView : UserControl
    {
        #region Event
        public Action OpenServer;
        public Action CloseServer;
        #endregion

        #region Properties
        public IClient Client { get; set; }
        #endregion

        public CommunicationView()
        {
            InitializeComponent();
        }

        public void Intialize(CommunicationMode mode)
        {
            switch (mode)
            {
                case CommunicationMode.TcpIp:
                    Client = new TcpIpClient();
                    break;
                case CommunicationMode.REST:
                    Client = new RestApiClient();
                    break;
                case CommunicationMode.IPC:
                    Client = new IpcClient();
                    break;
                case CommunicationMode.WCF:
                    Client = new IpcClient();
                    break;
            }

            Client.RecieveMessage += RecieveServerMessage;
        }

        private void ClientTbtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton button = (ToggleButton)sender;
            if (button.IsChecked.Value)
            {
                Client.On();
                if (Client.IsRunning())
                    OpenServer?.Invoke();
            }
            else
            {
                Client.Off();
                if (!Client.IsRunning())
                    CloseServer?.Invoke();
            }
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MessageTbx.Text))
            {
                return;
            }

            string message = MessageTbx.Text;
            Client.Send(message);
            ShowMessage("Client --> " + message);

            MessageTbx.Text = string.Empty;
        }

        private void RecieveServerMessage(string message)
        {
            ShowMessage("Server --> " + message);
        }

        private void ShowMessage(string message)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                LogTbx.AppendText(message);
                LogTbx.AppendText(Environment.NewLine);
                LogTbx.ScrollToEnd();
            }));
        }
    }
}
