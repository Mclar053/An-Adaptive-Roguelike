using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]

public abstract class Pickup : movingObject {

	protected float value;

	override protected void Start () {
		GetComponent<Rigidbody2D> ().freezeRotation = true;
	}

	public void setValue(int _s){
		value = _s;
	}

	override protected void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			if (pickupAction (other.gameObject)) {
				Destroy (this.gameObject);
			}
		}
	}

	virtual protected bool pickupAction(GameObject other){return false;}

}
