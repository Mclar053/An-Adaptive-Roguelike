using UnityEngine;
using System.Collections;

public abstract class movingObject : MonoBehaviour {
	
	public float speed, maxHitpoints, currentHitpoints, lastHit, hitDelay;
	
	virtual protected void Start () {
		GetComponent<Rigidbody2D> ().freezeRotation = true;
	}

	virtual protected void FixedUpdate () {
		
	}

	virtual protected void OnTriggerEnter2D(Collider2D other){
		
	}

	virtual protected void OnCollisionStay2D(Collision2D other){

	}

	virtual protected void OnCollisionEnter2D(Collision2D other){

	}

	public void damage(int _dmg){
		if (Time.time > lastHit + hitDelay) {
			currentHitpoints -= _dmg;
			lastHit = Time.time;
		}
	}

	public bool checkDead(){
		if (currentHitpoints <= 0) {
			return true;
		}
		return false;
	}

	public float getLastHit(){
		return lastHit;
	}
}
