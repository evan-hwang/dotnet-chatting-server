using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace client
{
    static class Constants
    {

    }

    class Connector
    {

        public Connector()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp ); 

            
        }
    }

    class Program
    {
        static async Task<int> ConnectToServerAsync() 
        {    
            using (HttpClient client = new HttpClient())  
            {  
                Console.WriteLine("Async Start.");
                Task<string> getStringTask = client.GetStringAsync("https://docs.microsoft.com");  
                Console.WriteLine("DoIndependentWork.");
                string urlContents = await getStringTask;  // 이 작업이 완료될 때까지 ConnectToServerAsync를 호출한 메서드에 제어 권한 양도.
                Console.WriteLine("Async Complete.");
                return urlContents.Length;  
            }  
        }

        static void Main(string[] args)
        {
            Console.WriteLine("ConnectToServerAsync Start.");
            ConnectToServerAsync().Wait(); // await에서 Task<int>를 반환 받음.
            Console.WriteLine("ConnectToServerAsync Completed.");
        }
    }
}