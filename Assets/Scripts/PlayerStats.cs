using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour{

	int userID;
	Dictionary<int,List<RoomStats>> roomStats;
	RoomStats[] currentFloor;

	void Awake(){
		userID = -1;
		roomStats = new Dictionary<int, List<RoomStats>> ();
	}

	public void loadStats(int _id, Dictionary<int,List<RoomStats>> _stats){
		userID = _id;
		roomStats = _stats;
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

	public bool roomTypePlayed(int _roomID){
		if(roomStats.ContainsKey(_roomID)){
			return true;
		}
		return false;
	}

	public void createCurrentFloorRoom(int _roomIndex, int _roomID){
		if (currentFloor [_roomIndex] == null) {
			currentFloor [_roomIndex] = new RoomStats ();
			currentFloor [_roomIndex].roomID = _roomID;
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

	public bool firstEnemyKilled(int _roomIndex){
		if (currentFloor [_roomIndex].timeToKillFirstEnemy == 0) {
			currentFloor [_roomIndex].timeToKillFirstEnemy = Time.time;
			return true;
		}
		return false;
	}

	public bool firstDamageToPlayer(int _roomIndex){
		if (currentFloor [_roomIndex].timeToGetHit == 0) {
			currentFloor [_roomIndex].timeToGetHit = Time.time;
			return true;
		}
		return false;
	}

	public void playerDamaged(int _roomIndex, float _dmg){
		currentFloor [_roomIndex].damageTakenInRoom+=_dmg;
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
				Debug.Log (System.String.Format("{0}:{1} StartTime:{2} EndTime:{3} EnemyKilled:{4} PlayerDamaged:{5}",_key.Key, i, _key.Value[i].startTime,_key.Value[i].endTime,_key.Value[i].timeToKillFirstEnemy,_key.Value[i].damageTakenInRoom));
			}
		}
	}

	public void printCurrentFloorStats(){
		Debug.Log ("-------CURRENT ROOM STATISTICS-------");
		for (int i = 0; i < currentFloor.Length; i++) {
			if (currentFloor [i] != null) {
				Debug.Log (System.String.Format("{0}: StartTime:{1}  EndTime:{2} EnemyKilled:{3} PlayerDamaged:{4}",i,currentFloor[i].startTime,currentFloor[i].endTime,currentFloor[i].timeToKillFirstEnemy,currentFloor[i].damageTakenInRoom));
			}
		}
	}
}
