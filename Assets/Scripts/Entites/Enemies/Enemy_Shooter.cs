using UnityEngine;
using System.Collections;

public class Enemy_Shooter : Enemy<Enemy_Shooter> {

	protected override void Start (){
		GetComponent<Rigidbody2D> ().freezeRotation = true;
		setStats(10,10,0,2,1f,7,2);
		lastFired = Time.time + 1f;
		fsm = new StateMachine<Enemy_Shooter> (this);
		fsm.changeState( new Shooter_ShootPlayer ());
		currentColour = new Color (1f, 1f, 1f, 1f);
	}

	public void fireAtPlayer(){
		if(fire()){
			Transform target = GameObject.FindGameObjectWithTag ("Player").transform;
			//Gets the movement vector for the player
			Vector2 directionVector = new Vector2(target.position.x - GetComponent<Rigidbody2D>().transform.position.x, target.position.y - GetComponent<Rigidbody2D>().transform.position.y);

			GameManager.instance.roomScript.createBullet (1, GetComponent<Rigidbody2D> ().position, Vector2.ClampMagnitude(directionVector,1), shotSpeed, dmg, range);
			lastFired = Time.time;
			fireDelay = (Vector2.SqrMagnitude (target.position - GetComponent<Rigidbody2D> ().transform.position)/10f) + 0.7f;
		}
	}

	public void charge(){
		currentColour = new Color (1f - (Time.time - lastFired) / fireDelay, 1f - (Time.time - lastFired) / fireDelay, 1f);
	}
}

public class Shooter_ShootPlayer : State<Enemy_Shooter>{

	public void enter(Enemy_Shooter agent){

	}

	public void execute(Enemy_Shooter agent){
		agent.fireAtPlayer ();
		agent.charge ();
	}

	public void exit(Enemy_Shooter agent){
	}
}
