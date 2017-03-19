using UnityEngine;
using System.Collections;

public class Room {

	public int[,] layout;
	public int[,] entities; //[0]-ID [1]-X  [2]-Y

	public Room(int _w, int _h, int _entityNum){
		layout = new int[_w,_h];
		entities = new int[_entityNum,3];
	}

	public void setEntity(int _pos, int _x, int _y, int _id){
		entities [_pos, 0] = _id-2;
		entities [_pos, 1] = _x;
		entities [_pos, 2] = _y;
	}
}
