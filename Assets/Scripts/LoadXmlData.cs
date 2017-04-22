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

			//Debug.Log (rows.Count+" "+roomsList.Count);

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
					xmlToSave += "<room id='" + j + "'>";
						
					List<RoomStats> currentPlayedRoomStats = playerProfiles [i].getRoomInstances (j);
					for (int k = 0; k < currentPlayedRoomStats.Count; k++) {
						xmlToSave += "<stat id='" + k + "' startTime='" + currentPlayedRoomStats [k].startTime + "' endTime='" + currentPlayedRoomStats [k].endTime + "' firstEnemyTime='" + currentPlayedRoomStats [k].timeToKillFirstEnemy + "' hitTime='" + currentPlayedRoomStats [k].timeToGetHit + "' damageTaken='" + currentPlayedRoomStats [k].damageTakenInRoom + "'/>";
					}
					xmlToSave += "</room>";
				}
			}
			xmlToSave += "</roomStats></player>";
		}
		xmlToSave += "</AllPlayers>";
		profileXML.LoadXml (xmlToSave);
		profileXML.Save ("Assets/player-profiles.xml");
	}

	public void loadPlayerProfiles(){
		XmlDocument profileXML = new XmlDocument ();
		profileXML.LoadXml (PlayerData.text);
		XmlNodeList players = profileXML.GetElementsByTagName("player");
		for(int i=0; i<players.Count; i++){
			PlayerStats newPlayer = new PlayerStats ();
			newPlayer.resetPlayerStats ();
			newPlayer.userID = int.Parse(players [i].Attributes ["id"].Value);

			XmlNodeList playerRooms = players [i].ChildNodes[0].ChildNodes;
			for(int j=0; j<playerRooms.Count; j++){
				XmlNodeList roomInstances = playerRooms [j].ChildNodes;

				for(int k=0; k<roomInstances.Count; k++){
					RoomStats instance = new RoomStats ();
					instance.roomID = int.Parse(playerRooms[j].Attributes["id"].Value);
					instance.startTime = float.Parse(roomInstances[k].Attributes["startTime"].Value);
					instance.endTime = float.Parse(roomInstances[k].Attributes["endTime"].Value);
					instance.timeToKillFirstEnemy = float.Parse(roomInstances[k].Attributes["firstEnemyTime"].Value);
					instance.timeToGetHit = float.Parse(roomInstances[k].Attributes["hitTime"].Value);
					instance.damageTakenInRoom = float.Parse(roomInstances[k].Attributes["damageTaken"].Value);

					newPlayer.createInstance (instance);
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
}