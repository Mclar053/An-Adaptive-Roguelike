using UnityEngine;
using System.Collections;

/* Reference for basic outline of Game Manager
 * Link: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager?playlist=17150
 * Author: Matt Schell
 */ 

//Music "skeleton.mp3" created by Gustaf Svenungsson

public class GameManager : MonoBehaviour {

	public static GameManager instance = null; //Static instance of Gamemanager
	public RoomManager roomScript; //Roommanager. Contains all room controls.
	public ScoreManager scoreManager; //Scoremanager. Contains all score controls.
	public LoadXmlData roomData; //File Manager. Deals with all saving and loading of data. Deals with handling player statistics.
	private GUIStyle guiStyle = new GUIStyle(); //To display text onto the screen
	public int numberOfRooms, numberOfBossRooms, numberOfSpecialRooms; //Number of different rooms as the different types are separated in the File Manager.

	//Error handling messages to the player
	private string playerSelectText;
	private string playerSelectErrorMessage;

	private int level = 1; //The level the player is on.
	private GameStates currentState; //The current state of the program. Starts at MainMenu

	bool debug = false; //Whether the debug screen is running or not

	// Use this for initialization
	void Awake () {
		/* Created to: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager?playlist=17150
		 * If an instance of the game manager has not been created
		 * created one and if the instance is not the current one then
		 * destroy that gameobject.
		 */
		if (instance == null) {
			instance = this;
		} else if(instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);

		//Gets the components attached to the gameobject to initialise all the managers
		roomScript = GetComponent<RoomManager> ();
		roomData = GetComponent<LoadXmlData> ();
		scoreManager = GetComponent<ScoreManager> ();

		//Loads in room data from XML
		roomData.loadRooms ();
		//Loads all player data from XML
		roomData.loadPlayerProfiles ();

		//Sets constant of number of rooms in the game
		numberOfRooms = roomData.getNumberOfRooms ();
		numberOfBossRooms = roomData.getNumberOfBossRooms ();
		numberOfSpecialRooms = roomData.getNumberOfSpecialRooms ();

		//Set the starting state of the game to the Main Menu
		GameManager.instance.changeState (GameStates.MainMenu);

		//Set the gui font and colour
		guiStyle.fontSize = 30;
		guiStyle.normal.textColor = Color.white;
	}

	//@Method: Creates a dungeon for the game using the level number as a parameter
	void InitGame(){
		roomScript.SetupLevel (level);
	}

	//@Method: Advances the game to the next level
	public void nextLevel(){
		//Increment the level number
		level++;
		//Store the current floor into the main player data and save it into the player-profiles.xml file
		roomData.CurrentPlayer().storeFloorData ();
		roomData.savePlayerProfiles ();
		//Create another dungeon
		InitGame ();
	}
	
