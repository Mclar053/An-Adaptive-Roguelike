using UnityEngine;
using System.Collections;

public class Enemy_Shooter : Enemy {



	protected override void Start (){
		speed = 10;
		currentHitpoints = 10;
		dmg = 2;
		fireDelay = 4;
		shotSpeed = 7;
		range = 2;
		lastFired = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(fire()){
			Transform target = GameObject.FindGameObjectWithTag ("Player").transform;
			//Gets the movement vector for the player
			Vector2 directionVector = new Vector2(target.position.x - GetComponent<Rigidbody2D>().transform.position.x, target.position.y - GetComponent<Rigidbody2D>().transform.position.y);

			GameManager.instance.roomScript.createBullet (1, GetComponent<Rigidbody2D> ().position, Vector2.ClampMagnitude(directionVector,1), shotSpeed, dmg, range);
			lastFired = Time.time;
			fireDelay = (Vector2.SqrMagnitude (target.position - GetComponent<Rigidbody2D> ().transform.position)/10f)+0.7f;
		}

	}
}
