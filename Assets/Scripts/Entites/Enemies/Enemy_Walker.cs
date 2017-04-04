using UnityEngine;
using System.Collections;

public class Enemy_Walker : Enemy<Enemy_Walker> {

	// Use this for initialization
	override protected void Start () {
		GetComponent<Rigidbody2D> ().freezeRotation = true;
		setStats (12, 10, 0, 2, 20, 0, 0);
		fsm = new StateMachine<Enemy_Walker> (this);
		fsm.changeState (new Walker_RunToPlayer());
	}
}

public class Walker_RunToPlayer : State<Enemy_Walker>{

	public void enter(Enemy_Walker agent){
		
	}

	public void execute(Enemy_Walker agent){
		agent.runToPlayer ();
	}

	public void exit(Enemy_Walker agent){
	}
}