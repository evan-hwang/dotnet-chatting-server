using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class RoomScene : MonoBehaviour {
	public Text roomName;

	private List<IRoom> _rooms;

	void Start() {
		this.CreateRoomAsync();
	}

	public void CreateRoomAsync() {
		Debug.Log(roomName.text + "라는 이름으로 룸 생성 시도");
		var payload = new CreateRoomPayload();
		payload.roomName = roomName.text;
		var pkt = new Protocol.JsonPacket {
			Code = "create_room",
			Payload = payload
		};
		NetworkManager.Instance.Send(pkt);
	}

	public void GetRoomsAsync() {
		Debug.Log("룸 가져오기");
		var payload = new GetRoomsPayload();
		var pkt = new Protocol.JsonPacket {
			Code = "get_rooms",
			Payload = payload
		};
		NetworkManager.Instance.Send(pkt);
	}
}
