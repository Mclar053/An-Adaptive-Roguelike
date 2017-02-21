using UnityEngine;
using System.Collections;

abstract public class GameObj : MonoBehaviour {

	protected Vector2 pos;
	protected Vector2 speed;
	public GameObject go;
	public Bounds boundingBox;

	protected GameObj(float x=0f, float y=0f){
		pos = new Vector2 (x,y);
		boundingBox = new Bounds(pos, go.transform.localScale);
	}

	public virtual void interactWith(GameObj _other){

	}

	public void setSpeed(float x, float y){
		speed = new Vector2 (x,y);
	}
}