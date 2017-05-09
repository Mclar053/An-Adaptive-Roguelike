using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Xml;

public class RoomManager : MonoBehaviour {

	//Columns and rows of all rooms
	public int columns = 15;
	public int rows = 9;

	//All gameobjects that need to be instantiated
	public GameObject doorLeftTile;
	public GameObject doorRightTile;
	public GameObject doorTopTile;
	public GameObject doorBottomTile;
	public GameObject floorTile;
	public GameObject bossFloorTile;
	public GameObject wallTile;
	public GameObject gapTile;
	public GameObject[] enemy;
	public GameObject[] bullet;
	public GameObject player;
	public GameObject healthBar;

	//Pickup prefabs
	public GameObject score;
	public GameObject health;
	public GameObject[] upgrades; //UNUSED

	//Current Level
	private mTree levelTree;

	//List of room objects
	private Transform[] roomHolder = new Transform[0]; //Holds all gameobejcts for all rooms
	private Transform playerTransform; //Holds the players game object
	private Transform healthBarTransform; 
	public int currentRoom; //Current room selected
	public Vector2 currentGridPosition = new Vector2(0,0); //The current position of the grid map the player is in. Used for changes between different rooms.
	private int currentLevel; //The curent level the player is on

	//Destroys old dungeons and creates new dungeons with all gameobjects
	void createLevel(int _level){
		//Get the current level the player is on
		currentLevel = _level;
		//Delete all game objects of the old dungeon
		destroyLevel ();

		//Create a new dungeon layout tree
		levelTree = new mTree (_level);
		//Ensure the dungeon tree has at least 4 end rooms
		while (levelTree.getEndRoomCount () < 4) {
			levelTree = new mTree (_level);
		}
		//Create a boss room at one the end rooms
		levelTree.makeBossRoom ();

		//Retrieve all the nodes from the dungeon layout tree
		List<Node> levelNodes = levelTree.getNodes ();

		//Set the length of the roomHolder to the number of nodes in the tree
		roomHolder = new Transform[levelNodes.Count];
		currentRoom = 0; //The current room is the spawn room
		currentGridPosition = new Vector2(0,0); //The current grid position is at the spawn room


		//For each node in the dungeon tree
		for (int i = 0; i < levelNodes.Count; i++) {
			Node currentNode = levelNodes[i];
			//Check position of doors for room
			bool[] doors = new bool[4] {false,false,false,false}; //[0] Up [1] Down [2] Left [3] Right

			//Check for child node doors
			if(currentNode.getChildren() != null){
				foreach(int _childIndex in currentNode.getChildren()){
					if(checkDoor(0, 1, currentNode, levelNodes[_childIndex])){
						doors[0] = true;
					}
					else if(checkDoor(0, -1, currentNode, levelNodes[_childIndex])){
						doors[1] = true;
					}
					else if(checkDoor(-1, 0, currentNode, levelNodes[_childIndex])){
						doors[2] = true;
					}
					else if(checkDoor(1, 0, currentNode, levelNodes[_childIndex])){
						doors[3] = true;
					}
				}
			}

			//Check for parent node door
			if(currentNode.getParent() != -1){
				if(checkDoor(0, 1, currentNode, levelNodes[currentNode.getParent()])){
					doors[0] = true;
				}
				else if(checkDoor(0, -1, currentNode, levelNodes[currentNode.getParent()])){
					doors[1] = true;
				}
				else if(checkDoor(-1, 0, currentNode, levelNodes[currentNode.getParent()])){
					doors[2] = true;
				}
				else if(checkDoor(1, 0, currentNode, levelNodes[currentNode.getParent()])){
					doors[3] = true;
				}
			}

			int[,] roomLayout, roomEntities;

			roomLayout = new int[0, 0];
			roomEntities = new int[0, 0];

			//Debug.Log (i+" TYPE: "+ currentNode.getRoomType()+" ID: "+currentNode.getRoomID());

			//Checks the room type of the node to select the appropriate room layout with entity placements
			if (currentNode.getRoomType () == 1) { //Boss Rooms
				roomLayout = GameManager.instance.roomData.getBossRoomLayout (currentNode.getRoomID ()-GameManager.instance.numberOfRooms);
				roomEntities = GameManager.instance.roomData.getBossRoomEntities (currentNode.getRoomID ()-GameManager.instance.numberOfRooms);
			} else if (currentNode.getRoomType () == 2) { //Special Rooms
				roomLayout = GameManager.instance.roomData.getSpecialRoomLayout (currentNode.getRoomID ());
				roomEntities = GameManager.instance.roomData.getSpecialRoomEntities (currentNode.getRoomID ());
			} else { //Normal Rooms
				roomLayout = GameManager.instance.roomData.getRoomLayout (currentNode.getRoomID ());
				roomEntities = GameManager.instance.roomData.getRoomEntities (currentNode.getRoomID ());
			}


			//Calculates a score for the player based on their average performance of the room ID selected
			float performanceScore = GameManager.instance.roomData.CurrentPlayer().getRoomAvergePerformance (currentNode.getRoomID ());
			//Gets the current modifier of the selcted room ID
			int currentMod = GameManager.instance.roomData.CurrentPlayer().getRoomModifer (currentNode.getRoomID ());

			//Code to modify the rooms based on player performance
			//------
			//If the room is not the spawn room
			if (currentNode.getRoomID () != 0) {
				//Check if the performance score is greater than 0 - Error detection from performance score calculation (see ln 129)
				if (performanceScore >= 0) {
					//Add to the current modifier based on the players performance with the current modifier
					currentMod += GameManager.instance.roomData.checkRoomModifier (currentNode.getRoomID (), performanceScore, currentMod);
					//Set the current room ID's modifier to the changed modifier
					GameManager.instance.roomData.CurrentPlayer().setRoomModifier (currentNode.getRoomID (), currentMod);
				}
			}
			//-----

			//Create a new transform object to hold all the gameobjects of that room
			roomHolder[i] = new GameObject ("Room"+i).transform;
			//Create all gameobjects in the room
			createRoom (i, doors, roomLayout);
			//If the room is not the spawn room -- Spawn room is safe
			if (i != 0) {
				//Add enemy gameobjects to the current room with the current modifier to the room
				addEnemies (i, roomEntities, currentMod); 
				//Deactivate the room in the unity hierarchy -- Spawn room is active at the start of the dungeon
				roomHolder [i].gameObject.SetActive (false);
			}
		}
	}

