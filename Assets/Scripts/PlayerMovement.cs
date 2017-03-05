using UnityEngine;
using System.Collections;

// PlayerScript requires the GameObject to have a Rigidbody2D component

[RequireComponent (typeof (Rigidbody2D))]

public class PlayerMovement : movingObject {

	void Start () {
		GetComponent<Rigidbody2D> ().freezeRotation = true;
		currentHitpoints = 20;
		speed = 10;
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
			bool makeBullet = false;
			Vector2 movementVector = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"));
			Vector2 clampedVector = Vector2.ClampMagnitude(movementVector,0.4f);
			Vector2 directionVector = new Vector2 ();

			if(Input.GetKey(KeyCode.DownArrow)){
				directionVector = new Vector2 (0, -1) + clampedVector;
				makeBullet = true;
			}
			else if(Input.GetKey(KeyCode.UpArrow)){
				directionVector = new Vector2 (0, 1) + clampedVector;
				makeBullet = true;
			}
			else if(Input.GetKey(KeyCode.LeftArrow)){
				directionVector = new Vector2 (-1, 0) + clampedVector;
				makeBullet = true;
			}
			else if(Input.GetKey(KeyCode.RightArrow)){
				directionVector = new Vector2 (1, 0) + clampedVector;
				makeBullet = true;

			}

			if (makeBullet) {
				GameManager.instance.roomScript.createBullet (GetComponent<Rigidbody2D> ().position, directionVector);
				lastFired = Time.time;
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