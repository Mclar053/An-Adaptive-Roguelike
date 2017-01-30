using UnityEngine;
using System.Collections;

//Node class adapted from Natural Computing coursework two by Matthew Clark
//Link: https://github.com/Mclar053/genetic_trees/blob/master/Node.pde
public class Node{

	private int parent;
	private int[] children = null;
	private int roomID;
	private float spacePartition;
	private bool horizontalDirection;

	//Default constructor
	//Sets parent to arguement (-1 as default) and room id to random integer between 0 and 50
	//Sets space partition to arguement (0.5f is not given)
	public Node(int _parent=-1, float _spacePartition=0.5f, bool _horizontal=false){
		parent = _parent;
		roomID = Random.Range(0,50);
		spacePartition = _spacePartition;
		horizontalDirection = _horizontal;
	}

	//Constructor that takes parent and room id arguements
	//Sets parent and room id to arguements
	public Node(int _parent, int _id, float _spacePartition=0.5f, bool _horizontal=false){
		parent = _parent;
		roomID = _id;
		spacePartition = _spacePartition;
		horizontalDirection = _horizontal;
	}

	//Copy constructor
	//Copies all elements of _n to the current node
	public Node(Node _n){
		parent = _n.getParent();
		setChildren(_n.getChildren());
		roomID = _n.getRoomID();
		spacePartition = _n.getSpacePartition();
		horizontalDirection = _n.splitHorizontalDirection();
	}

	//parent setter
	public void setParent(int _parent){
		parent = _parent;
	}

	//Sets children array length
	public void setChildrenArray(int _children){
		children = new int[_children];
	}

	//Set specific child node to an integer
	public void setChildNode(int _child, int _childNode){
		children[_child] = _childNode;
	}

	//Copies arguement to this.children
	public void setChildren(int[] _children){
		if(_children != null){
			children = new int[_children.Length];
			for(int i=0; i<children.Length; i++){
				children[i] = _children[i];
			}
		} else{
			children = null;
		}
	}

	//Sets the room id
	public void setRoomID(int _id){
		roomID = _id;
	}

	//Gets parent of this
	public int getParent(){
		return parent;
	}

	//Gets children int array of this
	public int[] getChildren(){
		if(children != null){
			int[] _c = new int[children.Length];
			for(int i=0; i<_c.Length; i++){
				_c[i] = children[i];
			}
			return _c;
		}
		return null;
	}

	//Get room if of this
	public int getRoomID(){
		return roomID;
	}

	public float getSpacePartition(){
		return spacePartition;
	}

	public bool splitHorizontalDirection(){
		return horizontalDirection;
	}

	//Removes a child node from a nodes childNodes int array
	public void removeChildNode(int _pos){
		if(children.Length-1!=0){
			int[] childNodes = new int[children.Length-1];
			int counter = 0;
			for(int i=0; i<children.Length; i++){
				if(i!=_pos)
					childNodes[counter++] = children[i];
			}
			setChildren(childNodes);
		}
		else{
			setChildren(null);
		}
	}

	//Adds a child node to a nodes childNodes int array
	public void addChildNode(int _value){
		int[] childNodes;
		if(children == null){
			childNodes = new int[1];
		}
		else{
			childNodes = new int[children.Length+1];
			for(int i=0; i<children.Length; i++){
				childNodes[i] = children[i];
			}
		}
		childNodes[childNodes.Length-1] = _value;
		setChildren(childNodes);
	}
}