	// Update is called once per frame
	//@Method: Checks if the room is complete and [FOR DEBUGGING ONLY] if the 'r' key is pressed to reset the level
	void Update () {
		//Checks keystrokes
		if (GameManager.instance.CurrentState () == GameStates.MainGame) { //-----Main Game (GAMEPLAY)
			//Checks if the room is complete and the player has not completed this specific room before
			if (roomScript.checkRoomComplete () && !roomData.CurrentPlayer().roomCompleted (roomScript.currentRoom)) {
				//Sets the endtime for the player in their statistics
				roomData.CurrentPlayer().endRoomTime (roomScript.currentRoom); 
				//If the current room in not the spawn room
				if (roomScript.currentRoom != 0) {
					//Create a pickup in the middle of the room
					roomScript.createPickup ();
				}
			}

			//DEBUG CONTROLS//

			//If R is pressed, created another dungeon in the same level
			if (Input.GetKeyDown (KeyCode.R)) {
				roomScript.SetupLevel (level);
			}
			//If H is pressed print all the save stats of the player
			if (Input.GetKeyDown (KeyCode.H)) {
				roomData.CurrentPlayer().printStats ();
			}
			//If J is pressed, print all of the stats current stored for the current floor
			if (Input.GetKeyDown (KeyCode.J)) {
				roomData.CurrentPlayer().printCurrentFloorStats ();
			}
			//If M is pressed, print the number of players that have been stored in statistics
			if (Input.GetKeyDown (KeyCode.M)) {
				roomData.printPlayers ();
			}
			//If ] is pressed, enable/disable GUI debug interface
			if (Input.GetKeyDown (KeyCode.RightBracket)) {
				debug = !debug;
			}
		} else if (GameManager.instance.CurrentState () == GameStates.GameOver) {//-----Game Over
			//Play the game again with the same player if SPACE is pressed
			if (Input.GetKeyDown (KeyCode.Space)) {
				GameManager.instance.changeState (GameStates.MainGame);
			}
			//Leave to the Main Menu if ESCAPE is pressed
			if (Input.GetKeyDown (KeyCode.Escape)) {
				GameManager.instance.changeState (GameStates.MainMenu);
			}
		} else if (GameManager.instance.CurrentState () == GameStates.PlayerSelect) {//-----Player Select
			//Captured input from the keyboard
			string keyPressed = Input.inputString; 
			int number; //Number if successfully parsed

			//Attempt to convert the input into an integer
			//If successful, add the number to the 'playerSelectText'
			if(int.TryParse(keyPressed,out number)){
				playerSelectText += number;
			}

			//If delete or backspace pressed, remove the last character from the 'playerSelectText' variable
			if ((Input.GetKeyDown (KeyCode.Backspace) || Input.GetKeyDown (KeyCode.Delete)) && playerSelectText.Length > 0) {
				playerSelectText = playerSelectText.Substring (0, playerSelectText.Length - 1);
			}

			//If SPACE is pressed, attempt to confirm the players choice
			if (Input.GetKeyDown (KeyCode.Space)) {

				//Check if the player has entered any input in
				if (playerSelectText == "") {
					//Player has not entered anything in
					playerSelectErrorMessage = "Please Enter A Player ID";
				} else{
					//Attempt to load the player with the id they have provided
					bool loadedPlayer = roomData.loadPlayer (int.Parse (playerSelectText));
					if (loadedPlayer) {
						//If the playerID exists, load the main game
						GameManager.instance.changeState (GameStates.MainGame);
						playerSelectErrorMessage = "";
					} else {
						//Else, notify the player with message
						playerSelectErrorMessage = "ID Entered Does Not Exist";
					}
				} 
			}
			//Return the to Main Menu
			if (Input.GetKeyDown (KeyCode.Escape)) {
				GameManager.instance.changeState (GameStates.MainMenu);
			}

		} else if (GameManager.instance.CurrentState () == GameStates.MainMenu) {//-----Main Menu
			//Item 1 is to start game as a new player
			if (Input.GetKeyDown ("1")) {
				//Add a new player to the statistics
				roomData.addPlayer ();
				//Start playing the main game
				GameManager.instance.changeState (GameStates.MainGame);
			} else if (Input.GetKeyDown ("2")) {
				//Select a player via player ID
				//Change state the PlayerSelect
				GameManager.instance.changeState (GameStates.PlayerSelect);
			}
		}
	}

	//Retrieves the current state from the Game Manager
	public GameStates CurrentState(){
		return currentState;
	}

	//Change state and initiate starting actions from that state
	public void changeState(GameStates _state) {

		//If the currentstate is different from the new state
		if(currentState != _state) {
			//Change the state to the new state
			currentState = _state;

			switch (_state) {
				case GameStates.MainMenu:
					break;
				case GameStates.PlayerSelect:
				//Initialise the error message and player select text variables to blank
					playerSelectText = "";
					playerSelectErrorMessage = "";
					break;
				case GameStates.MainGame:
				//When starting the game, set level to 1, reset the player score and create the player GameObject
					GameManager.instance.level = 1;
					scoreManager.resetScore ();
					roomScript.createPlayer ();
				//Create the dungeon
					InitGame ();
					break;
				case GameStates.GameOver:
				//Delete all the room gameobjects in the level
					roomScript.destroyLevel ();
				//Delete the player game object
					roomScript.destroyPlayer ();
				//Remove the room where the player had died from their statistics
					roomData.CurrentPlayer().removeCurrentRoomStat (roomScript.currentRoom);
				//Store all the current floor rooms into the player statistics dictionary
					roomData.CurrentPlayer().storeFloorData ();
				//Save the player statistics in the XML file
					roomData.savePlayerProfiles ();
					break;
			}
		}
	}

	//Change room to a proposed x and y value
	public void changeRoom(float _newGridX, float _newGridY){
		//Change room via the room manager
		roomScript.changeRoom (_newGridX, _newGridY);
		//Check if the player has played this room before
		if (!roomData.CurrentPlayer().roomCompleted (roomScript.currentRoom)) {
			//If they haven't, create a new room stat instance to record player data
			roomData.CurrentPlayer().createCurrentFloorRoom (roomScript.currentRoom, roomScript.getLevelNodeRoomID (roomScript.currentRoom));
			//Record the time when the player entered the room
			roomData.CurrentPlayer().startRoomTime (roomScript.currentRoom);
		}
	}

