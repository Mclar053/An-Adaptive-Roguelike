using UnityEngine;
using System.Collections;

public class Enemy_Ghost : Enemy<Enemy_Ghost> {

	override protected void Start (){
		setStats(7,10,0,2,0,0,0);
		GetComponent<Rigidbody2D> ().freezeRotation = true;
		fsm = new StateMachine<Enemy_Ghost> (this);
		fsm.changeState(new Ghost_RunToPlayer ());
	}
}

public class Ghost_RunToPlayer : State<Enemy_Ghost>{

	public void enter(Enemy_Ghost agent){
		
	}

	public void execute(Enemy_Ghost agent){
		agent.runToPlayer ();
	}

	public void exit(Enemy_Ghost agent){
	}
}