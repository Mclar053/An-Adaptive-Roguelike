using UnityEngine;
using System.Collections;

public abstract class movingObject : MonoBehaviour {
	
	public float speed, maxHitpoints, currentHitpoints, hitDelay, dmg, fireDelay, shotSpeed, range;
	protected float lastHit, lastFired;
	
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

	public void damage(float _dmg){
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

	public bool fire(){
		if (Time.time > lastFired + fireDelay) {
			lastFired = Time.time;
			return true;
		}
		return false;
	}
}
