using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour {

	public int columns = 13;
	public int rows = 7;

	public GameObject doorLeftTile;
	public GameObject doorRightTile;
	public GameObject doorTopTile;
	public GameObject doorBottomTile;
	public GameObject floorTile;
	public GameObject wallTile;
	public GameObject gapTile;
	public GameObject enemy;
	public GameObject bullet;

	//Current Level
	private mTree levelTree;

	//List of room objects
	//private List<Room> rooms = new List<Room>(); //Holds each room for each index
	private Transform[] roomHolder;
	public int currentRoom = 0; //Current room selected
	public Vector2 currentGridPosition = new Vector2(0,0);

	void createLevel(int _level){
		levelTree = new mTree (_level);
		while (levelTree.getEndRoomCount () < 4) {
			levelTree = new mTree (_level);
		}
		List<Node> levelNodes = levelTree.getNodes ();

		roomHolder = new Transform[levelNodes.Count];

		for (int i = 0; i < levelNodes.Count; i++) {
			Node currentNode = levelNodes[i];
			bool doorUp = false;
			bool doorDown = false;
			bool doorLeft = false;
			bool doorRight = false;

			//Check for child node doors
			if(currentNode.getChildren() != null){
				foreach(int _childIndex in currentNode.getChildren()){
					if(currentNode.getGridPosition().x == levelNodes[_childIndex].getGridPosition().x &&
						currentNode.getGridPosition().y+1 == levelNodes[_childIndex].getGridPosition().y){
						doorUp = true;
					}
					else if(currentNode.getGridPosition().x == levelNodes[_childIndex].getGridPosition().x &&
						currentNode.getGridPosition().y-1 == levelNodes[_childIndex].getGridPosition().y){
						doorDown = true;
					}
					else if(currentNode.getGridPosition().x-1 == levelNodes[_childIndex].getGridPosition().x &&
						currentNode.getGridPosition().y == levelNodes[_childIndex].getGridPosition().y){
						doorLeft = true;
					}
					else if(currentNode.getGridPosition().x+1 == levelNodes[_childIndex].getGridPosition().x &&
						currentNode.getGridPosition().y == levelNodes[_childIndex].getGridPosition().y){
						doorRight = true;
					}
				}
			}

			//Check for parent node door
			if(currentNode.getParent() != -1){
				if(currentNode.getGridPosition().x == levelNodes[currentNode.getParent()].getGridPosition().x &&
					currentNode.getGridPosition().y+1 == levelNodes[currentNode.getParent()].getGridPosition().y){
					doorUp = true;
				}
				else if(currentNode.getGridPosition().x == levelNodes[currentNode.getParent()].getGridPosition().x &&
					currentNode.getGridPosition().y-1 == levelNodes[currentNode.getParent()].getGridPosition().y){
					doorDown = true;
				}
				else if(currentNode.getGridPosition().x-1 == levelNodes[currentNode.getParent()].getGridPosition().x &&
					currentNode.getGridPosition().y == levelNodes[currentNode.getParent()].getGridPosition().y){
					doorLeft = true;
				}
				else if(currentNode.getGridPosition().x+1 == levelNodes[currentNode.getParent()].getGridPosition().x &&
					currentNode.getGridPosition().y == levelNodes[currentNode.getParent()].getGridPosition().y){
					doorRight = true;
				}
			}

			Debug.Log (System.String.Format("{0} : {1} : {2} : {3} : {4}",i, doorUp, doorDown, doorLeft, doorRight));
			roomHolder[i] = new GameObject ("Room"+i).transform;
			createRoom (i, doorUp, doorDown, doorLeft, doorRight);
			addEnemies (i);

			if(i!=0)
				roomHolder [i].gameObject.SetActive (false);
		}
	}

	void createRoom(int _room, bool _doorUp, bool _doorDown, bool _doorLeft, bool _doorRight){
		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for(int x = -1; x < columns + 1; x++)
		{
			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for(int y = -1; y < rows + 1; y++)
			{
				//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
				GameObject toInstantiate = floorTile;

				//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
				if (x == -1 || x == columns || y == -1 || y == rows) {
					toInstantiate = wallTile;
				}
				//Checks if the current position requires a door tile
				if (_doorUp && x == Mathf.Floor (columns / 2f) && y == rows) {
					toInstantiate = doorTopTile;
				}
				else if (_doorDown && x == Mathf.Floor (columns / 2f) && y == -1) {
					toInstantiate = doorBottomTile;
				}
				else if(_doorLeft && y == Mathf.Floor (rows / 2f) && x == -1) {
					toInstantiate = doorLeftTile;
				}
				else if(_doorRight && y == Mathf.Floor (rows / 2f) && x == columns) {
					toInstantiate = doorRightTile;
				}

				if(x > 3 && x < 10 && y > 3 && y < 6){
					toInstantiate = gapTile;
				}

				//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				instance.transform.SetParent (roomHolder[_room]);
			}
		}
	}

	void addEnemies(int _room){
		for(int i=0; i<3; i++){
			GameObject instance = Instantiate (enemy, new Vector3 (Random.Range (0, columns), Random.Range (0, rows), 0f), Quaternion.identity) as GameObject;
			instance.transform.SetParent (roomHolder [_room]);
		}
	}

	//Creates a bullet in the room
	public void createBullet(Vector2 _pos, float _dirX, float _dirY){
		GameObject instance = Instantiate (bullet, _pos, Quaternion.identity) as GameObject;
		instance.gameObject.GetComponent<Bullet> ().changeDirection (_dirX, _dirY);
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

	public void SetupLevel(int _level){
		createLevel (_level);
		//roomSetup ();
	}
}
