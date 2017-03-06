using UnityEngine;
using System.Collections;

public class Enemy_Slider : Enemy {

	private float lastHitBlock = 0;
	public Vector2 direction;

	// Use this for initialization
	void Start () {
		speed = 20;
		currentHitpoints = 5;
		dmg = 2;
		fireDelay = 20;
		direction = new Vector2 (0,-1);
	}

	void FixedUpdate () {
		//Adds a force to the player for the direction they are going
		//The movementvector does not exceed 1 meaning that diagonals are just as fast as moving horizontally or vertically
		GetComponent<Rigidbody2D>().AddForce (Vector3.ClampMagnitude(direction,1) * speed);
	}

	void OnCollisionStay2D(Collision2D other){
		if(other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<movingObject> ().damage(dmg);
			if(other.gameObject.GetComponent<movingObject>().checkDead()){
				Debug.Log ("DEAD!");
			}
		}
		if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Gap" || other.gameObject.tag == "Enemy") {
			changeDirection ();
		}
	}

	void changeDirection(){
		if(direction.x == 0 && direction.y == -1){
			direction = new Vector2 (1,0);
		} else if(direction.x == 1 && direction.y == 0){
			direction = new Vector2 (0,1);
		} else if(direction.x == 0 && direction.y == 1){
			direction = new Vector2 (-1,0);
		} else if(direction.x == -1 && direction.y == 0){
			direction = new Vector2 (0,-1);
		}
	}
}
