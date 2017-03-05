using UnityEngine;
using System.Collections;

public class Enemy : movingObject {

	// Use this for initialization
	void Start () {
		speed = 10;
		currentHitpoints = 5;
		dmg = 2;
		fireDelay = 20;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Transform target = GameObject.FindGameObjectWithTag ("Player").transform;
		//Gets the movement vector for the player
		Vector2 movementVector = new Vector2(target.position.x - GetComponent<Rigidbody2D>().transform.position.x, target.position.y - GetComponent<Rigidbody2D>().transform.position.y);
		//Adds a force to the player for the direction they are going
		//The movementvector does not exceed 1 meaning that diagonals are just as fast as moving horizontally or vertically
		GetComponent<Rigidbody2D>().AddForce (Vector3.ClampMagnitude(movementVector,1) * speed);
	}

	void OnCollisionStay2D(Collision2D other){
		if(other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<movingObject> ().damage(2);
			if(other.gameObject.GetComponent<movingObject>().checkDead()){
				Debug.Log ("DEAD!");
			}
		}
	}
}
