using UnityEngine;
using System.Collections;

// PlayerScript requires the GameObject to have a Rigidbody2D component

[RequireComponent (typeof (Rigidbody2D))]

public class PlayerMovement : movingObject {

	override protected void Start () {
		GetComponent<Rigidbody2D> ().freezeRotation = true;
		resetPlayer ();
		currentColour = new Color (1f, 1f, 1f, 1f);
	}

    /* Reference: http://answers.unity3d.com/questions/1233704/2d-top-down-movement-1.html
    * Author: Zoogyburger
    */
	override protected void FixedUpdate () {
		//Gets the movement vector for the player
		Vector2 movementVector = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"));
		//Adds a force to the player for the direction they are going
		//The movementvector does not exceed 1 meaning that diagonals are just as fast as moving horizontally or vertically
		GetComponent<Rigidbody2D>().AddForce (Vector3.ClampMagnitude(movementVector,1) * speed);
	}

	override protected void Update(){

		//Manages if the player can firing and checks what direction they are firing in
		if(fire()){
			bool makeBullet = false;
			Vector2 movementVector = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"));
			Vector2 clampedVector = Vector2.ClampMagnitude(movementVector,0.2f);
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
				GameManager.instance.roomScript.createBullet (0, GetComponent<Rigidbody2D> ().position, directionVector, shotSpeed, dmg, range);
				lastFired = Time.time;
			}

		}

		//Checks if player has been hit and display that to user
		if (Time.time < lastHit + hitDelay) {
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 0f, 0f, 1f);
		} else {
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
		}
	}

	override protected void OnTriggerEnter2D(Collider2D other){
		Vector2 currentGridPos = GameManager.instance.roomScript.currentGridPosition;
		if(other.tag == "DoorLeft"){
			transform.position = new Vector2 (13,4);
			GameManager.instance.changeRoom (currentGridPos.x - 1, currentGridPos.y);
		}
		if(other.tag == "DoorRight"){
			transform.position = new Vector2 (1,4);
			GameManager.instance.changeRoom (currentGridPos.x + 1, currentGridPos.y);
		}
		if(other.tag == "DoorTop"){
			transform.position = new Vector2 (7,1);
			GameManager.instance.changeRoom (currentGridPos.x, currentGridPos.y + 1);
		}
		if(other.tag == "DoorBottom"){
			transform.position = new Vector2 (7,7);
			GameManager.instance.changeRoom (currentGridPos.x, currentGridPos.y - 1);
		}
		if (other.tag == "NextFloor") {
			GameManager.instance.nextLevel ();
		}
	}

	public void resetPlayer(){
		maxHitpoints = 10;
		currentHitpoints = maxHitpoints;
		speed = 35;
		hitDelay = 0.5f;
		dmg = 3;
		fireDelay = 0.5f;
		shotSpeed = 6f;
		lastHit = 0f;
		lastFired = 0f;
		range = 1f;
		GetComponent<Rigidbody2D> ().position = new Vector2 (7, 4);
	}
}