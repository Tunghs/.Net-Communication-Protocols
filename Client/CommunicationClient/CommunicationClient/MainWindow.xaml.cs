using System;
using System.Collections.Generic;
using System.Linq;
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

            UpdateLog(MessageTbx.Text, true);
            MessageTbx.Text = string.Empty;
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

            }
        }
    }
}
