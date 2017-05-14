using UnityEngine;
using System.Collections;

public class Enemy_Slider : Enemy<Enemy_Slider> {
	
	public Vector2 direction;
	public float lastChange;

	// Use this for initialization
	override protected void Start () {
		lastChange = 0;
		setStats (22, 20, 0, 2, 20, 0, 0);
		GetComponent<Rigidbody2D> ().freezeRotation = true;
		fsm = new StateMachine<Enemy_Slider> (this);
		fsm.changeState (new Slider_ShootPlayer());
		currentColour = new Color (1f, 1f, 1f, 1f);
		changeDifficulty (modifier);
	}

	override protected void OnCollisionStay2D(Collision2D other){
		//Damage player if collide with player
		if(other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<movingObject> ().damage(dmg);
		}
		//If hit by the player, walls, doors, gaps or enemy - Change direction
		if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Gap" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "DoorLeft" || other.gameObject.tag == "DoorRight" || other.gameObject.tag == "DoorTop" || other.gameObject.tag == "DoorBottom") && Time.time > lastChange + 0.6f) {
			GetComponent<Rigidbody2D>().AddForce (Vector3.ClampMagnitude(this.transform.position-other.transform.position,1) * speed);
			changeDirection ();
		}
	}

	public void movePattern (){
		//Adds a force to the player for the direction they are going
		//The movementvector does not exceed 1 meaning that diagonals are just as fast as moving horizontally or vertically
		GetComponent<Rigidbody2D>().AddForce (Vector3.ClampMagnitude(direction,1) * speed);
	}

	void changeDirection(){
		if(direction.x == 0 && direction.y == -1){ //Check if direction is south, Change to left
			direction = new Vector2 (-1,0);
		} else if(direction.x == 1 && direction.y == 0){ //Check if direction is south, Change to left
			direction = new Vector2 (0,-1);
		} else if(direction.x == 0 && direction.y == 1){ //Check if direction is south, Change to left
			direction = new Vector2 (1,0);
		} else if(direction.x == -1 && direction.y == 0){ //Check if direction is south, Change to left
			direction = new Vector2 (0,1);
		}
		lastChange = Time.time;
		transform.Rotate (0,0,-90); //Rotate the sprite 90 degrees
	}
}

public class Slider_ShootPlayer : State<Enemy_Slider>{

	public void enter(Enemy_Slider agent){
		agent.direction = new Vector2 (0,1);
	}

	public void execute(Enemy_Slider agent){
		agent.movePattern ();
	}

	public void exit(Enemy_Slider agent){
	}
}
