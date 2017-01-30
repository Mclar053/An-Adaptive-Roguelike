using UnityEngine;
using System.Collections;

//Inherit from Room
public class Corridor : Room {

	//0=North-South, 1=East-West
	int direction;

	//Constructor for corridor
	public Corridor(float _x, float _y, int _direction): base(_x,_y){
		direction = _direction;
		size.Set (30f,100f);
	}
}
