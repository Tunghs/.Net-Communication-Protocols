using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.Protocols
{
    public interface IServer
    {
        bool IsRunning();
        Action<string> RecieveMessage { get; set; }
        void On();
        void Off();
        void Send(string data);
    }
}
