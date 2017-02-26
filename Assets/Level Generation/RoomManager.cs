﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour {
	
	//Current Level
	private mTree levelTree;

	//List of room objects
	private List<Room> rooms = new List<Room>(); //Holds each room for each index
	private Transform roomHolder;
	public int currentRoom; //Current room selected

	void createLevel(int _level){
		levelTree = new mTree (_level);
		List<Node> levelNodes = levelTree.getNodes ();
		for (int i = 0; i < levelNodes.Count; i++) {
			Node currentNode = levelNodes[i];
			bool doorUp = false;
			bool doorDown = false;
			bool doorLeft = false;
			bool doorRight = false;

			//Check for child node doors
			if(currentNode.getChildren() != null){
				foreach(int _childIndex in currentNode.getChildren()){
					if(currentNode.getGridPosition().x == levelNodes[_childIndex].getGridPosition().x &&
						currentNode.getGridPosition().y+1 == levelNodes[_childIndex].getGridPosition().y){
						doorUp = true;
					}
					else if(currentNode.getGridPosition().x == levelNodes[_childIndex].getGridPosition().x &&
						currentNode.getGridPosition().y-1 == levelNodes[_childIndex].getGridPosition().y){
						doorDown = true;
					}
					else if(currentNode.getGridPosition().x+1 == levelNodes[_childIndex].getGridPosition().x &&
						currentNode.getGridPosition().y == levelNodes[_childIndex].getGridPosition().y){
						doorLeft = true;
					}
					else if(currentNode.getGridPosition().x-1 == levelNodes[_childIndex].getGridPosition().x &&
						currentNode.getGridPosition().y == levelNodes[_childIndex].getGridPosition().y){
						doorRight = true;
					}
				}
			}

			//Check for parent node door
			if(currentNode.getParent() != -1){
				if(currentNode.getGridPosition().x == levelNodes[currentNode.getParent()].getGridPosition().x &&
					currentNode.getGridPosition().y+1 == levelNodes[currentNode.getParent()].getGridPosition().y){
					doorUp = true;
				}
				else if(currentNode.getGridPosition().x == levelNodes[currentNode.getParent()].getGridPosition().x &&
					currentNode.getGridPosition().y-1 == levelNodes[currentNode.getParent()].getGridPosition().y){
					doorDown = true;
				}
				else if(currentNode.getGridPosition().x+1 == levelNodes[currentNode.getParent()].getGridPosition().x &&
					currentNode.getGridPosition().y == levelNodes[currentNode.getParent()].getGridPosition().y){
					doorLeft = true;
				}
				else if(currentNode.getGridPosition().x-1 == levelNodes[currentNode.getParent()].getGridPosition().x &&
					currentNode.getGridPosition().y == levelNodes[currentNode.getParent()].getGridPosition().y){
					doorRight = true;
				}
			}
			rooms.Add (gameObject.AddComponent(Room(i,doorUp,doorDown,doorLeft,doorRight)));
		}
	}

//	void roomSetup(){
//		roomHolder = new GameObject ("currentRoom").transform;
//
//		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
//		for(int x = -1; x < columns + 1; x++)
//		{
//			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
//			for(int y = -1; y < rows + 1; y++)
//			{
//				//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
//				GameObject toInstantiate = floorTile;
//
//				//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
//				if(x == -1 || x == columns || y == -1 || y == rows){
//					toInstantiate = wallTile;
//				}
//
//				//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
//				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
//
//				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
//				instance.transform.SetParent (roomHolder);
//			}
//		}
//	}

	public void SetupLevel(int _level){
		createLevel (_level);
		//roomSetup ();
	}

	void moveIntoRoom(int _old, int _new){

	}

}