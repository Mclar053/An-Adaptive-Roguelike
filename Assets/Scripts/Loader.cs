using UnityEngine;
using System.Collections;

/* Reference: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager?playlist=17150
 * Author: Matt Schell
 */ 

public class Loader : MonoBehaviour {

	public GameObject gameManager;

	void Awake () {
		if(GameManager.instance == null){
			Instantiate (gameManager);
		}
	}
}
