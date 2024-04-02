using RemoteObjects;

using System;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace CommunicationServer.Protocols
{
    internal class IpcServer : IServer
    {
        static string tx = string.Empty;
        static bool isRunning = false;

        public Action<string> RecieveMessage { get; set; }

        public void Off()
        {
            isRunning = false;
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        [STAThread]
        [SecurityPermission(SecurityAction.Demand)]
        public void On()
        {
            isRunning = true;
            Task.Run(() =>
            {
                RunServer();
            });
        }

        public void Send(string data)
        {
            tx = data;
        }

        private void RunServer()
        {
            RemoteObject.CreateServer();
            RemoteObject ro = new RemoteObject();

            string id = "server";
            ro.Message = string.Empty;

            while (true)
            {
                if (!isRunning)
                {
                    break;
                }

                if (!string.IsNullOrEmpty(tx))
                {
                    if (tx.ToUpper() == "Q")
                    {
                        isRunning = true;
                        break;
                    }

                    ro.ID = id;
                    ro.Message = tx;
                    ro.IsServerMessage = true;
                    tx = string.Empty;
                }

                if (ro.IsClientMessage)
                {
                    if (ro.ID.Contains("client"))
                    {
                        string clientID = ro.ID;
                        string message = ro.Message;
                        RecieveMessage?.Invoke(message);
                    }

                    ro.IsClientMessage = false;
                }
            }
        }
    }
}