	//@Method: [FOR DEBUGGING ONLY] Draws player stats to the screen
	void OnGUI(){
		if (GameManager.instance.CurrentState () == GameStates.MainMenu) {
			GUI.Label (new Rect (10, 0, 300, 25), "An Adaptive Roguelike", guiStyle);
			GUI.Label (new Rect (10, 40, 100, 25), "1 - New Player", guiStyle);
			GUI.Label (new Rect (10, 65, 100, 25), "2 - Select Player", guiStyle);
		} else if (GameManager.instance.CurrentState () == GameStates.PlayerSelect) {
			GUI.Label (new Rect (10, 0, 100, 25), "Select Player", guiStyle);
			GUI.Label (new Rect (10, 40, 200, 25), "="+playerSelectText+"=", guiStyle);
			GUI.Label (new Rect (10, 65, 200, 25), playerSelectErrorMessage, guiStyle);
			GUI.Label (new Rect (10, 90, 200, 25), "Press Space To Continue", guiStyle);
			GUI.Label (new Rect (10, 115, 300, 25), "Press Escape To Return To The Main Menu", guiStyle);
		}
		else if (GameManager.instance.CurrentState () == GameStates.MainGame) {
			GUI.Label (new Rect (10, 25, 100, 25), "Health: " + GameObject.FindGameObjectWithTag ("Player").GetComponent<movingObject> ().currentHitpoints + "/" + GameObject.FindGameObjectWithTag ("Player").GetComponent<movingObject> ().maxHitpoints, guiStyle);
			GUI.Label (new Rect (10, 0, 100, 25), "Floor #: " + level, guiStyle);
			GUI.Label (new Rect (10, 50, 100, 25), "Score: " + GameManager.instance.scoreManager.getScore (), guiStyle);

			//DEBUG
			if (debug) {
				GUI.Label (new Rect (10, 75, 100, 25), "Speed: " + GameObject.FindGameObjectWithTag ("Player").GetComponent<movingObject> ().speed, guiStyle);
				GUI.Label (new Rect (10, 100, 100, 25), "Range: " + GameObject.FindGameObjectWithTag ("Player").GetComponent<movingObject> ().range, guiStyle);
				GUI.Label (new Rect (10, 125, 100, 25), "Fire Rate: " + GameObject.FindGameObjectWithTag ("Player").GetComponent<movingObject> ().fireDelay, guiStyle);
				GUI.Label (new Rect (10, 150, 100, 25), "Shot Speed: " + GameObject.FindGameObjectWithTag ("Player").GetComponent<movingObject> ().shotSpeed, guiStyle);
				GUI.Label (new Rect (10, 175, 100, 25), "Damage: " + GameObject.FindGameObjectWithTag ("Player").GetComponent<movingObject> ().dmg, guiStyle);
				GUI.Label (new Rect (10, 225, 100, 25), "Player ID: " + GameManager.instance.roomData.CurrentPlayer().userID, guiStyle);
				GUI.Label (new Rect (10, 250, 100, 25), "CurrentMod: " + GameManager.instance.roomData.CurrentPlayer().getRoomModifer(roomScript.getLevelNodeRoomID(roomScript.currentRoom)), guiStyle);
				GUI.Label (new Rect (10, 275, 100, 25), "Room ID: " + roomScript.getLevelNodeRoomID(roomScript.currentRoom), guiStyle);
			}
		} else if (GameManager.instance.CurrentState () == GameStates.GameOver) {
			GUI.Label (new Rect (10, 0, 100, 25), "Game Over", guiStyle);
			GUI.Label (new Rect (10, 25, 100, 25), "Score: " + GameManager.instance.scoreManager.getScore (), guiStyle);
			GUI.Label (new Rect (10, 75, 100, 25), "Press Space To Play", guiStyle);
			GUI.Label (new Rect (10, 100, 300, 25), "Press Escape To Return To The Main Menu", guiStyle);
		}
	}
}


/* Use of GameStates in a GameManager
 * User: Tseng
 * Reference: https://forum.unity3d.com/threads/help-how-do-you-set-up-a-gamemanager.131170/
 * Accessed: 22/04/2017
 * 
 * @enum: Contains GameStates of the game to allow the game to have end states
 */
public enum GameStates {
	MainMenu,
	MainGame,
	GameOver,
	PlayerSelect
}

//Change from Box to Edge Colliders
//Reference: https://forum.unity3d.com/threads/rigidbody-getting-stuck-on-tiled-wall.220861/