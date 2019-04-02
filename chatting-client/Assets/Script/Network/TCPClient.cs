using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class TCPClient {

	Socket _socket;

	byte[] sendBuffer = new byte[4096];

	IPEndPoint remoteEP;  

	public TCPClient (IPAddress ip, int port) {
		remoteEP = new IPEndPoint(ip, port);  
		_socket = new Socket (remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
	}

	public void Connect() {
		try {
			_socket.Connect (remoteEP);
		} catch (Exception e) {
			Debug.Log("connection error " + e.ToString ());
			return;
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
			Debug.Log("receive " + byteTransferred);
		}

	}

	public async Task Send (Protocol.JsonPacket pkt) {
		try {
			for (int i = 0; i < 1; ++i) {
				var jo = JsonConvert.SerializeObject(pkt);
				Debug.Log("Json 객체");
				Debug.Log(jo);
				var so = System.Text.Encoding.Default.GetBytes (jo);
				var ho = BitConverter.GetBytes ((UInt16) so.Length);

				Buffer.BlockCopy (ho, 0, sendBuffer, 0, 2);
				Buffer.BlockCopy (so, 0, sendBuffer, 2, so.Length);

				int sentLen = await _socket.SendAsync (new ArraySegment<byte> (sendBuffer, 0, so.Length + 2), SocketFlags.None);
			}
		} catch (SocketException se) {
			Debug.Log("se error " + se.ToString ());
		} catch (Exception e) {
			Debug.Log("send operation error " + e.ToString ());
		}
	}

	public void Close() {
		
	}
}
