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
	public mTree(){
		levelNum = 2;
		nodes = new List<Node>();
		nodes.Add(new Node(-1,1f));
		createTree();
	}

	//Same as default constructor but tree size is different
	public mTree(int _levelNum){
		levelNum = _levelNum;
		nodes = new List<Node>();
		nodes.Add(new Node(-1,1f));
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
		//EDITED: Checks if the current node creates a set of 2 child nodes
		//If the number produced between 0-1 is greater than a number based on the formula in ''num then it creates the child nodes
		//*_*_*_*_*_*_*_*
		float num = 1f-(1f+(levelNum/10f))/(currentLevel+0.01f);
		float val = Random.value;
		currentLevel++;
		if(num < val){
			//Gets the current node from node arraylist
			Node currentNode = nodes[_nodeNum];

			//*_*_*_*_*_*_*_*
			//EDITED: Sets the children array length to 2 as this creates binary trees
			currentNode.setChildrenArray(2);
			//*_*_*_*_*_*_*_*

			//*_*_*_*_*_*_*_*
			//ADDED: Random space partition % for the child node when it is generated and direction of partition
			float spacePart = Random.Range(300,700)/1000f;
			bool horizontalPart;

			//Checks the direction the child nodes would split in
			if (currentNode.splitHorizontalDirection()) {
				horizontalPart = false;
			} else {
				horizontalPart = true;
			}
			//*_*_*_*_*_*_*_*


			//For each child node
			for(int i=0; i<currentNode.getChildren().Length; i++){
				//Create a new node in the nodes arraylist
				nodes.Add(new Node(_nodeNum,spacePart,horizontalPart));
				//Set the child node's index in the current node
				currentNode.setChildNode(i,nodes.Count-1);
				//Create child nodes for the newly create child node
				createNodes(nodes.Count-1);

				//ADDED: For the other child node, find remaining space partition %
				spacePart = 1 - spacePart;
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
			Debug.Log(i+"----"+n.getParent()+"----"+n.getRoomID()+"----"+n.getSpacePartition()+"----"+n.splitHorizontalDirection());
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
