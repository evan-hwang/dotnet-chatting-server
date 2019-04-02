using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public enum NETWORK_MSG_TYPE {
    GET_ROOMS,
    CREATE_ROOM,
    JOIN_ROOM,
    SEND,
    RECEIVE,
    EXIT
}

public class NetworkManager: Singleton<NetworkManager> {

    TCPClient client;

    public void Connect() {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());  
        IPAddress ipAddress = ipHostInfo.AddressList[0];  

        try {
            Debug.Log("소켓 연결");
            client = new TCPClient (ipAddress, 11000);
            client.Connect();
            client.StartReceive();

        } catch (System.Exception e) {
            Debug.Log (e.ToString ());
        }

        // System.Threading.Thread.Sleep (10000);
        // Debug.Log ("Over!!");
    }

    public void Send(Protocol.JsonPacket pkt) {
        client.Send(pkt);
    }

    // public async Task Send(NETWORK_MSG_TYPE msgType) {
    //     await client.TestSend();

        // switch(msgType) {
        //     case NETWORK_MSG_TYPE.CREATE_ROOM:
        //         break;

        //     case NETWORK_MSG_TYPE.GET_ROOMS:
        //         break;

        //     case NETWORK_MSG_TYPE.JOIN_ROOM:
        //         break;

        //     case NETWORK_MSG_TYPE.SEND:
        //         break;

        //     case NETWORK_MSG_TYPE.RECEIVE:
        //         break;

        //     case NETWORK_MSG_TYPE.EXIT:
        //         break;

        //     default:
        //         Debug.Log("Send를 시도했으나 적절한 MSG_TYPE이 들어오지 않았습니다.");
        //         break;
        // }
    }
// }