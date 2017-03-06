using UnityEngine;
using System.Collections;

public class Bullet_Enemy : Bullet {

	// Use this for initialization
	override protected void Start () {
		timeCreated = Time.time;
	}

	override protected void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<movingObject> ().damage(dmg);
			if(other.gameObject.GetComponent<movingObject>().checkDead()){
				Debug.Log ("Bullet DEAD");
			}
		}

		if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Gap" && other.gameObject.tag != "Projectile") {
			Destroy (this.gameObject);
		}
	}
}
