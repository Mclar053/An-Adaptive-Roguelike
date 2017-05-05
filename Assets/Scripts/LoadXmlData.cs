using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class LoadXmlData : MonoBehaviour{ // the Class
	public TextAsset GameAsset;
	public TextAsset PlayerData;

	List<Room> rooms = new List<Room>();
	List<Room> bossRooms = new List<Room>();
	List<Room> specialRooms = new List<Room>();

	List<PlayerStats> playerProfiles = new List<PlayerStats>();

	/* Reference: http://unitynoobs.blogspot.co.uk/2011/02/xml-loading-data-from-xml-file.html
	 * Author: Rodrigo Barros 
	 * 
	 * 
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
			//Debug.Log(entities.Count);
			for(int j=0; j<entities.Count; j++){
				//Debug.Log (entities[j].Attributes["column"].Value);
				newRoom.setEntity (j, int.Parse(entities[j].Attributes["column"].Value), int.Parse(entities[j].Attributes["row"].Value), int.Parse(entities[j].Attributes["type"].Value));
			}

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
	 * 
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
//		profileXML.Save ("Assets/player-profiles.xml");
		profileXML.Save(Application.dataPath+"/player-profiles.xml");
	}

	public void loadPlayerProfiles(){
		XmlDocument profileXML = new XmlDocument ();
//		profileXML.LoadXml (PlayerData.text);
		Debug.Log(Application.dataPath);

		if (!File.Exists (Application.dataPath + "/player-profiles.xml")) {
			this.savePlayerProfiles ();
		}
		profileXML.Load (Application.dataPath+"/player-profiles.xml");
		XmlNodeList players = profileXML.GetElementsByTagName("player");
		for(int i=0; i<players.Count; i++){
			PlayerStats newPlayer = new PlayerStats ();
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
			//newPlayer.printStats ();
			playerProfiles.Add (newPlayer);
		}
	}

	public PlayerStats loadPlayer(int _playerID){
		if (_playerID < playerProfiles.Count) {
			return playerProfiles [_playerID];
		}
		return null;
	}

	public void savePlayer(PlayerStats _player, int _playerID){
		if (_playerID < 0 || _playerID >= playerProfiles.Count) {
			playerProfiles.Add (_player);
		} else {
			playerProfiles [_playerID] = _player;
		}
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

	public int checkRoomModifier(int _roomID, float _roomScore, int currentRoomMod){
		if(_roomID >=0 && _roomID < getNumberOfRooms()){
			return rooms [_roomID].checkRoomModifier (_roomScore, currentRoomMod);
		}
		else if(_roomID >=getNumberOfRooms() && _roomID < getNumberOfRooms()+getNumberOfBossRooms()){
			return bossRooms [_roomID].checkRoomModifier (_roomScore, currentRoomMod);
		} else{
			return specialRooms [_roomID].checkRoomModifier (_roomScore, currentRoomMod);
		}

	}
}