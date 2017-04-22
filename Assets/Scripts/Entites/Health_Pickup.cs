using UnityEngine;
using System.Collections;

public class Health_Pickup : Pickup {

	override protected void Start () {
		value = Random.Range (1, 4);
		base.Start ();
	}

	override protected bool pickupAction(GameObject other){
		if (other.GetComponent<PlayerMovement> ().currentHitpoints < other.GetComponent<PlayerMovement> ().maxHitpoints) {
			other.GetComponent<PlayerMovement> ().heal (value);
			return true;
		}
		return false;
	}
}
