using UnityEngine;
using System.Collections;

public abstract class movingObject : MonoBehaviour {

	//Stats for all entities
	public float speed, maxHitpoints, currentHitpoints, hitDelay, dmg, fireDelay, shotSpeed, range;
	public float lastHit, lastFired; //Tracks when the entity has fired or got hit
	public int modifier; //Affects the health of the entity
	protected Color currentColour; //Tint of the sprite
	
	virtual protected void Start () {
		//Stop the physics engine spinning the entity
		GetComponent<Rigidbody2D> ().freezeRotation = true;
	}

	virtual protected void Update(){
		//If the entity has been hit change colour to red, else tint it the default colour
		if (Time.time < lastHit + 0.05f) {
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 0f, 0f, 1f);
		} else {
			gameObject.GetComponent<SpriteRenderer> ().color = currentColour;
		}
	}

	virtual protected void FixedUpdate () {
		
	}

	virtual protected void OnTriggerEnter2D(Collider2D other){

	}

	virtual protected void OnTriggerStay2D(Collider2D other){
		
	}

	virtual protected void OnCollisionStay2D(Collision2D other){

	}

	//Gives values to all the stats
	protected void setStats(float _speed,float _maxHitpoints,float _hitDelay,float _dmg,float _fireDelay,float _shotSpeed,float _range){
		speed = _speed;
		maxHitpoints = _maxHitpoints;
		currentHitpoints = _maxHitpoints;
		hitDelay = _hitDelay;
		dmg = _dmg;
		fireDelay = _fireDelay;
		shotSpeed = _shotSpeed;
		range = _range;
	}

	//Damages the current entity with the given amount of damage
	public void damage(float _dmg){
		if (Time.time > lastHit + hitDelay) {
			currentHitpoints -= _dmg;
			lastHit = Time.time;

			//Checks if the entity hit is the player
			if (gameObject.tag == "Player") {
				//Tracks the damage taken
				GameManager.instance.roomData.CurrentPlayer().playerDamaged (GameManager.instance.roomScript.currentRoom,_dmg);
				//Changes the health bar
				GameObject.FindGameObjectWithTag ("HealthBar").transform.localScale = new Vector2 (currentHitpoints/maxHitpoints,1);
				GameObject.FindGameObjectWithTag ("HealthBar").transform.localPosition = new Vector2 (-1*((1-currentHitpoints/maxHitpoints)/2),0);
				//If the player has died, change the game state
				if (checkDead ()) {
					GameManager.instance.changeState(GameStates.GameOver);
				}
			}
		}
	}

	//Heal the entity
	public void heal(float _hp){
		//Add the given hp to the current hitpoints
		currentHitpoints += _hp;
		//If the current is greater than the max, set to maximum hitpoints
		if (currentHitpoints > maxHitpoints) {
			currentHitpoints = maxHitpoints;
		}
		//Change the health bar
		GameObject.FindGameObjectWithTag ("HealthBar").transform.localScale = new Vector2 (currentHitpoints/maxHitpoints,1);
		GameObject.FindGameObjectWithTag ("HealthBar").transform.localPosition = new Vector2 (-1*((1-currentHitpoints/maxHitpoints)/2),0);
	}

	//Checks if the entity has less hitpoints than 0
	virtual public bool checkDead(){
		if (currentHitpoints <= 0) {
			return true;
		}
		return false;
	}
		
	public float getLastHit(){
		return lastHit;
	}

	//Checks if the player is able to fire based on when they last fired and their fire delay
	public bool fire(){
		if (Time.time > lastFired + fireDelay) {
			return true;
		}
		return false;
	}

	//Change the maximum number of hitpoints based on the modifier
	public void changeDifficulty(int _modifier){
		maxHitpoints += Mathf.Max (-0.6f * maxHitpoints, _modifier * 0.1f * maxHitpoints);
		currentHitpoints = maxHitpoints;
	}
}
