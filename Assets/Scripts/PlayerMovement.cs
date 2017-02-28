using UnityEngine;
using System.Collections;

// PlayerScript requires the GameObject to have a Rigidbody2D component

[RequireComponent (typeof (Rigidbody2D))]

public class PlayerMovement : MonoBehaviour {


	public float playerSpeed = 4f;

	void Start () {

	}

	void FixedUpdate () {
//		Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
//		GetComponent<Rigidbody2D>().velocity=targetVelocity * playerSpeed;
		Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		//GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + movement_vector * Time.deltaTime * playerSpeed);

		GetComponent<Rigidbody2D> ().AddForce (Vector3.ClampMagnitude(movementVector,1) * playerSpeed);
		//GetComponent<Rigidbody2D>().velocity = Vector3.ClampMagnitude(movementVector,1) * playerSpeed;
	}
}