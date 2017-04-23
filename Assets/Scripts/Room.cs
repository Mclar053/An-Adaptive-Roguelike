using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room {

	public int[,] layout;
	public int[,] entities; //[0]-ID [1]-X  [2]-Y
	float basePromote, baseDemote; //Add promotion and demotion values from LoadXMLData.cs
	float baseScoreChange;

	public Room(int _w, int _h, int _entityNum){
		layout = new int[_w,_h];
		entities = new int[_entityNum,3];
		baseScoreChange = 0.07f;
	}

	public void addBaseModifierValues(float _promote, float _demote){
		basePromote = _promote;
		baseDemote = _demote;
	}

	public void setEntity(int _pos, int _x, int _y, int _id){
		entities [_pos, 0] = _id-2;
		entities [_pos, 1] = _x;
		entities [_pos, 2] = _y;
	}

	public int checkRoomModifier(float _roomScore, int currentRoomMod){
		if (_roomScore < basePromote / (baseScoreChange * currentRoomMod + 0.5f)) {
			return 1;
		} else if (_roomScore > baseDemote / (baseScoreChange * currentRoomMod + 0.5f) && currentRoomMod > -5) {
			return -1;
		}
		return 0;
	}
}
