using System;

namespace CommunicationClient.Protocols
{
    public interface IClient
    {
        bool IsRunning();
        Action<string> RecieveMessage { get; set; }
        void On();
        void Off();
        void SendAsync(string data);
    }
}