	//Destorys all game objects in the dungeon level
	public void destroyLevel(){
		foreach(Transform _t in roomHolder){
			if (_t != null) {
				Destroy (_t.gameObject);
			}
		}
	}

	//Checks two node grid positions to see if a door placement is needed at _xMod,_yMod 
	private bool checkDoor(int _xMod, int _yMod, Node _currentNode, Node _otherNode){
		if(_currentNode.getGridPosition().x + _xMod == _otherNode.getGridPosition().x &&
			_currentNode.getGridPosition().y + _yMod == _otherNode.getGridPosition().y){
			return true;
		}
		return false;
	}

	/* Reference:
	 * Author:
	 * 
	 *@method: Creates all tile gameobjects for a room  
	 */
	void createRoom(int _room, bool[] _doors, int[,] levelLayout){
		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for(int x = 0; x < columns; x++){
			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for(int y = 0; y < rows; y++){
				//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
				GameObject toInstantiate;

				switch (levelLayout [x, y]) {
				case 1:
					toInstantiate = gapTile;
					break;
				case 2: case 3:
					toInstantiate = wallTile;
					break;
				case 4:
					toInstantiate = bossFloorTile;
					break;
				default:
					toInstantiate = floorTile;
					break;
				}
					
				//Checks if the current position requires a door tile
				if (_doors[0] && x == Mathf.Floor (columns / 2f) && y == rows-1) {
					toInstantiate = doorTopTile;
				}
				else if (_doors[1] && x == Mathf.Floor (columns / 2f) && y == 0) {
					toInstantiate = doorBottomTile;
				}
				else if(_doors[2] && y == Mathf.Floor (rows / 2f) && x == 0) {
					toInstantiate = doorLeftTile;
				}
				else if(_doors[3] && y == Mathf.Floor (rows / 2f) && x == columns-1) {
					toInstantiate = doorRightTile;
				}

				//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				instance.transform.SetParent (roomHolder[_room]);
			}
		}
	}

	//Creates a player gameobject
	public void createPlayer(){
		playerTransform = new GameObject ("PlayerTransform").transform;
		GameObject instance = Instantiate (player, new Vector3 (7, 4, 0), Quaternion.identity) as GameObject;
		instance.transform.SetParent (playerTransform);
	}

	//Deletes the player gameobject
	public void destroyPlayer(){
		Destroy (playerTransform.gameObject);
	}

