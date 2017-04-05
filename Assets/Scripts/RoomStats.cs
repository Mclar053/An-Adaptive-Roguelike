using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomStats {

	public int roomID;

	//Stats
	public float startTime;
	public float endTime;
	float timeToKillFirstEnemy;
	float timeToGetHit;
	float timeToCompleteRoom;
	float damageTakenInRoom;

	//Player Stats
	//float playerSpeed;
	//float playerFireDelay;
	//float playerDamage;
	//float playerShotSpeed;
	//float playerCurrentHP;
	//float playerMaxHP;

	bool death;
	bool complete;

	public RoomStats(){

	}

	public RoomStats(RoomStats _room){
		roomID = _room.roomID;
		startTime = _room.startTime;
		endTime = _room.endTime;
		complete = _room.isComplete ();
	}

	public void completed(){
		complete = true;
	}

	public bool isComplete(){
		return complete;
	}

}
 