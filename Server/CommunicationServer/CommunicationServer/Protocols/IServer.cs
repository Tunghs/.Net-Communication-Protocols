﻿using System;

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
