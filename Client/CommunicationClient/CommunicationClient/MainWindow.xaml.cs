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
    public enum CommunicationMode
    {
        TcpIp,
        REST,
        IPC,
        WCF
    }

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        List<CommunicationView> views;
        List<TabItem> tabs;

        private RestAPI _client;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            views = new List<CommunicationView>() { TcpIpComuView, RestComuView, IpcComuView, WcfComuView };
            tabs = new List<TabItem>() { TcpIPTab, RestTab, IpcTab, WcfTab };
            for (int index = 0; index < 4; index++)
            {
                views[index].Intialize((CommunicationMode)index);
                views[index].OpenServer += (() => { tabs.ForEach(x => x.IsEnabled = false); tabs[ServerTab.SelectedIndex].IsEnabled = true; ServerTab.Items.Refresh(); });
                views[index].CloseServer += (() => { tabs.ForEach(x => x.IsEnabled = true); ServerTab.Items.Refresh(); });
            }
        }

        //private void SendBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(MessageTbx.Text))
        //    {
        //        return;
        //    }

        //    if (RestApiBtn.IsChecked.Value)
        //    {
        //        SendPostMsg();
        //    }

        //    MessageTbx.Text = string.Empty;
        //}

        //private async void SendPostMsg()
        //{
        //    UpdateLog(MessageTbx.Text, false);
        //    var values =
        //        new Dictionary<string, string> { { "username", "myUser" }, { "password", "myPassword" } };

        //    await _client.PostAsync(values);
        //}

        //private void UpdateLog(string message, bool isServer)
        //{
        //    Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
        //    {

        //        if (isServer)
        //        {
        //            message = "Client: " + message;
        //        }
        //        else
        //        {
        //            message = "Client: " + message;
        //        }

        //        LogTbx.AppendText(message);
        //        LogTbx.AppendText(Environment.NewLine);
        //        LogTbx.ScrollToEnd();
        //    }));
        //}

        //private void RestApiBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (RestApiBtn.IsChecked.Value)
        //    {
        //        _client = new RestAPI();
        //    }
        //}
    }
}
