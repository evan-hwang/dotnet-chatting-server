using UnityEngine;

public class GameManager : Singleton<GameManager> {
	private string username;
	private string roomname;

	public string GetUserName() {
		return this.username;
	}

	public void SetUserName(string username) {
		Debug.Log("username 입력 완료. :" + username);
		this.username = username;
	}

	public string GetRoomName() {
		return this.roomname;
	}
	
	public void SetRoomName(string roomname) {
		this.roomname = roomname;
	}
}