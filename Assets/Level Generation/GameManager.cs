using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

//	public static GameManager instance = null;
	public RoomManager roomScript;

	private int level = 10;

	// Use this for initialization
	void Awake () {
//		if (instance == null) {
//			instance = this;
//		} else if(instance != this){
//			Destroy (gameObject);
//		}
//		DontDestroyOnLoad (gameObject);

		roomScript = GetComponent<RoomManager> ();
		InitGame ();
	}

	void InitGame(){
		roomScript.SetupLevel (level);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
