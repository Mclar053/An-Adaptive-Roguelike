using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour{

	float score;

	public void resetScore(){
		score = 0;
	}

	public float getScore(){
		return score;
	}

	public void addScore(float _score){
		score += _score;
	}
}
