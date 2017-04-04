using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats {

	int userID;
	Dictionary<int,List<RoomStats>> roomStats;

	public PlayerStats(){
		roomStats = new Dictionary<int, List<RoomStats>> ();
	}

	//@Input: int _roomIndex -> the index of the room that has been selected
	//@Output: List<RoomStats> -> the list of RoomStats objects that have been created for the room selected
	//----
	//@Method: Selects a room and returns all the instances of that room
	public List<RoomStats> getRoomInstances(int _roomIndex){

	}

	//@Input: int _roomIndex -> the index of the room that has been selected
	//@Output: int -> the index of the instance that has just been created
	//----
	//@Method: Create a new instance of a room to gather stats of how the player has performed
	public int createInstance(int _roomIndex){
		roomStats [_roomIndex].Add (new RoomStats());
		return roomStats [_roomIndex].Count - 1;
	}



	public void setStat(int _roomIndex, int _instanceIndex){

	}
}
