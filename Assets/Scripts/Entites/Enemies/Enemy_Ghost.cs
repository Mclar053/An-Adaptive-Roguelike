using UnityEngine;
using System.Collections;

public class Enemy_Ghost : Enemy {

	override protected void Start (){
		GetComponent<Rigidbody2D> ().freezeRotation = true;
		speed = 7;
		currentHitpoints = 10;
		dmg = 2;
	}

}
