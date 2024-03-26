using CommunicationClient.Protocols;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommunicationClient
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestAPI _client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MessageTbx.Text))
            {
                return;
            }

            if (RestApiBtn.IsChecked.Value)
            {
                SendPostMsg();
            }
            
            MessageTbx.Text = string.Empty;
        }

        private async void SendPostMsg()
        {
            UpdateLog(MessageTbx.Text, false);
            var values =
                new Dictionary<string, string> { { "username", "myUser" }, { "password", "myPassword" } };

            await _client.PostAsync(values);
        }

        private void UpdateLog(string message, bool isServer)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
            {

                if (isServer)
                {
                    message = "Server: " + message;
                }
                else
                {
                    message = "Client: " + message;
                }

                LogTbx.AppendText(message);
                LogTbx.AppendText(Environment.NewLine);
                LogTbx.ScrollToEnd();
            }));
        }

        private void RestApiBtn_Click(object sender, RoutedEventArgs e)
        {
            if (RestApiBtn.IsChecked.Value)
            {
                _client = new RestAPI();
            }
        }
    }
}
