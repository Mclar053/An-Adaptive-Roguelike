using UnityEngine;
using System.Collections;

public class Enemy_Ghost : Enemy {

	protected override void Start (){
		speed = 10;
		currentHitpoints = 10;
		dmg = 2;
	}

}
