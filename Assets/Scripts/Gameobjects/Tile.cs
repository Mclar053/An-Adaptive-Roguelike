using UnityEngine;
using System.Collections;

abstract public class Tile : GameObj {

	protected Tile(float x=0, float y=0) : base(x,y){
		
	}
}

public class normalTile : Tile{

	public normalTile(float x=0, float y=0): base(x,y){

	}
}

public class gapTile : Tile{

	public gapTile(float x=0, float y=0): base(x,y){
//		go = Instantiate ();
	}

	public virtual void interactWith(GameObj _other){
		if(_other.boundingBox.Intersects(this.boundingBox)){
			_other.setSpeed(0,0);
		}
	}
}