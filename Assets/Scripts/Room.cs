using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public Vector2 pos;
	protected Vector2 size;
	public int roomID;
	private BoundingBox roomBounds;

	//Room Constructor
	public Room(float _x, float _y, int _roomID=-1){
		this.pos.Set (_x,_y);
		this.size.Set (60f,60f);
		this.roomID = _roomID;
		this.roomBounds = new BoundingBox ((int)pos.x, (int)pos.y, (int)size.x+20, (int)size.y+20);
	}

	//Checks if one room is in another room
	public bool isInRoom(Room _other){
		if (_other.getBoundingBox ().isInside (this.roomBounds)) {
			return true;
		}
		return false;
	}

	//Get bounding box of the room
	public BoundingBox getBoundingBox(){
		return roomBounds;
	}
}
