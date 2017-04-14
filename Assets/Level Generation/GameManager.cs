using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public RoomManager roomScript;
	public PlayerStats statistics;
	public LoadXmlData roomData;
	private GUIStyle guiStyle = new GUIStyle();
	public int numberOfRooms, numberOfBossRooms, numberOfSpecialRooms;

	private int level = 1;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if(instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);

		roomScript = GetComponent<RoomManager> ();
		roomData = GetComponent<LoadXmlData> ();
		statistics = GetComponent<PlayerStats> ();

		//Loads in room data from XML
		roomData.loadRooms ();
		roomData.loadPlayerProfiles ();
		statistics.loadPlayer (roomData.loadPlayer (0));

		//Sets constant of number of rooms in the game
		numberOfRooms = roomData.getNumberOfRooms ();
		numberOfBossRooms = roomData.getNumberOfBossRooms ();
		numberOfSpecialRooms = roomData.getNumberOfSpecialRooms ();

		//Sets up a level
		InitGame ();
		guiStyle.fontSize = 20;
		guiStyle.normal.textColor = Color.white;
	}

	//@Method: Creates a dungeon for the game using the level number as a parameter
	void InitGame(){
		roomScript.SetupLevel (level);
	}

	//@Method: Advances the game to the next level
	public void nextLevel(){
		level++;
		statistics.storeFloorData ();
		roomData.savePlayer (statistics,statistics.userID);
		roomData.savePlayerProfiles ();
		InitGame ();
	}
	
	// Update is called once per frame
	//@Method: Checks if the room is complete and [FOR DEBUGGING ONLY] if the 'r' key is pressed to reset the level
	void Update () {
		if (roomScript.checkRoomComplete () && !statistics.roomCompleted(roomScript.currentRoom)) {
			statistics.endRoomTime (roomScript.currentRoom);
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			roomScript.SetupLevel (level);
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			statistics.printStats ();
		}
		if (Input.GetKeyDown (KeyCode.J)) {
			statistics.printCurrentFloorStats ();
		}
		if (Input.GetKeyDown (KeyCode.M)) {
			roomData.printPlayers ();
		}
	}

	public void changeRoom(float _newGridX, float _newGridY){
		roomScript.changeRoom (_newGridX,_newGridY);
		if(!statistics.roomCompleted(roomScript.currentRoom)){
			statistics.createCurrentFloorRoom (roomScript.currentRoom, roomScript.getLevelNodeRoomID(roomScript.currentRoom));
			statistics.startRoomTime (roomScript.currentRoom);
		}
	}

	//@Method: [FOR DEBUGGING ONLY] Draws player stats to the screen
	void OnGUI(){
		GUI.Label (new Rect(10,0,100,25), "Health: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().currentHitpoints+"/"+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().maxHitpoints,guiStyle);
		GUI.Label (new Rect(10,25,100,25), "Speed: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().speed,guiStyle);
		GUI.Label (new Rect(10,50,100,25), "Range: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().range,guiStyle);
		GUI.Label (new Rect(10,75,100,25), "Fire Rate: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().fireDelay,guiStyle);
		GUI.Label (new Rect(10,100,100,25), "Shot Speed: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().shotSpeed,guiStyle);
		GUI.Label (new Rect(10,125,100,25), "Damage: "+GameObject.FindGameObjectWithTag("Player").GetComponent<movingObject>().dmg,guiStyle);
		GUI.Label (new Rect(10,150,100,25), "Floor #: "+level,guiStyle);
	}
}
