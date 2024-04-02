using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace CommunicationServer.Protocols
{
    internal class WcfServer : IServer
    {
        ServiceHost host;
        public Action<string> RecieveMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsRunning()
        {
            throw new NotImplementedException();
        }

        public void Off()
        {
            throw new NotImplementedException();
        }

        public void On()
        {
            string address = "net.tcp://localhost:8080/myAddress";
            NetTcpBinding binding = new NetTcpBinding();
            //// Service Host 만들기
            //host = new ServiceHost(typeof(MyService));

            //// End Point 추가
            //host.AddServiceEndpoint(typeof(IMyContract), binding, address);

            //// Service Host 시작
            //host.Open();
        }

        public void Send(string data)
        {
            throw new NotImplementedException();
        }
    }
}
