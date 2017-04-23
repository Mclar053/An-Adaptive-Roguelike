using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats {

	//Store current mod for each room

	public int userID = 0;
	Dictionary<int,Dictionary<int,List<RoomStats>>> roomStats = new Dictionary<int, Dictionary<int, List<RoomStats>>> ();
	Dictionary<int, int> roomModifiers = new Dictionary<int,int>(); //Contains all the current room modifiers for the player 
	RoomStats[] currentFloor;

	public PlayerStats(){}

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

	public void setRoomModifier(int _roomID, int _modID = 0){
		if (roomModifiers.ContainsKey(_roomID)) { //Checks if key exists in roomModifiers Dictionary
			if(roomStats[_roomID][roomModifiers [_roomID]].Count >= 2){
				if (roomModifiers [_roomID] != _modID) { //If the values are different. UPDATE --> CHECKS THAT IT MAKES SENSE Remove all old statistics the mod room roomStats place.
					roomStats [_roomID] [_modID] = new List<RoomStats> ();
				}
				roomModifiers [_roomID] = _modID; //Change the current room modifier for the player
			}
		} else {
			roomModifiers.Add (_roomID, _modID); //Adds a roomModifier for the room
		}
	}

	public int getRoomModifer(int _roomID){
		if(!roomModifiers.ContainsKey(_roomID)){ //Checks if a value exists in roomModifiers Dictionary
			setRoomModifier(_roomID); //Create a room modifier
		}
		return roomModifiers [_roomID]; //Return the modID
	}
		
	public void copyRoomStats(PlayerStats _player){
		for(int i=0; i<GameManager.instance.roomData.getTotalNumberOfRooms(); i++){
			this.setRoomModifier (i, _player.getRoomModifer (i));
			this.setRoomInstances (i, _player.getRoomInstances (i));
		}
	}

	public void setRoomInstances(int _roomIndex, Dictionary<int, List<RoomStats>> _stats){
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
	public Dictionary<int, List<RoomStats>> getRoomInstances(int _roomIndex){
		if(!roomStats.ContainsKey(_roomIndex)){
			return null;
		}
		return roomStats [_roomIndex];
	}

	//UPDATE THE COMMENT
	public void setRoomModInstances(int _roomIndex, int _modID, List<RoomStats> _stats){
		if (_stats != null) {
			if (!roomStats[_roomIndex].ContainsKey (_modID)) {
				roomStats[_roomIndex].Add (_modID, _stats);
			} else {
				roomStats [_roomIndex][_modID] = _stats;
			}
		}
	}

	/*@Input:
	 * //UPDATE!!!
	 * int _roomIndex -> the index of the room that has been selected
	 * ----
	 * @Output:
	 * List<RoomStats> -> the list of RoomStats objects that have been created for the room selected
	 * ----
	 * @Method: Selects a room and returns all the instances of that room
	 */
	public List<RoomStats> getRoomModInstances(int _roomIndex, int _modID){
		if(!roomStats.ContainsKey(_roomIndex)){
			return null;
		}
		else if(!roomStats[_roomIndex].ContainsKey(_modID)){
			return null;
		}
		return roomStats [_roomIndex][_modID];
	}

	public bool roomTypePlayed(int _roomID){
		if (roomStats.ContainsKey (_roomID)) {
			return true;
		}
		return false;
	}

	public bool roomModPlayed(int _roomID, int _modID = 0){
		if (roomStats [_roomID].ContainsKey (_modID)) {
			return true;
		}
		return false;
	}

	public void createCurrentFloorRoom(int _roomIndex, int _roomID){
		if (currentFloor [_roomIndex] == null) {
			currentFloor [_roomIndex] = new RoomStats ();
			currentFloor [_roomIndex].roomID = _roomID;
			if (roomModifiers.ContainsKey (_roomID)) {
				currentFloor [_roomIndex].modID = roomModifiers[_roomID];
			} else {
				currentFloor [_roomIndex].modID = 0;
			}

		}
	}

	/*@Input
	 * RoomStats _roomIndex -> the RoomStats object which has been selected to be added
	 * ----
	 * @Output 
	 * int -> the index of the instance that has just been created
	 * ----
	 * //UPDATE
	 * @Method: Create a new instance of a room to gather stats of how the player has performed
	 */
	public int createInstance(RoomStats _room){
		if (!roomStats.ContainsKey (_room.roomID)) {
			roomStats.Add (_room.roomID, new Dictionary<int, List<RoomStats>>());
		}
		createModInstance (_room);
		return roomStats [_room.roomID].Count - 1;
	}
		
	/*@Input
	 * RoomStats _roomIndex -> the index of the room that has been selected
	 * ----
	 * @Output 
	 * int -> the index of the instance that has just been created
	 * ----
	 * //UPDATE
	 * @Method: Create a new instance of a room to gather stats of how the player has performed
	 */
	public int createModInstance(RoomStats _room){
		if (!roomStats[_room.roomID].ContainsKey (_room.modID)) {
			roomStats[_room.roomID].Add (_room.modID, new List<RoomStats>());
		}
		roomStats [_room.roomID][_room.modID].Add (new RoomStats(_room));
		if(roomStats [_room.roomID][_room.modID].Count > 10){
			roomStats [_room.roomID] [_room.modID].RemoveAt (0);
		}
		return roomStats [_room.roomID][_room.modID].Count - 1;
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

	public float getRoomAvergePerformance(int _roomID){
		float totalScore = 0;
		if (!roomStats.ContainsKey (_roomID)) { //Room doesn't exist
			totalScore = -2;
		}
		else if(!roomStats[_roomID].ContainsKey (roomModifiers[_roomID])){ //The mod of the room selected doesn't exist
			totalScore = -3;
		}
		else if(roomStats[_roomID][roomModifiers[_roomID]].Count < 5){ //Room mod doesn't have enough data
			totalScore = -1;
		} else { //Room mod has enough data and exists
			List<RoomStats> selectedRoom = roomStats [_roomID][roomModifiers[_roomID]];
			for(int i=0; i<selectedRoom.Count; i++){ //Calculate score for each room
				totalScore+=selectedRoom[i].performanceScore() * (-0.01f * Mathf.Pow(i,2) + 1);
			}
			totalScore /= selectedRoom.Count;
		}

		return totalScore;
	}

	public void printStats(){
		Debug.Log ("-------ALL ROOM STATISTICS-------");
		foreach (KeyValuePair<int, Dictionary<int,List<RoomStats>>> _room in roomStats) {
			foreach (KeyValuePair<int, List<RoomStats>> _key in _room.Value) {
				for (int i = 0; i < _key.Value.Count; i++) {
					Debug.Log (System.String.Format ("Room:{0} Mod:{1} statID:{2} StartTime:{3} EndTime:{4} EnemyKilled:{5} PlayerDamaged:{6}",_room.Key, _key.Key, i, _key.Value [i].startTime, _key.Value [i].endTime, _key.Value [i].timeToKillFirstEnemy, _key.Value [i].damageTakenInRoom));
				}
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
