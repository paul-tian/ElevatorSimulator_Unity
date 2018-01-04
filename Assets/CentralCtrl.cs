using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CentralCtrl : MonoBehaviour {
	public GameObject Car1;
	public GameObject Car2;
	public Text InnerDisplay1;
	public Text InnerDisplay2;
	public Text OuterDisplayUp;
	public Text OuterDisplayDown;

	private int LevelCount;

	private int ForceAssignTo1;
	private int ForceAssignTo2;

	private int Speed1;
	private int Speed2;

	private string InnerString1;
	private string InnerString2;
	private string OuterStringUp;
	private string OuterStringDown;

	private float SavedTime;
	private float CurrentTime;
	private float WaitTime;

	private int Car1TargetLevel;
	private int Car1CurrentLevel;
	private int Car1MovingDirection;

	private int Car2TargetLevel;
	private int Car2CurrentLevel;
	private int Car2MovingDirection;

	private bool[] Car1Target = new bool [100];
	private bool[] Car1Display = new bool [100];

	private bool[] Car2Target = new bool [100];
	private bool[] Car2Display = new bool [100];

	private bool[] UpFlag = new bool [100];
	private bool[] DownFlag = new bool [100];

	private bool[] UpDisplay = new bool [100];
	private bool[] DownDisplay = new bool [100];

	private bool[] UpAssigned = new bool [100];
	private bool[] DownAssigned = new bool [100];

	void Start () { // Use this for initialization
		LevelCount = 24;
		Speed1 = 3;
		Speed2 = 3;
		WaitTime = 1;
		SavedTime = Time.realtimeSinceStartup;
		for (int i = 0; i < LevelCount; i++) {
			ForceAssignTo1 = 0;
			ForceAssignTo2 = 0;
			UpFlag [i] = false;
			UpAssigned [i] = false;
			DownFlag [i] = false;
			DownAssigned [i] = false;
			UpDisplay [i] = false;
			DownDisplay [i] = false;
		}
	}

	void Update () { // Update is called once per frame
		CurrentTime         = Time.realtimeSinceStartup;

		Car1CurrentLevel    = Car1.GetComponent <elevator_ctrl> ().CurrentLevel;
		Car1TargetLevel     = Car1.GetComponent <elevator_ctrl> ().TargetLevel;
		Car1MovingDirection = Car1.GetComponent <elevator_ctrl> ().MovingDirection;

		Car2CurrentLevel    = Car2.GetComponent <elevator_ctrl> ().CurrentLevel;
		Car2TargetLevel     = Car2.GetComponent <elevator_ctrl> ().TargetLevel;
		Car2MovingDirection = Car2.GetComponent <elevator_ctrl> ().MovingDirection;

		OuterDispatch ();
		InnerButtonDisplay ();
		OuterButtonDisplay ();
		if (CurrentTime - SavedTime >= WaitTime) {
			Car1Vector ();
			Car2Vector ();
			SavedTime = CurrentTime;
		}

		if (Car1CurrentLevel != Car1TargetLevel)
			Car1.GetComponent <Transform> ().Translate (new Vector3 (0, Car1MovingDirection, 0) * Time.deltaTime * Speed1);
		else {
			Car1Display [Car1TargetLevel] = false;
			Car1Target [Car1TargetLevel] = false;
			if (Car1MovingDirection == 1 && UpFlag [Car1TargetLevel] == true) {
				UpDisplay [Car1TargetLevel] = false;
				UpFlag [Car1TargetLevel] = false;
				UpAssigned [Car1TargetLevel] = false;
			}
			else if (Car1MovingDirection == 1 && DownFlag [Car1TargetLevel] == true) {
				if (ForceAssignTo1 == 1) {
					DownDisplay [Car1TargetLevel] = false;
					DownFlag [Car1TargetLevel] = false;
					DownAssigned [Car1TargetLevel] = false;
					Car1Display [Car1TargetLevel] = true;
					Car1Target [Car1TargetLevel] = true;
					Car2Target [Car1TargetLevel] = false;
					ForceAssignTo1 = 0;
				}
				if (ForceAssignTo1 == 0) {
					DownDisplay [Car1TargetLevel] = true;
					DownFlag [Car1TargetLevel] = true;
					DownAssigned [Car1TargetLevel] = false;
					Car1Target [Car1TargetLevel] = false;
					Car2Target [Car1TargetLevel] = true;
					ForceAssignTo2 = 1;
				}
			}
			else if (Car1MovingDirection == -1 && UpFlag [Car1TargetLevel] == true) {
				if (ForceAssignTo1 == 1) {
					UpDisplay [Car1TargetLevel] = false;
					UpFlag [Car1TargetLevel] = false;
					UpAssigned [Car1TargetLevel] = false;
					Car1Display [Car1TargetLevel] = true;
					Car1Target [Car1TargetLevel] = true;
					Car2Target [Car1TargetLevel] = false;
					ForceAssignTo1 = 0;
				}
				if (ForceAssignTo1 == 0) {
					int flag = 0;
					for (int i = Car1.GetComponent<elevator_ctrl> ().CurrentLevel; i >= 0 ; i--)
						if (Car1Target [i] == true) {
							Car1.GetComponent <elevator_ctrl> ().TargetLevel = i;
							flag = 1;
							break;
						}
					if (flag == 1) {
						Car1.GetComponent <Transform> ().Translate (new Vector3 (0, Car1MovingDirection, 0) * Time.deltaTime * Speed1);
						UpDisplay [Car1TargetLevel] = true;
						UpFlag [Car1TargetLevel] = true;
						UpAssigned [Car1TargetLevel] = false;
					} else
						Car1.GetComponent <elevator_ctrl> ().MovingDirection = 0;
				}
			}
			else if (Car1MovingDirection == -1 && DownFlag [Car1TargetLevel] == true) {
				DownDisplay [Car1TargetLevel] = false;
				DownFlag [Car1TargetLevel] = false;
				DownAssigned [Car1TargetLevel] = false;
			}
			else if (Car1MovingDirection == 0) {
				UpDisplay [Car1TargetLevel] = false;
				UpFlag [Car1TargetLevel] = false;
				UpAssigned [Car1TargetLevel] = false;
				DownDisplay [Car1TargetLevel] = false;
				DownFlag [Car1TargetLevel] = false;
				DownAssigned [Car1TargetLevel] = false;
			}
		}
		if (Car2CurrentLevel != Car2TargetLevel)
			Car2.GetComponent <Transform> ().Translate (new Vector3 (0, Car2MovingDirection, 0) * Time.deltaTime * Speed2);
		else {
			Car2Display [Car2TargetLevel] = false;
			Car2Target [Car2TargetLevel] = false;
			if (Car2MovingDirection == 1 && UpFlag [Car2TargetLevel] == true) {
				UpDisplay [Car2TargetLevel] = false;
				UpFlag [Car2TargetLevel] = false;
				UpAssigned [Car2TargetLevel] = false;
			}
			else if (Car2MovingDirection == 1 && DownFlag [Car2TargetLevel] == true) {
				if (ForceAssignTo2 == 1) {
					DownDisplay [Car2TargetLevel] = false;
					DownFlag [Car2TargetLevel] = false;
					DownAssigned [Car2TargetLevel] = false;
					Car2Display [Car2TargetLevel] = true;
					Car2Target [Car2TargetLevel] = true;
					Car1Target [Car2TargetLevel] = false;
					ForceAssignTo2 = 0;
				}
				if (ForceAssignTo2 == 0) {
					int flag = 0;
					for (int i = Car2.GetComponent <elevator_ctrl> ().CurrentLevel; i < LevelCount; i++)
						if (Car2Target [i] == true) {
							Car2.GetComponent <elevator_ctrl> ().TargetLevel = i;
							flag = 1;
							break;
						}
					if (flag == 1) {
						Car2.GetComponent <Transform> ().Translate (new Vector3 (0, Car2MovingDirection, 0) * Time.deltaTime * Speed2);
						DownDisplay [Car2TargetLevel] = true;
						DownFlag [Car2TargetLevel] = true;
						DownAssigned [Car2TargetLevel] = false;
					}
					else
						Car2.GetComponent <elevator_ctrl> ().MovingDirection = 0;
				}
			}
			else if (Car2MovingDirection == -1 && UpFlag [Car2TargetLevel] == true) {
				if (ForceAssignTo2 == 1) {
					UpDisplay [Car2TargetLevel] = false;
					UpFlag [Car2TargetLevel] = false;
					UpAssigned [Car2TargetLevel] = false;
					Car2Display [Car2TargetLevel] = true;
					Car2Target [Car2TargetLevel] = true;
					Car1Target [Car2TargetLevel] = false;
					ForceAssignTo2 = 0;
				}
				if (ForceAssignTo2 == 0) {
					UpDisplay [Car2TargetLevel] = true;
					UpFlag [Car2TargetLevel] = true;
					UpAssigned [Car2TargetLevel] = false;
					Car2Target [Car2TargetLevel] = false;
					Car1Target [Car2TargetLevel] = true;
					ForceAssignTo1 = 1;
				}
			}
			else if (Car2MovingDirection == -1 && DownFlag [Car2TargetLevel] == true) {
				DownDisplay [Car2TargetLevel] = false;
				DownFlag [Car2TargetLevel] = false;
				DownAssigned [Car2TargetLevel] = false;
			}
			else if (Car2MovingDirection == 0) {
				UpDisplay [Car2TargetLevel] = false;
				UpFlag [Car2TargetLevel] = false;
				UpAssigned [Car2TargetLevel] = false;
				DownDisplay [Car2TargetLevel] = false;
				DownFlag [Car2TargetLevel] = false;
				DownAssigned [Car2TargetLevel] = false;
			}
		}
	}

	public void Car1Vector () {
		int flag = 1;
		if (Car1.GetComponent <elevator_ctrl> ().MovingDirection == 1) {
			for (int i = Car1.GetComponent <elevator_ctrl> ().CurrentLevel; i < LevelCount; i++)
				if (Car1Target [i] == true) {
					Car1.GetComponent <elevator_ctrl> ().TargetLevel = i;
					flag = 0;
					break;
				}
		}
		else if (Car1.GetComponent <elevator_ctrl> ().MovingDirection == -1){
			for (int i = Car1.GetComponent<elevator_ctrl> ().CurrentLevel; i >= 0 ; i--)
				if (Car1Target [i] == true) {
					Car1.GetComponent <elevator_ctrl> ().TargetLevel = i;
					flag = 0;
					break;
				}
		}
		else if (Car1.GetComponent <elevator_ctrl> ().MovingDirection == 0){
			for (int i = 0; i < LevelCount ; i++)
				if (Car1Target [i] == true) {
					Car1.GetComponent <elevator_ctrl> ().TargetLevel = i;
					flag = 0;
					break;
				}
			if (Car1CurrentLevel > Car1.GetComponent <elevator_ctrl> ().TargetLevel)
				Car1.GetComponent <elevator_ctrl> ().MovingDirection = -1;
			else if (Car1CurrentLevel < Car1.GetComponent <elevator_ctrl> ().TargetLevel)
				Car1.GetComponent <elevator_ctrl> ().MovingDirection =  1;
			else
				Car1.GetComponent <elevator_ctrl> ().MovingDirection =  0;
		}
		if (flag == 1)
			Car1.GetComponent <elevator_ctrl> ().MovingDirection = 0;
	}

	public void Car2Vector () {
		int flag = 1;
		if (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 1) {
			for (int i = Car2.GetComponent <elevator_ctrl> ().CurrentLevel; i < LevelCount; i++)
				if (Car2Target [i] == true) {
					Car2.GetComponent <elevator_ctrl> ().TargetLevel = i;
					flag = 0;
					break;
				}
		}
		else if (Car2.GetComponent <elevator_ctrl> ().MovingDirection == -1){
			for (int i = Car2.GetComponent <elevator_ctrl> ().CurrentLevel; i >= 0 ; i--)
				if (Car2Target [i] == true) {
					Car2.GetComponent <elevator_ctrl> ().TargetLevel = i;
					flag = 0;
					break;
				}
		}
		else if (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 0) {
			for (int i = 0; i < LevelCount; i++)
				if (Car2Target [i] == true) {
					Car2.GetComponent <elevator_ctrl> ().TargetLevel = i;
					flag = 0;
					break;
				}
			if (Car2CurrentLevel > Car2.GetComponent <elevator_ctrl> ().TargetLevel)
				Car2.GetComponent <elevator_ctrl> ().MovingDirection = -1;
			else if (Car2CurrentLevel < Car2.GetComponent<elevator_ctrl>().TargetLevel)
				Car2.GetComponent <elevator_ctrl> ().MovingDirection =  1;
			else
				Car2.GetComponent <elevator_ctrl> ().MovingDirection =  0;
		}
		if (flag == 1)
			Car2.GetComponent <elevator_ctrl> ().MovingDirection = 0;
	}

	public void OuterDispatch () {
		for (int i = 0; i < LevelCount; i++) {
			if (UpDisplay [i] == true && UpAssigned [i] == false) {
				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 1)) {
					if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i) {
						if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - i) <= Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - i)) 
							{Car1Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
						else
							{Car2Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					} 
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i)
						{Car1Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i)
						{Car2Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i) {
						UpFlag [i] = true; UpDisplay [i] = true;
						/*if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - i) <= Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - i))
							{Car1Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
						else // may null it, have to test
							{Car2Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}*/
					}
				}// both direct up

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 0)) {
					if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel <= i) {
						if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - i) <= Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - i))
							{Car1Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
						else
							{Car2Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					} 
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i)
						{Car1Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i)
						{Car2Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
				}// 1 direct up & 2 stop

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 0) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 1)) {
					if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel <= i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i) {
						if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - i) <= Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - i))
							{Car1Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
						else
							{Car2Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					} 
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i)
						{Car2Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					else if (Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i)
						{Car1Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
				}// 2 direct up & 1 stop

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 0) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 0)) {
					if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - i) <= Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - i))
						{Car1Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					else
						{Car2Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
				}// both stop

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == -1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 0)) {
					Car2Target [i] = true; 
					UpFlag [i] = true;
					UpAssigned [i] = true;
				}// 1 direct down & 2 stop

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 0) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == -1)) {
					Car1Target [i] = true; 
					UpFlag [i] = true;
					UpAssigned [i] = true;
				}// 2 direct down & 1 stop

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == -1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == -1)) {
					UpFlag [i] = true;
					UpDisplay [i] = true;
				}// both direct down
 
				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == -1)) {
					if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i)
						{Car1Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i) {
						if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - (LevelCount - 1)) <= Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - 0))
							{Car1Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
						else
							{Car2Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i)
						{UpFlag [i] = true; UpDisplay [i] = true;}
				}// 1 direct up & 2 direct down

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == -1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 1)) {
					if (Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i)
						{Car2Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i) {
						if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - 0) <= Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - (LevelCount - 1)))
							{Car1Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
						else
							{Car2Target [i] = true; UpFlag [i] = true; UpAssigned [i] = true;}
					}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i)
						{UpFlag [i] = true; UpDisplay [i] = true;}
				}// 1 direct down & 2 direct up
			}
		
			if (DownDisplay [i] == true && DownAssigned [i] == false) {
				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == -1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == -1)) {
					if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i) {
						if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - i) < Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - i)) 
							{Car1Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
						else
							{Car2Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					} 
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i)
						{Car1Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i)
						{Car2Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i) {
						DownFlag [i] = true; DownDisplay [i] = true;
						/*if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - i) < Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - i))
							{Car1Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
						else // may null it, have to test
							{Car2Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}*/
					}
				}// both direct down

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == -1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 0)) {
					if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel >= i) {
						if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - i) < Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - i))
							{Car1Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
						else
							{Car2Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					} 
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i)
						{Car1Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i)
						{Car2Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
				}// 1 direct down & 2 stop

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 0) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == -1)) {
					if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel >= i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i) {
						if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - i) < Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - i))
						{Car1Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
						else
						{Car2Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					} 
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i)
						{Car2Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					else if (Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i)
						{Car1Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
				}// 2 direct down & 1 stop

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 0) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 0)) {
					if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - i) < Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - i))
						{Car1Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					else
						{Car2Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
				}// both stop

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 0)) {
					Car2Target [i] = true; 
					DownFlag [i] = true;
					DownAssigned [i] = true;
				}// 1 direct up & 2 stop

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 0) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 1)) {
					Car1Target [i] = true; 
					DownFlag [i] = true;
					DownAssigned [i] = true;
				}// 2 direct up & 1 stop

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 1)) {
					DownFlag [i] = true;
					DownDisplay [i] = true;
				}// both direct up

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == -1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == 1)) {
					if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i)
						{Car1Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i) {
						if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - 0) < Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - (LevelCount - 1)))
							{Car1Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
						else
							{Car2Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i)
						{DownFlag [i] = true; DownDisplay [i] = true;}
				}// 1 direct down & 2 direct up

				if ((Car1.GetComponent <elevator_ctrl> ().MovingDirection == 1) && (Car2.GetComponent <elevator_ctrl> ().MovingDirection == -1)) {
					if (Car2.GetComponent <elevator_ctrl> ().CurrentLevel > i)
					{Car2Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel > i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i) {
						if (Mathf.Abs (Car1.GetComponent <elevator_ctrl> ().CurrentLevel - (LevelCount - 1)) < Mathf.Abs (Car2.GetComponent <elevator_ctrl> ().CurrentLevel - 0))
							{Car1Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
						else
							{Car2Target [i] = true; DownFlag [i] = true; DownAssigned [i] = true;}
					}
					else if (Car1.GetComponent <elevator_ctrl> ().CurrentLevel < i && Car2.GetComponent <elevator_ctrl> ().CurrentLevel < i)
						{DownFlag [i] = true; DownDisplay [i] = true;}
				}// 1 direct up & 2 direct down
			}
		}
	}

	public void StartUp () {
		Speed1 = 3;
		Speed2 = 3;
		WaitTime = 1;
		for (int i = 0; i < LevelCount; i++) {
			UpFlag [i] = false;
			UpAssigned [i] = false;
			DownFlag [i] = false;
			DownAssigned [i] = false;
			UpDisplay [i] = false;
			DownDisplay [i] = false;
		}
		Car1Target [0] = true;
		Car2Target [Mathf.FloorToInt(LevelCount/2)] = true;
		SavedTime = Time.realtimeSinceStartup;
	}

	public void EmergencyStop1 () {
		Speed1 = 0;
		Car1.GetComponent <elevator_ctrl> ().MovingDirection = 0;
		for (int i = 0; i < LevelCount; i++) {
			Car1Target [i] = false;
			Car1Display [i] = false;
		}
	}

	public void EmergencyStop2 () {
		Speed2 = 0;
		Car2.GetComponent <elevator_ctrl> ().MovingDirection = 0;
		for (int i = 0; i < LevelCount; i++) {
			Car2Target [i] = false;
			Car2Display [i] = false;
		}
	}

	public void Self_Test () {
		for (int i = 0; i < LevelCount; i++) {
			Car1Target [i] = true;
			Car1Display [i] = true;
			Car2Target [i] = true;
			Car2Display [i] = true;
		}
		Car1.GetComponent <elevator_ctrl> ().MovingDirection = 1;
		Car2.GetComponent <elevator_ctrl> ().MovingDirection = 1;
	}

	public void ShutDown () {
		for (int i = 0; i < LevelCount; i++) {
			UpFlag [i] = false;
			UpAssigned [i] = false;
			DownFlag [i] = false;
			DownAssigned [i] = false;
			UpDisplay [i] = false;
			DownDisplay [i] = false;
			Car1Target [i] = false;
			Car1Display [i] = false;
			Car2Target [i] = false;
			Car2Display [i] = false;
		}
		Speed1 = 1;
		Speed2 = 1;
		Car1Target [0] = true;
		Car2Target [0] = true;
	}

	public void Manual_Test_Up () {
		Speed1 = 5;
		Speed2 = 5;
		WaitTime = 0.4f;
		for (int i = 1; i < LevelCount; i++) {
			Car1Target [i] = true;
			Car1Display [i] = true;
			Car2Target [i] = true;
			Car2Display [i] = true;
		}
		Car1.GetComponent <elevator_ctrl> ().MovingDirection = 1;
		Car2.GetComponent <elevator_ctrl> ().MovingDirection = 1;
	}

	public void Manual_Test_Down () {
		Speed1 = 5;
		Speed2 = 5;
		WaitTime = 0.4f;
		for (int i = LevelCount - 2; i >= 0; i--) {
			Car1Target [i] = true;
			Car1Display [i] = true;
			Car2Target [i] = true;
			Car2Display [i] = true;
		}
		Car1.GetComponent <elevator_ctrl> ().MovingDirection = -1;
		Car2.GetComponent <elevator_ctrl> ().MovingDirection = -1;
	}

	public void Car1Button (int level) {
			Car1Display [level] = true;
			Car1Target [level] = true;
	}

	public void Car2Button (int level) {
		Car2Display [level] = true;
		Car2Target [level] = true;
	}

	public void up_button (int level) {
		UpDisplay [level] = true;
	}

	public void down_button (int level) {
		DownDisplay [level] = true;
	}

	public void InnerButtonDisplay () {
		InnerString1 = "1#:\n";
		InnerString2 = "2#:\n";

		for (int i = LevelCount - 1; i >= 0; i--)
			if (Car1Display [i] == true)
				InnerString1 += ((i + 1) + "\n");
		
		for (int i = LevelCount - 1; i >= 0; i--)
			if (Car2Display [i] == true)
				InnerString2 += ((i + 1) + "\n");

		InnerDisplay1.text = InnerString1;
		InnerDisplay2.text = InnerString2;
	}

	public void OuterButtonDisplay () {
		OuterStringUp = "Up\n";
		OuterStringDown = "Down\n";

		for (int i = (LevelCount - 1); i >= 0; i--)
			if (UpDisplay [i] == true && UpFlag [i] == true)
				OuterStringUp += ((i + 1) + "\n");

		for (int i = (LevelCount - 1); i >= 0; i--)
			if (DownDisplay [i] == true && DownFlag [i] == true)
				OuterStringDown += ((i + 1) + "\n");

		OuterDisplayUp.text = OuterStringUp;
		OuterDisplayDown.text = OuterStringDown;
	}

	public void ExitProgram() {
		Application.Quit (); 
	}

	public void ReloadProgram() {
		SceneManager.LoadScene ("test1");
	}

}

//CarX.GetComponent <elevator_ctrl> ().CurrentLevel;
//CarX.GetComponent <elevator_ctrl> ().TargetLevel;
//CarX.GetComponent <elevator_ctrl> ().MovingDirection;