	//Adds enemy gameobjects
	void addEnemies(int _room, int[,] _entities, int _mod){
		//For all entities in the _entities array
		for(int i=0; i<_entities.GetLength(0); i++){
			//Instantiate the correct entity at the predetermined location
			GameObject instance = Instantiate (enemy[_entities[i,0]], new Vector3 (_entities[i,1],_entities[i,2], 0f), Quaternion.identity) as GameObject;

			//Checks if the enemy has a moving object component
			//Gives the enemy a modifier from the room modifier
			//This adjusts the difficulty of the enemy
			if (instance.GetComponent<movingObject> () != null) {
				instance.GetComponent<movingObject> ().modifier = _mod;
			}
			//Add the enemy gameobject to the correct room gameobject transform on the hierarchy
			instance.transform.SetParent (roomHolder [_room]);
		}
	}

	//Creates a bullet in the room
	public void createBullet(int _bulletType, Vector2 _pos, Vector2 _dir, float _speed, float _dmg, float _range){
		GameObject instance = Instantiate (bullet[_bulletType], _pos, Quaternion.identity) as GameObject;
		instance.gameObject.GetComponent<Bullet> ().setBulletStats(_speed,_dmg,_range);
		instance.gameObject.GetComponent<Bullet> ().changeDirection (_dir);
		instance.transform.SetParent (roomHolder [currentRoom]);
	}

	//Changes the current room the player is in
	public void changeRoom(float _newGridX, float _newGridY){
		roomHolder [currentRoom].gameObject.SetActive (false);
		int oldCurrentRoom = currentRoom;
		currentRoom = levelTree.getNodePosFromGridCoords (_newGridX, _newGridY);

		if (currentRoom != -1) { //Checks if there is a node with grid position that was passed through
			currentGridPosition = new Vector2(_newGridX,_newGridY);
		} else {
			currentRoom = oldCurrentRoom;
		}

		//Sets the current room to be activate in the hierarchy
		roomHolder [currentRoom].gameObject.SetActive (true);
	}

	//Checks if the room is complete
	public bool checkRoomComplete(){
		//Checks if there are any enemies in the room
		if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0){
			//If none then open all doors and the NextFloor portal
			openDoor (GameObject.FindGameObjectWithTag ("DoorBottom"));
			openDoor (GameObject.FindGameObjectWithTag ("DoorLeft"));
			openDoor (GameObject.FindGameObjectWithTag ("DoorRight"));
			openDoor (GameObject.FindGameObjectWithTag ("DoorTop"));
			openDoor (GameObject.FindGameObjectWithTag ("NextFloor"));
			return true;
		}
		return false;
	}

	/*@Input
	 * int _nodeIndex - the index of the node in the dungeon tree
	 * -----
	 * @Output
	 * int - room id of the node selected
	 * -----
	 * @Method: Return room id of a particular node in the level tree
	*/
	public int getLevelNodeRoomID(int _nodeIndex){
		return levelTree.getNode (_nodeIndex).getRoomID ();
	}

	//Changes the collider to a trigger and the colour tint of the gameobject to white
	void openDoor(GameObject _door){
		if(_door != null){
			_door.GetComponent<Collider2D> ().isTrigger = true;
			_door.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f);
		}
	}

	//Creates a pickup in the room and sets it's value
	public void createPickup(){
		GameObject toInstantiate;
		float chance = Random.Range (0, 100);

		//Create a health pickup that heals between 1-3 hp to the player
		toInstantiate = health;
		toInstantiate.GetComponent<Pickup> ().setValue (Random.Range (1,3));

		//70% chance to change the pickup to a score pickup
		if (chance > 30f) {
			//Score can be between the currentLevel the player is on and how far into the dungeon the player is
			//Meaning that the more floor completed, there is a guarentee for a  higher  score will be
			toInstantiate = score;
			toInstantiate.GetComponent<Pickup> ().setValue (Random.Range (currentLevel, currentLevel + currentRoom));
		}

		//Create the pickup and add it to the current room in the hierarchy
		GameObject instance = Instantiate (toInstantiate, new Vector3 (7, 4, 0f), Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder[currentRoom]);
	}

	//Create the level and set the current player's statistics to track a new dungeon level 
	public void SetupLevel(int _level){
		createLevel (_level);
		GameManager.instance.roomData.CurrentPlayer().newFloor (levelTree.getSize());
	}
}
