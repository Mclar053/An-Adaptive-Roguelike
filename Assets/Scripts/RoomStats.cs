using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomStats {

	public int roomID;

	//Stats
	public float startTime;
	public float endTime;
	public float timeToKillFirstEnemy;
	public float timeToGetHit;
	public float damageTakenInRoom;

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
		timeToKillFirstEnemy = 0;
		timeToGetHit = 0;
		damageTakenInRoom = 0;
		startTime = 0;
		endTime = 0;
	}

	public RoomStats(RoomStats _room){
		roomID = _room.roomID;
		startTime = _room.startTime;
		endTime = _room.endTime;
		complete = _room.isComplete ();

		timeToKillFirstEnemy = _room.timeToKillFirstEnemy;
//		timeToGetHit = _room.timeToGetHit;
		damageTakenInRoom = _room.damageTakenInRoom;
	}

	public void completed(){
		complete = true;
	}

	public bool isComplete(){
		return complete;
	}

	public float timeToCompleteRoom(){
		return endTime - startTime;
	}

}
 