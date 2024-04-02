using CommunicationServer.Protocols;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommunicationServer
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
        public IServer Server { get; set; }
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
                    Server = new TcpIpServer();
                    break;
                case CommunicationMode.REST:
                    Server = new RestApiServer();
                    break;
                case CommunicationMode.IPC:
                    Server = new IpcServer();
                    break;
                case CommunicationMode.WCF:
                    Server = new TcpIpServer();
                    break;
            }

            Server.RecieveMessage += RecieveClientMessage;
        }

        private void ServerTbtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton button = (ToggleButton)sender;
            if (button.IsChecked.Value)
            {
                Server.On();
                if (Server.IsRunning())
                    OpenServer?.Invoke();
            }
            else
            {
                Server.Off();
                if (!Server.IsRunning())
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
            Server.Send(message);
            ShowMessage("Server --> " + "msg");

            MessageTbx.Text = string.Empty;
        }

        private void RecieveClientMessage(string message)
        {
            ShowMessage("Client --> " + message);
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
