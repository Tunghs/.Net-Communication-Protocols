using RemoteObjects;

using System;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace CommunicationServer.Protocols
{
    internal class IpcServer
    {
        static string tx = string.Empty;
        static bool isStop = false;

        [STAThread]
        [SecurityPermission(SecurityAction.Demand)]
        public void Run(string[] args)
        {
            Task.Run(() =>
            {
                RunServer();
            });
        }

        private void RunServer()
        {
            RemoteObject.CreateServer();
            RemoteObject ro = new RemoteObject();

            string id = "server";
            ro.Message = string.Empty;

            while (true)
            {
                if (!string.IsNullOrEmpty(tx))
                {
                    if (tx.ToUpper() == "Q")
                    {
                        isStop = true;
                        break;
                    }

                    ro.ID = id;
                    ro.Message = tx;
                    Console.WriteLine($"서버 메세지 : {tx}");
                    ro.IsServerMessage = true;
                    tx = string.Empty;
                }

                if (ro.IsClientMessage)
                {
                    if (ro.ID.Contains("client"))
                    {
                        string clientID = ro.ID;
                        string message = ro.Message;
                        Console.WriteLine($"{clientID}의 메세지 : {message}");
                    }

                    ro.IsClientMessage = false;
                }
            }
        }
    }
}
