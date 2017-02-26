using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public Transform roomHolder;
	public int roomId;
	public bool complete;

	public int columns = 13;
	public int rows = 7;

	public GameObject doorTile;
	public GameObject floorTile;
	public GameObject wallTile;
	public GameObject gapTile;

	public Room(int _roomNum=0, bool _doorUp=false, bool _doorDown=false, bool _doorLeft=false, bool _doorRight=false){
		roomHolder = new GameObject ("Room"+_roomNum).transform;
		createRoom (_doorUp, _doorDown, _doorLeft, _doorRight);
	}

	void createRoom(bool _doorUp, bool _doorDown, bool _doorLeft, bool _doorRight){
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
				else if ((_doorUp && x == Mathf.Ceil (columns / 2) && y == -1) ||
				           (_doorDown && x == Mathf.Ceil (columns / 2) && y == columns) ||
				           (_doorLeft && y == Mathf.Ceil (rows / 2) && x == -1) ||
				           (_doorRight && y == Mathf.Ceil (rows / 2) && x == rows)) {
					toInstantiate = doorTile;
				}

				//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				instance.transform.SetParent (roomHolder);
			}
		}
	}

}
