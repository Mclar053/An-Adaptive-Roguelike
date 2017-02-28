using UnityEngine;
using System.Collections;

// PlayerScript requires the GameObject to have a Rigidbody2D component

[RequireComponent (typeof (Rigidbody2D))]

public class PlayerMovement : MonoBehaviour {


	public float playerSpeed = 4f;

	void Start () {
		GetComponent<Rigidbody2D> ().freezeRotation = true;
	}

	void FixedUpdate () {
		//Gets the movement vector for the player
		Vector2 movementVector = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"));
		//Adds a force to the player for the direction they are going
		//The movementvector does not exceed 1 meaning that diagonals are just as fast as moving horizontally or vertically
		GetComponent<Rigidbody2D>().AddForce (Vector3.ClampMagnitude(movementVector,1) * playerSpeed);
	}
}