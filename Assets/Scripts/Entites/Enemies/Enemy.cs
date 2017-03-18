using UnityEngine;
using System.Collections;

public class Enemy<A> : movingObject {

	public StateMachine<A> fsm;

	// Use this for initialization
	override protected void Start () {
		
	}
	
	// Update is called once per frame
	override protected void FixedUpdate () {
		fsm.update ();
	}
		
	override protected void OnTriggerStay2D(Collider2D other){
		damagePlayerTrigger (other);
	}

	override protected void OnCollisionStay2D(Collision2D other){
		damagePlayerCollision (other);
	}

	protected void damagePlayerTrigger(Collider2D other){
		if(other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<movingObject> ().damage(dmg);
			if(other.gameObject.GetComponent<movingObject>().checkDead()){
				Debug.Log ("DEAD!");
			}
		}
	}

	protected void damagePlayerCollision(Collision2D other){
		if(other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<movingObject> ().damage(dmg);
			if(other.gameObject.GetComponent<movingObject>().checkDead()){
				Debug.Log ("DEAD!");
			}
		}
	}

	public void runToPlayer(){
		Transform target = GameObject.FindGameObjectWithTag ("Player").transform;
		//Gets the movement vector for the player
		Vector2 movementVector = new Vector2(target.position.x - GetComponent<Rigidbody2D>().transform.position.x, target.position.y - GetComponent<Rigidbody2D>().transform.position.y);
		//Adds a force to the player for the direction they are going
		//The movementvector does not exceed 1 meaning that diagonals are just as fast as moving horizontally or vertically
		GetComponent<Rigidbody2D>().AddForce (Vector3.ClampMagnitude(movementVector,1) * speed);
	}

	public void movePattern(){

	}

	public void fireAtPlayer(){
		if(fire()){
			Transform target = GameObject.FindGameObjectWithTag ("Player").transform;
			//Gets the movement vector for the player
			Vector2 directionVector = new Vector2(target.position.x - GetComponent<Rigidbody2D>().transform.position.x, target.position.y - GetComponent<Rigidbody2D>().transform.position.y);

			GameManager.instance.roomScript.createBullet (1, GetComponent<Rigidbody2D> ().position, Vector2.ClampMagnitude(directionVector,1), shotSpeed, dmg, range);
			lastFired = Time.time;
		}
	}

}