using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    static class Constants
    {
        public const int HEADERSIZE = 2;
        public const int BUFFERSIZE = 4096;
    }

    class Acceptor
    {
        Thread workerThread;
        Socket listenerSocket;

        public Action<Socket> AccceptEvent;

        private void onWork()
        {
            while(true) 
            {
                var clientSocket = this.listenerSocket.Accept();
                AccceptEvent(clientSocket); 
            }
        }

        public Acceptor() 
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 11000);
            listenerSocket = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenerSocket.Bind(ep);
            listenerSocket.Listen(100);
        }

        public void Start() 
        {
            Thread workerThread = new Thread(this.onWork);
            workerThread.IsBackground = true;
            workerThread.Start();
        }
    }

    class Connector
    {
        public void Connect() 
        {
            
        }

        public void onAcceptComplete(Socket clientSocket)
        {
            this.Start(clientSocket);
        }

        public async Task<Socket> Start(Socket clientSocket) 
        {
            var data = "";
            var position = 0;
            var receiveLen = 0;
            var packetSize = 0;
            var totalLen = 0;
            var receiveBuffer = new byte[Constants.BUFFERSIZE];
            var arraySegment = new ArraySegment<byte>(receiveBuffer);

            while(true)
            {
                // 1. 데이터 받아오기
                receiveLen = await clientSocket.ReceiveAsync(arraySegment, SocketFlags.None);
                totalLen += receiveLen;
                Console.WriteLine("outer totalLen " + totalLen);

                while(totalLen > 0) 
                {
                    Console.WriteLine("inner Start");
                    // 2. receiveBuffer Pointer? 이동
                    arraySegment = new ArraySegment<byte>(receiveBuffer, position, receiveLen - position);
                    Console.WriteLine("inner arraySegment");
                    this.PrintArray(arraySegment.ToArray());

                    // 3. 헤더가 들어왔는가? 안 들어왔으면 다음 Receive wait
                    if (arraySegment.Count < Constants.HEADERSIZE) { break; };

                    // 4. 패킷 사이즈 결정
                    packetSize = BitConverter.ToInt16(receiveBuffer, position);

                    // 5. 바디가 다 들어왔는가? 안 들어왔으면 다음 Receive wait
                    if (arraySegment.Count < packetSize) { break; };

                    // 6. 패킷 분석
                    var bodyBuffer = new byte[packetSize - Constants.HEADERSIZE];
                    Array.Copy(receiveBuffer, position + Constants.HEADERSIZE, bodyBuffer, 0, packetSize - Constants.HEADERSIZE);
                    data = Encoding.UTF8.GetString(bodyBuffer);    
                    Console.WriteLine("received data: " + data);

                    position += packetSize;
                    totalLen -= packetSize;
                    Console.WriteLine("inner position " + position);
                    Console.WriteLine("inner packetSize " + packetSize);
                    Console.WriteLine("inner totalLen " + totalLen);
                }

                // 7. 종료
                break;
            }

            return null;
        }

        void PrintArray(Array array)
        {
            foreach(var item in array)
            {
                Console.Write(item.ToString() + ", ");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {   
            ManualResetEvent waitEvent = new ManualResetEvent(false);
            Acceptor acceptor = new Acceptor();
            Connector connector = new Connector();
            acceptor.AccceptEvent += connector.onAcceptComplete;
            acceptor.Start();
            waitEvent.WaitOne();

            // int[] test = new int[10]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            // Array.Copy(test, 5, test, 0, 5);
            // Console.WriteLine(test);
        }
    }
}
