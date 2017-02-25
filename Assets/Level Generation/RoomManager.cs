using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour {

	public int columns = 13;
	public int rows = 7;

	public GameObject doorTile;
	public GameObject floorTile;
	public GameObject wallTile;
	public GameObject gapTile;

	//List of room objects
	private List<Room> rooms; //Holds each room for each index
	private Transform roomHolder;
	public int currentRoom; //Current room selected

	void roomSetup(){
		roomHolder = new GameObject ("currentRoom").transform;

		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for(int x = -1; x < columns + 1; x++)
		{
			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for(int y = -1; y < rows + 1; y++)
			{
				//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
				GameObject toInstantiate = floorTile;

				//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
				if(x == -1 || x == columns || y == -1 || y == rows){
					toInstantiate = wallTile;
				}

				//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				instance.transform.SetParent (roomHolder);
			}
		}
	}

	public void setupRooms(){
		roomSetup ();
	}

	void moveIntoRoom(int _old, int _new){

	}

}
