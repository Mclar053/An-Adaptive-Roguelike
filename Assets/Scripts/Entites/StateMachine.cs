using UnityEngine;
using System.Collections;

//Generic States
public interface State<A>{

	void enter (A agent);
	void execute (A agent);
	void exit (A agent);
}

//Generic State Machine
public class StateMachine<A> {

	public A agent; //The agent of the type of the template
	public State<A> current; //The current state of the type of the template

	public StateMachine(A a) { //Constructor passes in an agent of the type of the template
		agent = a; //Sets the passed in agent to the class member agent 
	}

	public void changeState(State<A> next) {//Changes state to the next state
		if(current != null){
			current.exit(agent); //Exits the current state, passes in the agent
		}
		current = next; //Changes the state
		current.enter(agent); //Enters the agent into the new current state
	}

	public void update() {
		current.execute(agent); //Executes the current state with the agent
	}
}