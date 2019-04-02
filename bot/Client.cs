using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace bot {

    public class Client {

        static int CLIENT_INDEX = 0;
        Socket _socket;

        byte[] sendBuffer = new byte[4096];

        public Client () {

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());  
            IPAddress ipAddress = ipHostInfo.AddressList[0];  
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);  
            // IPEndPoint remoteEP = new IPEndPoint (IPAddress.Parse ("172.30.1.28"), 11000);

            // Create a TCP/IP socket.  
            _socket = new Socket (remoteEP.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
        }

        public async void Run (IPEndPoint ep) {

            try {

                try {
                    _socket.Connect (ep);
                } catch (Exception e) {
                    Console.WriteLine ("connection error " + e.ToString ());
                    return;
                }

                Console.WriteLine ("connect!!! " + System.Threading.Interlocked.Increment (ref CLIENT_INDEX));

                try {
                    //Console.WriteLine ("send!!!");
                    await TestSend ();

                } catch (System.Exception e) {

                    Console.WriteLine ("send error " + e.ToString ());
                    return;
                }

                int totalReadLen = 0;
                var receiveWrapper = new ArraySegment<byte> (new byte[4096]);
                while (true) {

                    int byteTransferred = await _socket.ReceiveAsync (receiveWrapper, SocketFlags.None);
                    if (byteTransferred == 0) {
                        return;
                    }

                    totalReadLen += byteTransferred;

                    Console.WriteLine ("receive " + byteTransferred);

                }
                // StartReceive ();

            } catch (Exception e) {
                Console.WriteLine (e.ToString ());
            }

        }

        public async Task StartReceive () {

            int totalReadLen = 0;
            var receiveWrapper = new ArraySegment<byte> (new byte[4096]);
            while (true) {

                int byteTransferred = await _socket.ReceiveAsync (receiveWrapper, SocketFlags.None);
                if (byteTransferred == 0) {
                    return;
                }

                totalReadLen += byteTransferred;

            }

        }

        public async Task TestSend () {

            try {

                var pkt = new Protocol.JsonPacket {
                    ID = 100,
                    User = "vita 500 aaaabbbbcccc",
                    Infomations = new List<int> {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6,
                    7,
                    8
                    },

                };

                for (int i = 0; i < 1; ++i) {

                    var jo = JsonConvert.SerializeObject (pkt);
                    // Console.WriteLine (jo);
                    var so = System.Text.Encoding.Default.GetBytes (jo);

                    int totalSendLen = 0;
                    int count = 1;

                    var ho = BitConverter.GetBytes ((UInt16) so.Length);
                    Buffer.BlockCopy (ho, 0, sendBuffer, 0, 2);
                    Buffer.BlockCopy (so, 0, sendBuffer, 2, so.Length);

                    int sentLen = await _socket.SendAsync (new ArraySegment<byte> (sendBuffer, 0, so.Length + 2), SocketFlags.None);
                    // Console.WriteLine ("sent len {0} count {1}", sentLen, count);
                    count++;
                    totalSendLen += sentLen;
                }

                // Console.WriteLine ("total send len " + totalSendLen);
            } catch (SocketException se) {
                Console.WriteLine ("se error " + se.ToString ());
            } catch (Exception e) {
                Console.WriteLine ("send operation error " + e.ToString ());
            }

        }

    }

}