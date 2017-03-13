using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class LoadXmlData : MonoBehaviour{ // the Class
	public TextAsset GameAsset;

	static string Cube_Character = "";
	static string Cylinder_Character = "";
	static string Capsule_Character = "";
	static string Sphere_Character = "";

	List<int[,]> rooms = new List<int[,]>();

	public void loadRooms(){
		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
		xmlDoc.LoadXml(GameAsset.text); // load the file.
		XmlNodeList roomsList = xmlDoc.GetElementsByTagName("room"); // array of the level nodes.

		for (int i=0; i<roomsList.Count; i++){
			int[,] roomLayout = new int[15,9];
			XmlNodeList rows = roomsList [i].ChildNodes;

			for (int j=0; j<rows.Count; j++){ // levels itens nodes.
				XmlNodeList columns = rows [j].ChildNodes;
				for (int k=0; k<columns.Count; k++){ // levels itens nodes.
					roomLayout[j,k] = int.Parse(columns[k].InnerText);
				}
			}
			rooms.Add(roomLayout); 
		}
	}

	public int[,] getRoom(int _room){
		return rooms[_room];
	}
}