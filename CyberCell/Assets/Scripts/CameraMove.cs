using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	public GameObject Cell;
	public float speed;
	Vector3 target;
	Vector2 TopRight;
	public float top, right, bottom, left;
	GameObject clone;
	public GameObject[] possibleRooms;
	public GameObject bossRoom;
	public GameObject startRoom;
	CellMover CellScript;
	bool BossRoomSet;
	bool bigDoorSpawned;
	public GameObject[] playerPers;
	public int[,] roomsPositions;
	//public GameObject cameraM;
	bool NotFirstFrame;
	int m;
	Color[] colors;
	public bool lsdBool;
	//  Use this for initialization
	public void Start () {
		colors = new Color[6]{Color.red, Color.cyan, Color.yellow, Color.green, Color.magenta, Color.blue};
//		cameraM = GameObject.FindGameObjectWithTag("Player");
		roomsPositions = new int[20,20]{
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
		if(GameObject.FindWithTag("cell") == null){
			switch (PlayerPrefs.GetString ("PlayerPers")) {
			case "Biotic1":
				Instantiate (playerPers [0], transform.position, transform.rotation);
				break;
			case "Biotic2":
				Instantiate (playerPers [1], transform.position, transform.rotation);
				lsdBool = true;
				break;
			case "Biotic3":
				Instantiate (playerPers [2], transform.position, transform.rotation);
				break;
			case "Cyborg1":
				Instantiate (playerPers [3], transform.position, transform.rotation);
				break;
			case "Cyborg2":
				Instantiate (playerPers [4], transform.position, transform.rotation);
				break;
			case "Cyborg3":
				Instantiate (playerPers [5], transform.position, transform.rotation);
				break;
			case "Nanobot1":
				Instantiate (playerPers [6], transform.position, transform.rotation);
				break;
			case "Nanobot2":
				Instantiate (playerPers [7], transform.position, transform.rotation);
				break;
			case "Nanobot3":
				Instantiate (playerPers [8], transform.position, transform.rotation);
				break;
			default:
				Instantiate (playerPers [0], transform.position, transform.rotation);
				break;
			}
		}
		GameObject room = startRoom;
		TopRight = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 0));
		int i = 10;
		int j = 10;
		int dir;
		int numberOfRooms = Random.Range (6, 9);
		for(int a = 0; a <= numberOfRooms; a+=0){
			if(roomsPositions[i, j] != 1)	a++;
			roomsPositions [i, j] = 1;
			do{
				dir = Random.Range(0, 3);
				switch(dir){
				case 0:
					i++;
					break;
				case 1:
					j++;
					break;
				case 2:
					i--;
					break;
				case 3:
					j--;
					break;
				}
			}while(i<0||j<0||i>19||j>19);
		}
		//new Color[]{Color.red, Color.green, Color.cyan, Color.yellow};
		float currX = 0;
		float currY = 0;
		for(int l = 0; l<=19; l++){
			for(int m = 0; m<=19; m++){
				if(roomsPositions[l, m] == 1){
					if((l < 19)&&(roomsPositions[l+1, m] == 0)&&!BossRoomSet){
						roomsPositions[l+1, m] = 2;
						BossRoomSet = true;
						currX = (l+1)*2*room.GetComponent<RoomScript>().width;
						currY = m*2*room.GetComponent<RoomScript>().height;
						clone = Instantiate(bossRoom, new Vector2(currX, currY), transform.rotation) as GameObject;
						if(l < 19&&roomsPositions[l+2, m] >0){
							clone.transform.FindChild ("Right").FindChild ("door").gameObject.tag = "door";
						}
						else if(!bigDoorSpawned){
							clone.transform.FindChild ("Right").FindChild ("door").gameObject.tag = "bigDoor";
							bigDoorSpawned = true;
						}
						if(l > 0&&roomsPositions[l, m] >0){
							clone.transform.FindChild ("Left").FindChild ("door").gameObject.tag = "door";
						}
						else if(!bigDoorSpawned){
							clone.transform.FindChild ("Left").FindChild ("door").gameObject.tag = "bigDoor";
							bigDoorSpawned = true;
						}
						if(m < 19&&roomsPositions[l+1, m+1] >0){
							clone.transform.FindChild ("Top").FindChild ("door").gameObject.tag = "door";
						}
						else if(!bigDoorSpawned){
							clone.transform.FindChild ("Top").FindChild ("door").gameObject.tag = "bigDoor";
							bigDoorSpawned = true;
						}
						if(m > 0&&roomsPositions[l+1, m-1] >0){
							clone.transform.FindChild ("Bottom").FindChild ("door").gameObject.tag = "door";
						}
						else if(!bigDoorSpawned){
							clone.transform.FindChild ("Bottom").FindChild ("door").gameObject.tag = "bigDoor";
							bigDoorSpawned = true;
						}
						
					}
					currX = l*2*room.GetComponent<RoomScript>().width;
					currY = m*2*room.GetComponent<RoomScript>().height;
					clone = Instantiate(room, new Vector2(currX, currY), transform.rotation) as GameObject;
					if(l < 19&&roomsPositions[l+1, m] > 0){
						clone.transform.FindChild ("Right").FindChild ("door").gameObject.tag = "door";
					}
					if(l > 0&&roomsPositions[l-1, m] > 0){
						clone.transform.FindChild ("Left").FindChild ("door").gameObject.tag = "door";
					}
					if(m < 19&&roomsPositions[l, m+1] > 0){
						clone.transform.FindChild ("Top").FindChild ("door").gameObject.tag = "door";
					}
					if(m > 0&&roomsPositions[l, m-1] > 0){
						clone.transform.FindChild ("Bottom").FindChild ("door").gameObject.tag = "door";
					}
					room = possibleRooms[Random.Range(0, possibleRooms.Length)];
				}
			}
		}
	//	cameraM.transform.parent = transform;
		Cell = GameObject.FindGameObjectWithTag ("cell");
		CellScript = Cell.GetComponent<CellMover> ();	

	}
	
	// Update is called once per frame
	void Update () {
		if (lsdBool) {
			foreach(GameObject epiteliyka in GameObject.FindGameObjectsWithTag ("dynamicBarrier")){
				epiteliyka.rigidbody2D.mass = 0.000001f;
				epiteliyka.GetComponent<TrailRenderer>().enabled = true;
				epiteliyka.collider2D.sharedMaterial.bounciness = 1;
			}
			Cell.GetComponent<TrailRenderer>().enabled = true;;
			StartCoroutine (lsd ());
		}
		if (!NotFirstFrame)
						NotFirstFrame = true;
				else {
						
						top = CellScript.room.GetComponent<RoomScript> ().height + TopRight.y + CellScript.room.transform.position.y;
						right = CellScript.room.GetComponent<RoomScript> ().width + TopRight.x + CellScript.room.transform.position.x;
						bottom = -CellScript.room.GetComponent<RoomScript> ().height - TopRight.y + CellScript.room.transform.position.y;
						left = -CellScript.room.GetComponent<RoomScript> ().width - TopRight.x + CellScript.room.transform.position.x;
						rigidbody2D.AddForce ((Cell.transform.position - transform.position) * speed * Time.deltaTime * 60);
						transform.position = new Vector3 (Mathf.Clamp (transform.position.x, left, right), Mathf.Clamp (transform.position.y, bottom, top), transform.position.z);
				}
	}

	public IEnumerator lsd(){
		while (true) {
			Camera.main.backgroundColor = colors[m];
			m++;
			if(m>=colors.Length) m =0;
			yield return new WaitForSeconds(0.1f);
		}

	}

	void OnDestroy(){
		foreach(GameObject epiteliyka in GameObject.FindGameObjectsWithTag ("dynamicBarrier")){
			epiteliyka.rigidbody2D.mass = 50;
			epiteliyka.GetComponent<TrailRenderer>().enabled = false;
			epiteliyka.collider2D.sharedMaterial.bounciness = 0.1f;
		}
//		epiteliyka.rigidbody2D.mass = 50;
		Cell.GetComponent<TrailRenderer>().enabled = false;
	//	epiteliyka.GetComponent<TrailRenderer>().enabled = false;
	//	epiteliyka.collider2D.sharedMaterial.bounciness = 0.1f;
	}
}
