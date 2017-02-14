using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	BoardCreator b; //Boardcreator object
	mTree t; //BSP Tree
	bool showCorridors, showRooms; //Booleans to display corridors and rooms respectively
	int levelNum; //Level number

	public GameObject floor_Prefab;
	public GameObject rooms_Parent;

	//Corridors
	public GameObject h_corridor_Prefab;
	public GameObject v_corridor_Prefab;
	public GameObject corridors_Parent;

	//Walls
	public GameObject h_wall_Prefab;
	public GameObject v_wall_Prefab;

	//Used for GUIDrawRect function
	private static Texture2D _staticRectTexture;
	private static GUIStyle _staticRectStyle;

	void Start () {
		levelNum = 1; //Level start at 1
		createLevel (); //Create the level and all the sections
		showCorridors = true; //Display all corridors
		showRooms = true; //Display all rooms

	}

	void Update () {
		//Create new level
		if (Input.GetKeyDown ("r")) {
			createLevel();
		}

		//Display corridors change from true to false and visa versa
		if (Input.GetKeyDown ("z")) {
			showCorridors = !showCorridors;
		}

		//Display rooms change from true to false and visa versa
		if (Input.GetKeyDown ("x")) {
			showRooms = !showRooms;
		}

		//Change level number and create new level
		if(Input.GetKeyDown(key:KeyCode.LeftArrow)){
			if (levelNum-- <= 0) {
				levelNum++;
			} else {
				createLevel ();
			}
		}
		if(Input.GetKeyDown(key:KeyCode.RightArrow)){
			levelNum++;
			createLevel ();
		}
	}

	void OnGUI(){
		/*
		//Get all sections created in board creator object
		List<Section> boardSections = b.getSections ();

		if (showRooms) {
			//Display all leaf node board sections as rooms
			foreach (Section s in boardSections) {
				if (t.getNodes () [s.nodeIndex].getChildren () == null) {
					Rect room = new Rect (new Vector2 (s.pos.x+s.size.x * 0.1f, s.pos.y+s.size.y * 0.1f), new Vector2 (s.size.x * 0.8f, s.size.y * 0.8f));
					Color roomCol = new Color (0, 255, 0);
					GUIDrawRect (room, roomCol, s.nodeIndex);
				}
			}
		}
			
		if (showCorridors) {
			//Display all corridors in GUI
			foreach (Section s in boardSections) {
				Rect corridor;
				Color corridorCol = new Color (255, 0, 0);

				//Draw verticle lines
				if (s.centerPos.x == boardSections [s.parent].centerPos.x) {
					float yPos;

					if (s.centerPos.y < boardSections [s.parent].centerPos.y) {
						yPos = s.centerPos.y;
					} else {
						yPos = boardSections [s.parent].centerPos.y;
					}

					corridor = new Rect (new Vector2 (s.centerPos.x, yPos), new Vector2 (2f, Mathf.Abs (s.centerPos.y - boardSections [s.parent].centerPos.y)));
				}
				//Draw horizontal lines
				else {
					float xPos;
					if (s.centerPos.x < boardSections [s.parent].centerPos.x) {
						xPos = s.centerPos.x;
					} else {
						xPos = boardSections [s.parent].centerPos.x;
					}
					corridor = new Rect (new Vector2 (xPos, s.centerPos.y), new Vector2 (Mathf.Abs (s.centerPos.x - boardSections [s.parent].centerPos.x), 2f));
				}
				GUIDrawRect (corridor, corridorCol, -1);
			}
		}

		//Display the level number
		GUI.Label (new Rect (new Vector2 (0, 0), new Vector2 (150, 30)), string.Format("Level Number: {0}",levelNum));
		*/
	}

	//Create new tree and new boardcreator
	void createLevel(){

		foreach(Transform child in rooms_Parent.transform){
			GameObject.Destroy (child.gameObject);
		}
		foreach(Transform child in corridors_Parent.transform){
			GameObject.Destroy (child.gameObject);
		}

		t = new mTree(levelNum);
		t.printNodes ();
		b = new BoardCreator (t);
		List<Section> sections = b.getSections();

		//Create Rooms
		foreach (Section _s in sections) {
			if (t.getNodes () [_s.nodeIndex].getChildren () == null) {
				GameObject floorGO = Instantiate (floor_Prefab, new Vector2 (_s.centerPos.x - 5, _s.centerPos.y - 5), Quaternion.identity,rooms_Parent.transform) as GameObject;
				floorGO.transform.localScale = new Vector2 (_s.size.x * 0.7f, _s.size.y * 0.7f);
			}
		}

		/*
		//Create Corridors
		foreach (Section _s in sections) {
			GameObject corridorGO;

			//Vertical Corridor
			if (_s.centerPos.x == sections [_s.parent].centerPos.x) {
				corridorGO = Instantiate (v_corridor_Prefab, new Vector3 (_s.centerPos.x-5, (_s.centerPos.y + sections [_s.parent].centerPos.y) / 2f-5,-1f), Quaternion.identity,corridors_Parent.transform) as GameObject;
				corridorGO.transform.localScale = new Vector2 (0.05f, Mathf.Abs(_s.centerPos.y-sections [_s.parent].centerPos.y));
			}
			//Horizontal Corridor
			else {
				corridorGO = Instantiate (h_corridor_Prefab, new Vector3((_s.centerPos.x+sections[_s.parent].centerPos.x)/2f-5,_s.centerPos.y-5,-1f), Quaternion.identity,corridors_Parent.transform) as GameObject;
				corridorGO.transform.localScale = new Vector2 (Mathf.Abs(_s.centerPos.x-sections [_s.parent].centerPos.x), 0.05f);
			}
		}
		*/
	}


	// Original Code by User: IQpierce from https://forum.unity3d.com/threads/draw-a-simple-rectangle-filled-with-a-color.116348/
	// Note that this function is only meant to be called from OnGUI() functions.
	public static void GUIDrawRect( Rect position, Color color, int _index )
	{
		if( _staticRectTexture == null )
		{
			_staticRectTexture = new Texture2D( 1, 1 );
		}

		if( _staticRectStyle == null )
		{
			_staticRectStyle = new GUIStyle();
		}

		_staticRectTexture.SetPixel( 0, 0, color );
		_staticRectTexture.Apply();

		_staticRectStyle.normal.background = _staticRectTexture;

		if (_index == -1) {
			GUI.Box (position, " ", _staticRectStyle);
		} else {
			GUI.Box (position, _index + " ", _staticRectStyle);
		}


	}
}
