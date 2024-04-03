using RemoteObjects;

using System;
using System.Threading.Tasks;

namespace CommunicationClient.Protocols
{
    internal class IpcClient : IClient
    {
        static bool isRunning = false;
        private RemoteObject removeObject;

        public Action<string> RecieveMessage { get; set; }

        public bool IsRunning()
        {
            return isRunning;
        }

        public void Off()
        {
            removeObject = null;
            isRunning = false;
        }

        public void On()
        {
            RemoteObject.CreateClient();
            removeObject = new RemoteObject();
            removeObject.ID = "client";
            isRunning = true;
            Task.Run(() =>
            {
                RunServerWatching();
            });
        }

        private void RunServerWatching()
        {
            while (true)
            {
                if (!isRunning)
                    break;

                if (removeObject.IsServerMessage)
                {
                    removeObject.IsServerMessage = false;
                    RecieveMessage?.Invoke(removeObject.Message);
                }
            }
        }

        public void Send(string data)
        {
            removeObject.IsClientMessage = true;
            removeObject.Message = data;
        }
    }
}
