using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardCreator{

	//List of room, corridor and section objects
	List<Room> rooms;
	List<Corridor> corridors;
	List<Section> sections;

	//Constructor
	public BoardCreator(mTree _levelTree){
		//Create new board with all sections
		sections = new List<Section> ();
		createBoard (_levelTree);
	}

	//Create all sections for board
	public void createBoard(mTree _levelTree){

	}

	//Recursive loop that creates all sections
	//Depth first through tree to create all sections for the tree
	private void getSections(List<Node> _nodes, int _sectionIndex){
		
	}

	//Returns list of sections
	public List<Section> getSections(){
		return sections;
	}
}