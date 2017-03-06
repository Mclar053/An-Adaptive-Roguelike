using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public RoomManager roomScript;

	private int level = 3;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if(instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);

		roomScript = GetComponent<RoomManager> ();
		InitGame ();
	}

	void InitGame(){
		roomScript.SetupLevel (level);
	}
	
	// Update is called once per frame
	void Update () {
		roomScript.checkRoomComplete ();
	}

	void OnGUI(){
		GUI.Label (new Rect(10,0,100,25), "Health: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().currentHitpoints+"/"+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().maxHitpoints);
		GUI.Label (new Rect(10,25,100,25), "Speed: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().speed);
		GUI.Label (new Rect(10,50,100,25), "Range: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().range);
		GUI.Label (new Rect(10,75,100,25), "Fire Rate: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().fireDelay);
		GUI.Label (new Rect(10,100,100,25), "Shot Speed: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().shotSpeed);
		GUI.Label (new Rect(10,125,100,25), "Damage: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().dmg);
	}
}
