using UnityEngine;
using System.Collections;

public class Score_Pickup : Pickup {

	override protected void Start () {
		value = Random.Range (1, 4);
		base.Start ();
	}

	override protected bool pickupAction(GameObject other){
		GameManager.instance.scoreManager.addScore (value);
		return true;
	}
}
