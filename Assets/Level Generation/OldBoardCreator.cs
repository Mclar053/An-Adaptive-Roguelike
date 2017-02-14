using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
public class OldBoardCreator{

	//List of room, corridor and section objects
	List<Room> rooms;
	List<Corridor> corridors;
	List<Section> sections;

	//Constructor
	public OldBoardCreator(mTree _levelTree){
		//Create new board with all sections
		sections = new List<Section> ();
		createBoard (_levelTree);
	}

	//Create all sections for board
	public void createBoard(mTree _levelTree){
		sections = new List<Section> ();
		List<Node> nodes = _levelTree.getNodes (); //Get all tree nodes
		sections.Add (new Section(0,0,10,10,0,0)); //Add root section

		//Start recursive function 
		getSections (nodes, 0);

		//Print all sections
//		foreach (Section s in sections) {
//			s.printSection ();
//		}


	}

	//Recursive loop that creates all sections
	//Depth first through tree to create all sections for the tree
	private void getSections(List<Node> _nodes, int _sectionIndex){
		//Get the current section
		Section currentSection = sections [_sectionIndex];
		//Get current node index from the current section
		int currentIndex = currentSection.nodeIndex;
		//Get child node indices from the current node
		int[] childNodeIndices = _nodes [currentIndex].getChildren();

		//If the current selected node has child nodes
		if (childNodeIndices != null) {
			//Find how to split the current section
			bool splitHorizontal = _nodes[childNodeIndices[0]].splitHorizontalDirection();

			//Get the split % for the child nodes
			float splitA = _nodes[childNodeIndices[0]].getSpacePartition();
			float splitB = _nodes[childNodeIndices[1]].getSpacePartition();

			//Horizontal split
			if (splitHorizontal) {
				//Add both child sections to sections list
				sections.Add(horizontalSplit (currentSection, splitA, 0, childNodeIndices[0],_sectionIndex));
				sections.Add(horizontalSplit (currentSection, splitB, sections[sections.Count-1].size.y, childNodeIndices[1],_sectionIndex));
			}
			//Verticle split
			else {
				//Add both child sections to sections list
				sections.Add(verticleSplit (currentSection, splitA, 0, childNodeIndices[0],_sectionIndex));
				sections.Add(verticleSplit (currentSection, splitB, sections[sections.Count-1].size.x, childNodeIndices[1],_sectionIndex));
			}

			//Get the indices for the A and B
			int aIndex = sections.Count - 2;
			int bIndex = sections.Count - 1;

			//Recursive loop for both child nodes
			getSections (_nodes, aIndex);
			getSections (_nodes, bIndex);
		}
	}

	//Calculates new position and size for new section that is horizontally split
	private Section horizontalSplit(Section _original ,float _split, float _yOffset, int _nodeIndex, int _parentIndex){
		Vector2 newPos = new Vector2();
		Vector2 newSize = new Vector2 ();
		newSize.Set (_original.size.x, _original.size.y*_split);
		newPos.Set (_original.pos.x, (_original.pos.y) + _yOffset);
		return new Section (newPos, newSize, _nodeIndex, _parentIndex);
	}

	//Calculates new position and size for new section that is vertically split
	private Section verticleSplit(Section _original ,float _split, float _xOffset, int _nodeIndex, int _parentIndex){
		Vector2 newPos = new Vector2();
		Vector2 newSize = new Vector2 ();
		newSize.Set (_original.size.x*_split, _original.size.y);
		newPos.Set ((_original.pos.x) + _xOffset, _original.pos.y);
		return new Section (newPos, newSize, _nodeIndex, _parentIndex);
	}

	//Returns list of sections
	public List<Section> getSections(){
		return sections;
	}
}
*/