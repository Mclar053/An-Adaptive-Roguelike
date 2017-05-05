using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Xml;

public class RoomManager : MonoBehaviour {

	public int columns = 15;
	public int rows = 9;

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

	public GameObject score;
	public GameObject health;
	public GameObject[] upgrades;

	//Current Level
	private mTree levelTree;

	//List of room objects
	//private List<Room> rooms = new List<Room>(); //Holds each room for each index
	private Transform[] roomHolder = new Transform[0];
	private Transform playerTransform;
	private Transform healthBarTransform;
	public int currentRoom; //Current room selected
	public Vector2 currentGridPosition = new Vector2(0,0);
	private int currentLevel;

	void createLevel(int _level){
		currentLevel = _level;
		destroyLevel ();

		levelTree = new mTree (_level);
		while (levelTree.getEndRoomCount () < 4) {
			levelTree = new mTree (_level);
		}
		levelTree.makeBossRoom ();
		List<Node> levelNodes = levelTree.getNodes ();

		roomHolder = new Transform[levelNodes.Count];
		currentRoom = 0;
		currentGridPosition = new Vector2(0,0);

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

			if (currentNode.getRoomType () == 1) {
				roomLayout = GameManager.instance.roomData.getBossRoomLayout (currentNode.getRoomID ()-GameManager.instance.numberOfRooms);
				roomEntities = GameManager.instance.roomData.getBossRoomEntities (currentNode.getRoomID ()-GameManager.instance.numberOfRooms);
			} else if (currentNode.getRoomType () == 2) {
				roomLayout = GameManager.instance.roomData.getSpecialRoomLayout (currentNode.getRoomID ());
				roomEntities = GameManager.instance.roomData.getSpecialRoomEntities (currentNode.getRoomID ());
			} else {
				roomLayout = GameManager.instance.roomData.getRoomLayout (currentNode.getRoomID ());
				roomEntities = GameManager.instance.roomData.getRoomEntities (currentNode.getRoomID ());
			}


			float performanceScore = GameManager.instance.statistics.getRoomAvergePerformance (currentNode.getRoomID ());
			int currentMod = GameManager.instance.statistics.getRoomModifer (currentNode.getRoomID ());

			//PLAYTEST - COMMENT
			//------
			if (currentNode.getRoomID () != 0) {
				if (performanceScore >= 0) {
					Debug.Log ("Before "+currentMod+" ROOMID "+currentNode.getRoomID ()+" MOD "+GameManager.instance.statistics.getRoomModifer(currentNode.getRoomID()));
					currentMod += GameManager.instance.roomData.checkRoomModifier (currentNode.getRoomID (), performanceScore, currentMod);
					GameManager.instance.statistics.setRoomModifier (currentNode.getRoomID (), currentMod);
					Debug.Log ("After "+currentMod+" ROOMID "+currentNode.getRoomID ()+" MOD "+GameManager.instance.statistics.getRoomModifer(currentNode.getRoomID()));
				}
			}
			//-----

			//Debug.Log (System.String.Format("{0} : {1} : {2} : {3} : {4}",i, doors[0],doors[1],doors[2],doors[3]));
			roomHolder[i] = new GameObject ("Room"+i).transform;
			createRoom (i, doors, roomLayout);
			if (i != 0) {
				addEnemies (i, roomEntities, currentMod); //First room is safe
				roomHolder [i].gameObject.SetActive (false);
			}
		}
	}

	public void destroyLevel(){
		foreach(Transform _t in roomHolder){
			if (_t != null) {
				Destroy (_t.gameObject);
			}
		}
	}

	private bool checkDoor(int _xMod, int _yMod, Node _currentNode, Node _otherNode){
		if(_currentNode.getGridPosition().x + _xMod == _otherNode.getGridPosition().x &&
			_currentNode.getGridPosition().y + _yMod == _otherNode.getGridPosition().y){
			return true;
		}
		return false;
	}

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

	public void createPlayer(){
		playerTransform = new GameObject ("PlayerTransform").transform;
		GameObject instance = Instantiate (player, new Vector3 (7, 4, 0), Quaternion.identity) as GameObject;
		instance.transform.SetParent (playerTransform);
	}

	public void destroyPlayer(){
		Destroy (playerTransform.gameObject);
	}

	void addEnemies(int _room, int[,] _entities, int _mod){
		for(int i=0; i<_entities.GetLength(0); i++){
//			Debug.Log (System.String.Format("{0}, {1}, {2}, {3}",i,_entities[i,0],_entities[i,1],_entities[i,2]));
			GameObject instance = Instantiate (enemy[_entities[i,0]], new Vector3 (_entities[i,1],_entities[i,2], 0f), Quaternion.identity) as GameObject;
			if (instance.GetComponent<movingObject> () != null) {
				instance.GetComponent<movingObject> ().modifier = _mod;
			}
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

	public bool checkRoomComplete(){
		if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0){
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
	 * 
	 * -----
	 * @Output
	 * 
	 * -----
	 * @Method: Return room id of a particular node in the level tree
	*/
	public int getLevelNodeRoomID(int _nodeIndex){
		return levelTree.getNode (_nodeIndex).getRoomID ();
	}

//	void openPortal(GameObject _portal){
//		if(_portal != null){
//			_portal.GetComponent<Collider2D> ();
//		}
//	}

	void openDoor(GameObject _door){
		if(_door != null){
			_door.GetComponent<Collider2D> ().isTrigger = true;
			_door.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f);
		}
	}

	public void createPickup(){
		GameObject toInstantiate;
		float chance = Random.Range (0, 100);

		toInstantiate = health;
		toInstantiate.GetComponent<Pickup> ().setValue (Random.Range (1,3));

		if (chance > 30f) {
			toInstantiate = score;
			toInstantiate.GetComponent<Pickup> ().setValue (Random.Range (currentLevel, currentLevel + currentRoom));
		}

		GameObject instance = Instantiate (toInstantiate, new Vector3 (7, 4, 0f), Quaternion.identity) as GameObject;
		instance.transform.SetParent (roomHolder[currentRoom]);
	}

	public void SetupLevel(int _level){
		createLevel (_level);
		GameManager.instance.statistics.newFloor (levelTree.getSize());
		//roomSetup ();
	}
}
