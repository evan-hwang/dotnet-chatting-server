using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace bot {

    class Program {

        static void Main (string[] args) {

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());  
            IPAddress ipAddress = ipHostInfo.AddressList[0];  
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);  
            // IPEndPoint remoteEP = new IPEndPoint (IPAddress.Parse ("172.25.53.248"), 11000);

            try {
                List<Client> clients = new List<Client> ();
                for (int i = 0; i < 1; i++) {
                    var c = new Client ();
                    clients.Add (c);

                    c.Run (remoteEP);
                }

            } catch (System.Exception e) {

                Console.WriteLine (e.ToString ());
            }

            System.Threading.Thread.Sleep (10000);

            Console.WriteLine ("Over!!");
            Console.ReadLine ();
        }
    }
}