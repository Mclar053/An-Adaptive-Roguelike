using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats {

	public int userID = 0;
	Dictionary<int,List<RoomStats>> roomStats = new Dictionary<int, List<RoomStats>> ();
	RoomStats[] currentFloor;

	public PlayerStats(){

	}

	public void resetPlayerStats(){
	}

	public bool loadPlayer(PlayerStats _player){
		if (_player == null) {
			return false;
		} else{
			userID = _player.userID;
			copyRoomStats (_player);
			return true;
		}

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

	public void copyRoomStats(PlayerStats _player){
		for(int i=0; i<GameManager.instance.roomData.getTotalNumberOfRooms(); i++){
			this.setRoomInstances (i, _player.getRoomInstances (i));
		}
	}

	public void setRoomInstances(int _roomIndex, List<RoomStats> _stats){
		if (_stats != null) {
			if (!roomStats.ContainsKey (_roomIndex)) {
				roomStats.Add (_roomIndex, _stats);
			} else {
				roomStats [_roomIndex] = _stats;
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
		if(!roomStats.ContainsKey(_roomIndex)){
			return null;
		}
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

	public float getRoomAvergePerformance(int _roomIndex){
		float totalScore = 0;
		if (!roomStats.ContainsKey (_roomIndex)) { //Room doesn't exist
			totalScore = -2;
		} else if(roomStats[_roomIndex].Count < 3){ //Room doesn't have enough data
			totalScore = -1;
		} else { //Room has enough data and exists
			List<RoomStats> selectedRoom = roomStats [_roomIndex];
			for(int i=0; i<selectedRoom.Count; i++){ //Calculate score for each room
				totalScore+=selectedRoom[i].performanceScore() * (-0.01f * Mathf.Pow(i,2) + 1);
			}
			totalScore /= selectedRoom.Count;
		}

		return totalScore;
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
