using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DepthFirstStack<T> : MonoBehaviour {

	//stack is a list of T objects
	private List<T> stack;
	public int Count; //Number of items in the stack

	//Constructor
	//Creates new list called stack and count starts as 0
	public DepthFirstStack(){
		stack = new List<T> ();
		Count = 0;
	}
		
	//Add new item to stack
	public void push(T _object){
		stack.Insert (0, _object);
		Count = stack.Count;
	}

	//Pop top item off stack
	public T pop(int _index=0){
		T item = stack [_index];
		stack.RemoveAt (_index);
		Count = stack.Count;
		return item;
	}
}
