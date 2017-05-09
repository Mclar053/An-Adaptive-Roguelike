using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class LoadXmlData : MonoBehaviour{ // the Class
	public TextAsset GameAsset;
	public TextAsset PlayerData;

	//Stores all the different room types in different lists
	List<Room> rooms = new List<Room>();
	List<Room> bossRooms = new List<Room>();
	List<Room> specialRooms = new List<Room>();

	//Store all player statistics in a list
	List<PlayerStats> playerProfiles = new List<PlayerStats>();
	int currentPlayerIndex; //The current player playing

	/* Reference: http://unitynoobs.blogspot.co.uk/2011/02/xml-loading-data-from-xml-file.html
	 * Author: Rodrigo Barros 
	 * 
	 * Loads the room layout xml file and puts the rooms into the room lists (rooms, bossRooms and specialRooms)
	 * 
	 */
	public void loadRooms(){
		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
		xmlDoc.LoadXml(GameAsset.text); // load the file.
		XmlNodeList roomsList = xmlDoc.GetElementsByTagName("room"); // array of the room nodes.

		for (int i=0; i<roomsList.Count; i++){
			XmlNodeList rows = roomsList[i].ChildNodes[0].ChildNodes;
			XmlNodeList entities = roomsList[i].ChildNodes[1].ChildNodes;
			Room newRoom = new Room (15,9,entities.Count);
			//Load attributes that contain base promote and demote values
			newRoom.addBaseModifierValues (float.Parse (roomsList [i].Attributes ["promoteValue"].Value), float.Parse (roomsList [i].Attributes ["demoteValue"].Value));

			//Room Layout
			for (int j=0; j<rows.Count; j++){ // levels itens nodes.
				XmlNodeList columns = rows [j].ChildNodes;
				for (int k=0; k<columns.Count; k++){ // levels itens nodes.
					newRoom.layout[j,k] = int.Parse(columns[k].InnerText);
				}
			}

			//Entity Positions
			//Adds entities and their positions to the room
			for(int j=0; j<entities.Count; j++){
				newRoom.setEntity (j, int.Parse(entities[j].Attributes["column"].Value), int.Parse(entities[j].Attributes["row"].Value), int.Parse(entities[j].Attributes["type"].Value));
			}

			//Checks what room type it is and places it into the appropriate room list
			if(roomsList[i].ChildNodes[3].InnerText == "Normal"){
				rooms.Add(newRoom); 
			} else if(roomsList[i].ChildNodes[3].InnerText == "Boss"){
				bossRooms.Add (newRoom);
			} else{
				specialRooms.Add(newRoom);
			}
		}
	}

	/* Reference: https://msdn.microsoft.com/en-us/library/dw229a22(v=vs.110).aspx
	 * 
	 * Saves all of the player profiles in the player profile list to an xml file (player-profiles.xml)
	 */
	public void savePlayerProfiles(){
		XmlDocument profileXML = new XmlDocument ();
		string xmlToSave = "<AllPlayers>";
		for (int i = 0; i < playerProfiles.Count; i++) {
			xmlToSave += "<player id='" + playerProfiles[i].userID + "'><roomStats>";

			for (int j = 0; j < rooms.Count + bossRooms.Count + specialRooms.Count; j++) {
				if (playerProfiles [i].roomTypePlayed (j)) {
					xmlToSave += "<room id='" + j + "' currentMod='" + playerProfiles[i].getRoomModifer(j) + "'>";
					foreach(KeyValuePair<int,List<RoomStats>> _room in playerProfiles [i].getRoomInstances (j)){
						xmlToSave += "<roomMod id='"+ _room.Key +"'>";

						List<RoomStats> currentPlayedRoomStats = playerProfiles [i].getRoomModInstances (j,_room.Key);
						for (int k = 0; k < currentPlayedRoomStats.Count; k++) {
							xmlToSave += "<stat id='" + k + "' startTime='" + currentPlayedRoomStats [k].startTime + "' endTime='" + currentPlayedRoomStats [k].endTime + "' firstEnemyTime='" + currentPlayedRoomStats [k].timeToKillFirstEnemy + "' hitTime='" + currentPlayedRoomStats [k].timeToGetHit + "' damageTaken='" + currentPlayedRoomStats [k].damageTakenInRoom + "'/>";
						}
						xmlToSave += "</roomMod>";
					}
					xmlToSave += "</room>";
				}
			}
			xmlToSave += "</roomStats></player>";
		}
		xmlToSave += "</AllPlayers>";
		profileXML.LoadXml (xmlToSave);
		profileXML.Save(Application.dataPath+"/player-profiles.xml");
	}


	//Loads all of the player profiles from the player-profiles.xml file to the player profile list
	public void loadPlayerProfiles(){
		XmlDocument profileXML = new XmlDocument ();
		Debug.Log(Application.dataPath);

		if (!File.Exists (Application.dataPath + "/player-profiles.xml")) {
			this.savePlayerProfiles ();
		}
		profileXML.Load (Application.dataPath+"/player-profiles.xml");
		XmlNodeList players = profileXML.GetElementsByTagName("player");
		for(int i=0; i<players.Count; i++){
			PlayerStats newPlayer = new PlayerStats (i);
			newPlayer.resetPlayerStats ();
			newPlayer.userID = int.Parse(players [i].Attributes ["id"].Value);

			XmlNodeList playerRooms = players [i].ChildNodes[0].ChildNodes;
			for(int j=0; j<playerRooms.Count; j++){ //Loop through all rooms
				XmlNodeList roomMods = playerRooms [j].ChildNodes;
				newPlayer.setRoomModifier (j, int.Parse (playerRooms [j].Attributes ["currentMod"].Value));

				for (int l = 0; l < roomMods.Count; l++) { //Loop through all room modifiers
					XmlNodeList roomInstances = roomMods [l].ChildNodes;

					for (int k = 0; k < roomInstances.Count; k++) { //Loop through all stats in room modifiers
						RoomStats instance = new RoomStats ();
						instance.roomID = int.Parse (playerRooms [j].Attributes ["id"].Value);
						instance.modID = int.Parse (roomMods [l].Attributes ["id"].Value);
						instance.startTime = float.Parse (roomInstances [k].Attributes ["startTime"].Value);
						instance.endTime = float.Parse (roomInstances [k].Attributes ["endTime"].Value);
						instance.timeToKillFirstEnemy = float.Parse (roomInstances [k].Attributes ["firstEnemyTime"].Value);
						instance.timeToGetHit = float.Parse (roomInstances [k].Attributes ["hitTime"].Value);
						instance.damageTakenInRoom = float.Parse (roomInstances [k].Attributes ["damageTaken"].Value);

						newPlayer.createInstance (instance);
					}
				}
			}
			playerProfiles.Add (newPlayer);
		}
	}

	public bool loadPlayer(int _playerID){
		if (_playerID >= 0 && _playerID < playerProfiles.Count) {
			currentPlayerIndex = _playerID;
			return true;
		}
		return false;
	}

	public PlayerStats CurrentPlayer(){
		return playerProfiles [currentPlayerIndex];
	}

	public void addPlayer(){
		currentPlayerIndex = playerProfiles.Count;
		playerProfiles.Add (new PlayerStats(playerProfiles.Count));
	}

	public int getNewPlayerID(){
		return playerProfiles.Count;
	}

	//Room Counts
	public int getNumberOfRooms(){
		return rooms.Count;
	}

	public int getNumberOfBossRooms(){
		return bossRooms.Count;
	}

	public int getNumberOfSpecialRooms(){
		return specialRooms.Count;
	}

	public int getTotalNumberOfRooms(){
		return rooms.Count + bossRooms.Count + specialRooms.Count;
	}

	//Normal rooms
	public int[,] getRoomLayout(int _room){
		return rooms[_room].layout;
	}

	public int[,] getRoomEntities(int _room){
		return rooms[_room].entities;
	}

	//Boss rooms
	public int[,] getBossRoomLayout(int _room){
		Debug.Log (_room + "  " + getNumberOfBossRooms());
		return bossRooms[_room].layout;
	}

	public int[,] getBossRoomEntities(int _room){
		return bossRooms[_room].entities;
	}

	//Special rooms
	public int[,] getSpecialRoomLayout(int _room){
		return specialRooms[_room].layout;
	}

	public int[,] getSpecialRoomEntities(int _room){
		return specialRooms[_room].entities;
	}

	public void printPlayers(){
		Debug.Log (playerProfiles.Count);
	}

	//Checks the room modifier based on the performance score the player has achieved with the currentRoomModifier
	public int checkRoomModifier(int _roomID, float _roomScore, int currentRoomMod){
		if(_roomID >=0 && _roomID < getNumberOfRooms()){
			return rooms [_roomID].checkRoomModifier (_roomScore, currentRoomMod);
		}
		else if(_roomID >=getNumberOfRooms() && _roomID < getNumberOfRooms()+getNumberOfBossRooms()){
			return bossRooms [_roomID-getNumberOfRooms()].checkRoomModifier (_roomScore, currentRoomMod);
		} else{
			return specialRooms [_roomID].checkRoomModifier (_roomScore, currentRoomMod);
		}

	}
}