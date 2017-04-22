using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]

public abstract class Pickup : movingObject {

	override protected void Start () {
		GetComponent<Rigidbody2D> ().freezeRotation = true;
	}


	override protected void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			pickupAction (other.gameObject);
		}
	}

	virtual protected void pickupAction(GameObject other){

	}

}
