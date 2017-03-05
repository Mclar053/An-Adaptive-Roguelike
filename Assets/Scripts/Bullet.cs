using UnityEngine;
using System.Collections;

public class Bullet : movingObject {

	public Vector2 direction;
	private float timeCreated;


	// Use this for initialization
	void Start () {
		timeCreated = Time.time;
		speed = GameObject.FindGameObjectWithTag ("Player").GetComponent<movingObject> ().shotSpeed;
		dmg = GameObject.FindGameObjectWithTag ("Player").GetComponent<movingObject> ().dmg;
		range = GameObject.FindGameObjectWithTag ("Player").GetComponent<movingObject> ().range;
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 movementVector = direction;
		//Adds a force to the player for the direction they are going
		//The movementvector does not exceed 1 meaning that diagonals are just as fast as moving horizontally or vertically
		GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(movementVector,1) * speed;
		checkDead ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Enemy"){
			other.gameObject.GetComponent<movingObject> ().damage(dmg);
			if(other.gameObject.GetComponent<movingObject>().checkDead()){
				Destroy (other.gameObject);
			}
		}

		if (other.gameObject.tag != "Player" && other.gameObject.tag != "Untagged") {
			Destroy (this.gameObject);
		}
	}

	public void changeDirection(float _x, float _y){
		direction = new Vector2 (_x,_y);
	}

	void checkDead(){
		if(Time.time > timeCreated + range){
			Destroy (this.gameObject);
		}
	}
}
