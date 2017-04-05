using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour{

	int userID;
	Dictionary<int,List<RoomStats>> roomStats;
	RoomStats[] currentFloor;

	void Awake(){
		roomStats = new Dictionary<int, List<RoomStats>> ();
	}

	public void newFloor(int _numberOfRooms){
		currentFloor = new RoomStats[_numberOfRooms];
		currentFloor [0] = new RoomStats ();
		currentFloor [0].startTime = Time.time;
	}

	public void storeFloorData(){
		for(int i=0; i<currentFloor.Length; i++){
			RoomStats _room = currentFloor [i];
			if(_room != null){
				createInstance (_room);
			}
		}
	}

	/*@Input:
	 * int _roomIndex -> the index of the room that has been selected
	 * ----
	 * @Output:
	 * List<RoomStats> -> the list of RoomStats objects that have been created for the room selected
	 * ----
	 * @Method: Selects a room and returns all the instances of that room
	 */
	public List<RoomStats> getRoomInstances(int _roomIndex){
		return roomStats [_roomIndex];
	}

	public void createCurrentFloorRoom(int _roomIndex){
		if (currentFloor [_roomIndex] == null) {
			currentFloor [_roomIndex] = new RoomStats ();
		}
	}

	/*@Input
	 * int _roomIndex -> the index of the room that has been selected
	 * ----
	 * @Output 
	 * int -> the index of the instance that has just been created
	 * ----
	 * @Method: Create a new instance of a room to gather stats of how the player has performed
	 */
	public int createInstance(RoomStats _room){
		if (!roomStats.ContainsKey (_room.roomID)) {
			roomStats.Add (_room.roomID, new List<RoomStats>());
		}
		roomStats [_room.roomID].Add (new RoomStats(_room));
		return roomStats [_room.roomID].Count - 1;
	}

	/*@Input
	 * int _roomIndex -> the index of the room that has been selected
	 * ----
	 * @Output 
	 * void
	 * ----
	 * @Method: Sets the start time of the room instance selected to the current time. This'll be used to calculate time based stats.
	 */
	public void startRoomTime(int _roomIndex){
		currentFloor[_roomIndex].startTime = Time.time;
	}

	/*@Input
	 * int _roomIndex -> the index of the room that has been selected
	 * ----
	 * @Output 
	 * void
	 * ----
	 * @Method: Sets the end time of the room instance selected to the current time. This'll be used to calculate time based stats.
	 */
	public void endRoomTime(int _roomIndex){
		currentFloor[_roomIndex].endTime = Time.time;
		currentFloor[_roomIndex].completed ();
	}

	public bool roomCompleted(int _roomIndex){
		if (currentFloor [_roomIndex] != null) {
			if (currentFloor [_roomIndex].isComplete ()) {
				return true;
			}
		}
		return false;
	}

	public void printStats(){
		Debug.Log ("-------ALL ROOM STATISTICS-------");
		foreach (KeyValuePair<int, List<RoomStats>> _key in roomStats) {
			for (int i = 0; i < _key.Value.Count; i++) {
				Debug.Log (System.String.Format("{0}:{1} StartTime:{2} EndTime:{3}",_key.Key, i, _key.Value[i].startTime,_key.Value[i].endTime));
			}
		}
	}

	public void printCurrentFloorStats(){
		Debug.Log ("-------CURRENT ROOM STATISTICS-------");
		for (int i = 0; i < currentFloor.Length; i++) {
			if (currentFloor [i] != null) {
				Debug.Log (System.String.Format("{0}: StartTime:{1}  EndTime:{2}",i,currentFloor[i].startTime,currentFloor[i].endTime));
			}
		}
	}
}
