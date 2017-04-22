using UnityEngine;
using System.Collections;

public class Bullet : movingObject {

	public Vector2 direction;
	protected float timeCreated;


	// Use this for initialization
	override protected void Start () {
		timeCreated = Time.time;
	}

	// Update is called once per frame
	override protected void FixedUpdate () {
		Vector2 movementVector = direction;
		//Adds a force to the player for the direction they are going
		//The movementvector does not exceed 1 meaning that diagonals are just as fast as moving horizontally or vertically
		GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(movementVector,1) * speed;
		checkTimer ();
	}

	override protected void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Enemy"){
			other.gameObject.GetComponent<movingObject> ().damage(dmg);
			if(other.gameObject.GetComponent<movingObject>().checkDead()){
				GameManager.instance.statistics.firstEnemyKilled (GameManager.instance.roomScript.currentRoom);
				Destroy (other.gameObject);
			}
		}

		if (other.gameObject.tag != "Player" && other.gameObject.tag != "Gap" && other.gameObject.tag != "Projectile" && other.gameObject.tag != "Pickup" && other.gameObject.tag != "NextFloor") {
			Destroy (this.gameObject);
		}
	}

	public void changeDirection(Vector2 _dir){
		direction = _dir;
	}

	bool checkTimer(){
		if(Time.time > timeCreated + range){
			Destroy (this.gameObject);
			return true;
		}
		return false;
	}

	public void setBulletStats(float _speed, float _dmg, float _range){
		speed = _speed;
		dmg = _dmg;
		range = _range;
	}
}
