using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats {

	public int userID = 0; //The user ID
	Dictionary<int,Dictionary<int,List<RoomStats>>> allStatistics = new Dictionary<int, Dictionary<int, List<RoomStats>>> (); //Contains all the statistics the player will have for all rooms and modifiers of those rooms
	Dictionary<int, int> roomModifiers = new Dictionary<int,int>(); //Contains all the current room modifiers for the player 
	RoomStats[] currentFloor; //Contains stats of the current floor the player is playing

	//Constructor for a new player
	public PlayerStats(int _userID){
		userID = _userID;
		resetPlayerStats ();
	}

	//Resets all allStatistics and modifiers for the player
	public void resetPlayerStats(){
		allStatistics = new Dictionary<int, Dictionary<int, List<RoomStats>>> ();
		roomModifiers = new Dictionary<int,int> ();
	}

	//Resets currentFloor to an array of allStatistics at the length of the number of rooms in the level
	public void newFloor(int _numberOfRooms){
		currentFloor = new RoomStats[_numberOfRooms];
		currentFloor [0] = new RoomStats (); //Create a allStatistics for the spawn room
		currentFloor [0].startTime = Time.time; //Start the timer for the spawn room
	}

	//Transfers all data from currentFloor into allStatistics
	public void storeFloorData(){
		for(int i=0; i<currentFloor.Length; i++){
			RoomStats _room = currentFloor [i];
			//Checks if the room has been complete
			if(_room != null){
				//Add this room to allStatistics
				createInstance (_room);
			}
		}
	}

	//Sets the modifier of the current room ID 
	public void setRoomModifier(int _roomID, int _modID = 0){
		if (roomModifiers.ContainsKey(_roomID)) { //Checks if key exists in roomModifiers Dictionary
			if (allStatistics [_roomID] [roomModifiers [_roomID]].Count >= 3) {
				if (roomModifiers [_roomID] != _modID) { //If the modifiers are different.
					allStatistics [_roomID] [_modID] = new List<RoomStats> (); //Remove all old roomStat entries from the selected room and mod list
				}
				roomModifiers [_roomID] = _modID; //Change the current room modifier for the player
			}
		} else {
			roomModifiers.Add (_roomID, _modID); //Adds a roomModifier for the room
		}
	}

	//Retrieves the current modifier for the given roomID
	//If not found, create a new modifier
	public int getRoomModifer(int _roomID){
		if(!roomModifiers.ContainsKey(_roomID)){ //Checks if a value exists in roomModifiers Dictionary
			setRoomModifier(_roomID); //Create a room modifier
		}
		return roomModifiers [_roomID]; //Return the modID
	}

	//Removes a roomStat entry from currentFloor
	//--Currently used for removing the stats for the room where player has died
	public void removeCurrentRoomStat(int _roomIndex){
		currentFloor[_roomIndex] = null;
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
		if(!allStatistics.ContainsKey(_roomIndex)){
			return null;
		}
		return new Dictionary<int, List<RoomStats>>(allStatistics [_roomIndex]);
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
		if(!allStatistics.ContainsKey(_roomIndex)){
			return null;
		}
		else if(!allStatistics[_roomIndex].ContainsKey(_modID)){
			return null;
		}
		return allStatistics [_roomIndex][_modID];
	}

	//Checks if the player has played a room with the given room ID
	public bool roomTypePlayed(int _roomID){
		if (allStatistics.ContainsKey (_roomID)) {
			return true;
		}
		return false;
	}

	//Checks if a player has played a modified room with a given room ID and modID
	public bool roomModPlayed(int _roomID, int _modID = 0){
		if (allStatistics [_roomID].ContainsKey (_modID)) {
			return true;
		}
		return false;
	}

	//Creates a new room instance on the current level
	public void createCurrentFloorRoom(int _roomIndex, int _roomID){
		//Checks if the room has not been visited
		if (currentFloor [_roomIndex] == null) {
			//Creates a new roomStat object
			currentFloor [_roomIndex] = new RoomStats ();
			currentFloor [_roomIndex].roomID = _roomID;
			//Checks if the roomModifiers contains a value for the room ID of the current room
			if (roomModifiers.ContainsKey (_roomID)) {
				//If so, then set roomStat modifier object to what the list has
				currentFloor [_roomIndex].modID = roomModifiers[_roomID];
			} else {
				//Else set the roomStat modifier to 0
				currentFloor [_roomIndex].modID = 0;
			}

		}
	}

	/*@Input
	 * RoomStats _room -> the RoomStats object which has been selected to be added
	 * ----
	 * @Output 
	 * int -> the index of the instance that has just been created
	 * ----
	 * //UPDATE
	 * @Method: Create a new instance of a room to gather stats of how the player has performed
	 */
	public int createInstance(RoomStats _room){
		if (!allStatistics.ContainsKey (_room.roomID)) {
			allStatistics.Add (_room.roomID, new Dictionary<int, List<RoomStats>>());
		}
		createModInstance (_room);
		return allStatistics [_room.roomID].Count - 1;
	}
		
	/*@Input
	 * RoomStats _room -> the index of the room that has been selected
	 * ----
	 * @Output 
	 * int -> the index of the instance that has just been created
	 * ----
	 * //UPDATE
	 * @Method: Create a new instance of a room to gather stats of how the player has performed
	 */
	public int createModInstance(RoomStats _room){
		if (!allStatistics[_room.roomID].ContainsKey (_room.modID)) {
			allStatistics[_room.roomID].Add (_room.modID, new List<RoomStats>());
		}
		allStatistics [_room.roomID][_room.modID].Add (new RoomStats(_room));

		//If the player has more than 10 successful attempts in a particular modified room then remove the oldest entry
		//This keeps the score up to date
		//------
		if(allStatistics [_room.roomID][_room.modID].Count > 10){ 
			allStatistics [_room.roomID] [_room.modID].RemoveAt (0);
		}
		//------

		return allStatistics [_room.roomID][_room.modID].Count - 1;
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

	//Sets the stat of firstEnemyKilled for the current level room index to the current time
	public bool firstEnemyKilled(int _roomIndex){
		if (currentFloor [_roomIndex].timeToKillFirstEnemy == 0) {
			currentFloor [_roomIndex].timeToKillFirstEnemy = Time.time;
			return true;
		}
		return false;
	}

	//Sets the stat of timeToGetHit for the current level room index to the current time
	public bool firstDamageToPlayer(int _roomIndex){
		if (currentFloor [_roomIndex].timeToGetHit == 0) {
			currentFloor [_roomIndex].timeToGetHit = Time.time;
			return true;
		}
		return false;
	}

	//If the player is damaged, add the damage taken to to damageTakenInRoom stat
	public void playerDamaged(int _roomIndex, float _dmg){
		currentFloor [_roomIndex].damageTakenInRoom+=_dmg;
	}

	//Checks if the room has been completed by the player
	public bool roomCompleted(int _roomIndex){
		if (currentFloor [_roomIndex] != null) {
			if (currentFloor [_roomIndex].isComplete ()) {
				return true;
			}
		}
		return false;
	}

	//Calculates the average performance of a room for the player
	public float getRoomAvergePerformance(int _roomID){
		float totalScore = 0;
		if (!allStatistics.ContainsKey (_roomID)) { //Room doesn't exist
			totalScore = -2;
		}
		else if (!roomModifiers.ContainsKey(_roomID)) { //The mod of the room selected doesn't exist in roomModifiers
			totalScore = -4;
		}
		else if(!allStatistics[_roomID].ContainsKey (roomModifiers[_roomID])){ //The mod of the room selected doesn't exist in allStatistics
			totalScore = -3;
		}
		else if(allStatistics[_roomID][roomModifiers[_roomID]].Count < 3){ //Room mod doesn't have enough data
			totalScore = -1;
		} else { //Room mod has enough data and exists
			List<RoomStats> selectedRoom = allStatistics [_roomID][roomModifiers[_roomID]];
			for(int i=0; i<selectedRoom.Count; i++){ //Calculate score for each room
				totalScore+=selectedRoom[selectedRoom.Count-1-i].performanceScore() * (-0.01f * Mathf.Pow(i,2) + 1);
			}
			//Calculates the average by dividing by the number of rooms completed
			totalScore /= selectedRoom.Count;
		}

		return totalScore;
	}

	public void printStats(){
		Debug.Log ("-------ALL ROOM STATISTICS-------");
		foreach (KeyValuePair<int, Dictionary<int,List<RoomStats>>> _room in allStatistics) {
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
