using System;

namespace CommunicationClient.Protocols
{
    public interface IClient
    {
        bool IsRunning();
        Action<string> RecieveMessage { get; set; }
        void On();
        void Off();
        void Send(string data);
    }
}
