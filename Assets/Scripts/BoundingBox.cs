using UnityEngine;
using System.Collections;

public class BoundingBox : MonoBehaviour {

	//Points[0]=Bottom Left, Points[1]=Bottom Right, Points[2]=Top Right, Points[3]=Top Left
	private int[,] points = new int[4,2];

	//Default BoundingBox constructor
	public BoundingBox(int _x=0, int _y=0, int _w=10, int _h=10){
		this.points[0,0] = _x-(_w/2);
		this.points[0,1] = _y-(_h/2);
		this.points[1,0] = _x+(_w/2);
		this.points[1,1] = _y-(_h/2);
		this.points[2,0] = _x+(_w/2);
		this.points[2,1] = _y+(_h/2);
		this.points[3,0] = _x-(_w/2);
		this.points[3,1] = _y+(_h/2);
	}

	//BoundingBox constructor taking Vector2 parameters than integers
	public BoundingBox(Vector2 _pos, Vector2 _size){
		this.points[0,0] = (int)(_pos.x-(_size.x/2));
		this.points[0,1] = (int)(_pos.y-(_size.y/2));
		this.points[1,0] = (int)(_pos.x+(_size.x/2));
		this.points[1,1] = (int)(_pos.y-(_size.y/2));
		this.points[2,0] = (int)(_pos.x+(_size.x/2));
		this.points[2,1] = (int)(_pos.y+(_size.y/2));
		this.points[3,0] = (int)(_pos.x-(_size.x/2));
		this.points[3,1] = (int)(_pos.y+(_size.y/2));
	}

	//Return center position of bounding box
	public Vector2 getPos(){
		return new Vector2 ((points[0,0]-points[1,0])/2f,(points[0,1]-points[2,1])/2f);
	}

	//Return size of bounding box
	public Vector2 getSize(){
		return new Vector2 ((points[0,0]-points[1,0]),(points[0,1]-points[2,1]));
	}

	//Checks if another bounding box is inside the current bounding box
	public bool isInside(BoundingBox _other){
		if(checkPoint(points[0,0],points[0,1], _other.getPos(), _other.getSize()) ||
			checkPoint(points[1,0], points[1,1], _other.getPos(), _other.getSize()) ||
			checkPoint(points[2,0], points[2,1], _other.getPos(), _other.getSize()) ||
			checkPoint(points[3,0], points[3,1], _other.getPos(), _other.getSize()) ){
			return true;
		}
		return false;
	}

	//Checks if a point (with x and y) of current bounding box is inside the another bounding box
	private bool checkPoint(int _x, int _y, Vector2 _otherPos, Vector2 _otherSize){
		if(_x > _otherPos.x - _otherSize.x/2 &&
			_x < _otherPos.x + _otherSize.x/2 &&
			_y > _otherPos.y - _otherSize.y/2 &&
			_y < _otherPos.y + _otherSize.y/2){
			return true;
		}
		return false;
	}
}
