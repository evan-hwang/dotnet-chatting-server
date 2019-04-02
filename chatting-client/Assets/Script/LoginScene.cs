using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour {

	public Text userName;

	// Use this for initialization
	void Start () {
		
	}
	
	public void OnClickedStartBtn() {
		GameManager.Instance.SetUserName(userName.text);

		var payload = new LoginPayload {
			username = userName.text
		}; 
		
		Debug.Log(JsonConvert.SerializeObject(payload));
		var pkt = new Protocol.JsonPacket {
			Code = "login",
			Payload = payload
		};

		NetworkManager.Instance.Connect();
		NetworkManager.Instance.Send(pkt);
		SceneManager.LoadScene("RoomScene");
	}
}
