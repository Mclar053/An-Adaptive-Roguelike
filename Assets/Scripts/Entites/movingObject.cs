using UnityEngine;
using System.Collections;

public abstract class movingObject : MonoBehaviour {
	
	public float speed, maxHitpoints, currentHitpoints, hitDelay, dmg, fireDelay, shotSpeed, range;
	public float lastHit, lastFired;
	
	virtual protected void Start () {
		GetComponent<Rigidbody2D> ().freezeRotation = true;
	}

	virtual protected void FixedUpdate () {
		
	}

	virtual protected void OnTriggerEnter2D(Collider2D other){

	}

	virtual protected void OnTriggerStay2D(Collider2D other){
		
	}

	virtual protected void OnCollisionStay2D(Collision2D other){

	}

	protected void setStats(float _speed,float _maxHitpoints,float _hitDelay,float _dmg,float _fireDelay,float _shotSpeed,float _range){
		speed = _speed;
		maxHitpoints = _maxHitpoints;
		currentHitpoints = maxHitpoints;
		hitDelay = _hitDelay;
		dmg = _dmg;
		fireDelay = _fireDelay;
		shotSpeed = _shotSpeed;
		range = _range;
	}

	public void damage(float _dmg){
		if (Time.time > lastHit + hitDelay) {
			currentHitpoints -= _dmg;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (1,currentHitpoints/maxHitpoints+0.3f,currentHitpoints/maxHitpoints+0.3f,1f);
			lastHit = Time.time;
		}
	}

	virtual public bool checkDead(){
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
			return true;
		}
		return false;
	}
}
