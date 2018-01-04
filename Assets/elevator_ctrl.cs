using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator_ctrl : MonoBehaviour {
	public int TargetLevel;
	public int CurrentLevel;
	public int MovingDirection;

	void Update(){
		if (CurrentLevel - Mathf.FloorToInt (this.GetComponent<Transform> ().position.y) > 0.5)
			CurrentLevel = Mathf.CeilToInt (this.GetComponent<Transform> ().position.y);
		else
			CurrentLevel = Mathf.FloorToInt (this.GetComponent<Transform> ().position.y);
	}
}
