using UnityEngine;
using System.Collections;

public class Bullet_Enemy : Bullet {

	// Use this for initialization
	override protected void Start () {
		timeCreated = Time.time;
		currentColour = new Color (1f, 1f, 1f, 1f);
	}

	override protected void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<movingObject> ().damage(dmg);
		}

		if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Gap" && other.gameObject.tag != "Projectile" && other.gameObject.tag != "Pickup" && other.gameObject.tag != "NextFloor") {
			Destroy (this.gameObject);
		}
	}
}
