using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class LoadXmlData : MonoBehaviour{ // the Class
	public TextAsset GameAsset;

	List<Room> rooms = new List<Room>();

	public void loadRooms(){
		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
		xmlDoc.LoadXml(GameAsset.text); // load the file.
		XmlNodeList roomsList = xmlDoc.GetElementsByTagName("room"); // array of the room nodes.

		for (int i=0; i<roomsList.Count; i++){
			XmlNodeList rows = roomsList[i].ChildNodes[0].ChildNodes;
			XmlNodeList entities = roomsList[i].ChildNodes[1].ChildNodes;
			Room newRoom = new Room (15,9,entities.Count);

			Debug.Log (rows.Count+" "+roomsList.Count);

			//Room Layout
			for (int j=0; j<rows.Count; j++){ // levels itens nodes.
				XmlNodeList columns = rows [j].ChildNodes;
				for (int k=0; k<columns.Count; k++){ // levels itens nodes.
					newRoom.layout[j,k] = int.Parse(columns[k].InnerText);
				}
			}

			//Entity Positions
			Debug.Log(entities.Count);
			for(int j=0; j<entities.Count; j++){
				Debug.Log (entities[j].Attributes["column"].Value);
				newRoom.setEntity (j, int.Parse(entities[j].Attributes["column"].Value), int.Parse(entities[j].Attributes["row"].Value), int.Parse(entities[j].Attributes["type"].Value));
			}

			rooms.Add(newRoom); 
		}
	}

	public int[,] getRoomLayout(int _room){
		Debug.Log (_room);
		return rooms[_room].layout;
	}

	public int[,] getRoomEntities(int _room){
		return rooms[_room].entities;
	}
}