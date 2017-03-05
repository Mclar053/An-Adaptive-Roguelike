using UnityEngine;
using System.Collections;

// PlayerScript requires the GameObject to have a Rigidbody2D component

[RequireComponent (typeof (Rigidbody2D))]

public class PlayerMovement : movingObject {

	void Start () {
		GetComponent<Rigidbody2D> ().freezeRotation = true;
		currentHitpoints = 20;
		speed = 20;
		hitDelay = 0.5f;
		dmg = 3;
		fireDelay = 0.5f;
		shotSpeed = 6f;
		lastHit = 0f;
		lastFired = 0f;
		range = 1f;
	}

	void FixedUpdate () {
		//Gets the movement vector for the player
		Vector2 movementVector = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"));
		//Adds a force to the player for the direction they are going
		//The movementvector does not exceed 1 meaning that diagonals are just as fast as moving horizontally or vertically
		GetComponent<Rigidbody2D>().AddForce (Vector3.ClampMagnitude(movementVector,1) * speed);
	}

	void Update(){
		if(fire()){
			if(Input.GetKey(KeyCode.DownArrow)){
				GameManager.instance.roomScript.createBullet (GetComponent<Rigidbody2D> ().position, 0, -1);
			}
			else if(Input.GetKey(KeyCode.UpArrow)){
				GameManager.instance.roomScript.createBullet (GetComponent<Rigidbody2D> ().position, 0, 1);
			}
			else if(Input.GetKey(KeyCode.LeftArrow)){
				GameManager.instance.roomScript.createBullet (GetComponent<Rigidbody2D> ().position, -1, 0);
			}
			else if(Input.GetKey(KeyCode.RightArrow)){
				GameManager.instance.roomScript.createBullet (GetComponent<Rigidbody2D> ().position, 1, 0);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		Vector2 currentGridPos = GameManager.instance.roomScript.currentGridPosition;
		if(other.tag == "DoorLeft"){
			transform.position = new Vector2 (12,3);
			GameManager.instance.roomScript.changeRoom (currentGridPos.x - 1, currentGridPos.y);
		}
		if(other.tag == "DoorRight"){
			transform.position = new Vector2 (0,3);
			GameManager.instance.roomScript.changeRoom (currentGridPos.x + 1, currentGridPos.y);
		}
		if(other.tag == "DoorTop"){
			transform.position = new Vector2 (6,0);
			GameManager.instance.roomScript.changeRoom (currentGridPos.x, currentGridPos.y + 1);
		}
		if(other.tag == "DoorBottom"){
			transform.position = new Vector2 (6,6);
			GameManager.instance.roomScript.changeRoom (currentGridPos.x, currentGridPos.y - 1);
		}
	}
}