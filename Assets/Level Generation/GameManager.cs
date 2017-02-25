using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public RoomManager roomScript;

	// Use this for initialization
	void Awake () {
		roomScript = GetComponent<RoomManager> ();
		InitGame ();
	}

	void InitGame(){
		roomScript.setupRooms ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
