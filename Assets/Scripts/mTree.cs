using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Tree class adapted from Natural Computing coursework two by Matthew Clark
//Link: https://github.com/Mclar053/genetic_trees/blob/master/Tree.pde
public class mTree{

	//---------------PROPERTIES
	private List<Node> nodes;

	//Only used in initial creation of tree
	private int levelNum;
	private int currentLevel = 0;

	//---------------CONSTRUCTORS
	//Default constructor
	public mTree(int _levelNum=2){
		levelNum = _levelNum;
		nodes = new List<Node>();
		nodes.Add(new Node());
		createTree();
	}

	//Copy constructor
	public mTree(List<Node> _nodes){
		nodes = new List<Node>();
		foreach(Node n in _nodes){
			nodes.Add(new Node(n));
		}
	}

	//---------------METHODS

	//Create Tree
	void createTree(){
		//Create tree from root node
		createNodes(0);
	}

	//------CREATE NEW TREE
	//Recursive to create nodes for tree
	void createNodes(int _nodeNum){
		//*_*_*_*_*_*_*_*
		//EDITED: Checks if the current node creates a set of child nodes
		//If the number produced between 0-1 is greater than a number based on the formula in ''num then it creates the child nodes
		//*_*_*_*_*_*_*_*
		float num = 1f-(1f+(levelNum/10f))/(currentLevel+0.01f);
		float val = Random.value;
		currentLevel++;
		if(num < val){
			//Gets the current node from node arraylist
			Node currentNode = nodes[_nodeNum];

			int maxNodes = Random.Range (1,3);
			List<Vector2> directions;

			for (int i = 0; i < maxNodes; i++) {
				//Checking direction of node for each room
				//0=up, 1=down, 2=left, 3=right
				int randDirection = Random.Range (0,3);
				Vector2 selectedDirection = new Vector2 ();

				//Up
				if (randDirection == 0) {
					selectedDirection = new Vector2 (currentNode.getGridPosition().x,currentNode.getGridPosition().y+1);
				}
				//Down
				else if (randDirection == 1) {
					selectedDirection = new Vector2 (currentNode.getGridPosition().x,currentNode.getGridPosition().y-1);
				}
				//Left
				else if (randDirection == 2) {
					selectedDirection = new Vector2 (currentNode.getGridPosition().x-1,currentNode.getGridPosition().y);
				}
				//Right
				else {
					selectedDirection = new Vector2 (currentNode.getGridPosition().x+1,currentNode.getGridPosition().y);
				}

				if (checkNodeGridPosition (selectedDirection) == -1) {
					directions.Add (selectedDirection);
				}

			}

			//*_*_*_*_*_*_*_*
			//EDITED: Sets the children array length to the number of rooms that can be generated
			currentNode.setChildrenArray(directions.Count);
			//*_*_*_*_*_*_*_*

			//Stores all the child node numbers when they have been added to the nodes List
			//This allows these rooms to be generated first and so that they are not overwritten later in the process
			List<int> childNodeNumbers;

			//For each child node
			for(int i=0; i<currentNode.getChildren().Length; i++){
				//Create a new node in the nodes arraylist
				nodes.Add(new Node(_nodeNum,directions[i].x,directions[i].y));
				//Set the child node's index in the current node
				currentNode.setChildNode(i,nodes.Count-1);
				childNodeNumbers.Add (nodes.Count-1);
			}

			foreach(int _num in childNodeNumbers){
				//Create child nodes for the newly create child node
				createNodes(_num);
			}
			//Save the current node in the arraylist
			nodes.Insert(_nodeNum,currentNode);
			nodes.RemoveAt(_nodeNum+1);
		}
		//Move out of a level in the tree
		currentLevel--;
	}

	//Returns position of found number in array. If not found, return -1
	private int searchIntArray(int[] _array, int _num){
		for(int i=0; i<_array.Length; i++){
			if(_array[i]==_num)
				return i;
		}
		return -1;
	}

	//Gets the current counter for an int array
	//It counts the integers that are not equal to -1
	//E.g [-1,-1,-1,0,1,2,-1,3,4] would return [5]
	private int getCurrentCount(int[] _subIndex){
		int sum = 0;
		foreach(int i in _subIndex){
			if(i!=-1){
				sum++;
			}
		}
		return sum;
	}

	/*ADDED: Checks if the nodes grid position is within the current nodes system
	Returns position is nodes array if found, returns -1 if not found*/
	private int checkNodeGridPosition(Vector2 _pos){
		for(int i=0; i<this.getSize(); i++){
			//Checks if the position that has been passed into the function is equal to any of the nodes grid positions
			if(nodes[i].getGridPosition().Equals(_pos)){
				//If found return the index where found at
				return i;
			}
		}
		//If not found, return -1 to indicate that the position has not been found
		return -1;
	}

	//---------------GETTERS
	public Node getParentNode(int _n){
		if(_n<nodes.Count){
			return nodes[nodes[_n].getParent()];
		}
		return null;
	}

	public Node getNode(int _n){
		if(_n<nodes.Count){
			return nodes[_n];
		}
		return null;
	}

	public List<Node> getNodes(){
		List<Node> n = new List<Node>();
		foreach(Node _n in nodes){
			n.Add(new Node(_n));
		}
		return n;
	}

	public int getSize(){
		return nodes.Count;
	}


	//PRINT ALL NODES
	public void printNodes(){
		Debug.Log(nodes.Count);
		for(int i=0; i<nodes.Count; i++){
			Node n = nodes[i];
			int[] c = n.getChildren();
			Debug.Log(i+"----"+n.getParent()+"----"+n.getRoomID());
			if(c!=null){
				foreach(int _c in c) Debug.Log(" " + _c);
			}
			Debug.Log("\n\n");
		}
	}

	//Returns all parent pointers for all nodes of current tree
	public int[] getParents(){
		int[] p = new int[nodes.Count];
		for(int i=0; i<p.Length; i++){
			p[i] = nodes[i].getParent();
		}
		return p;
	}

	//Gets all child node pointers for all nodes of current tree
	public int[,] getChildren(){
		int[,] c = new int[nodes.Count,3];
		for(int i=0; i<c.Length; i++){
			Node cn = nodes[i];
			if(cn.getChildren() != null){
				for(int j=0; j<cn.getChildren().Length; j++){
					c[i, j] = -2;
					c[i, j] = cn.getChildren()[j];
				}
			}
		}
		return c;
	}

	//Returns a new copy of this tree
	public mTree copyTree(){
		return new mTree(getNodes());
	}
}
