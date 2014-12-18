using UnityEngine;
using System.Collections;

public class RoomScript : MonoBehaviour {
	public float height, width;
	public int numberOfEnemies;
	public bool isPlayerHere;
	public GameObject[] doors;
	public GameObject mydoor;
	public GameObject[] allDoors;
	public Sprite closedDoorSprite;
	public Sprite openedDoorSprite;
	bool ifPlayerEntered;
	float PlayerLevel;
	public GameObject[] enemies;
	public bool isClear;
	CellMover  CellScript;
	float levelOfSpawnedEnemies;
	GameObject enemy;
	GameObject bigDoor;
	public GameObject[] barriers;
	public int minBarriers;
	public int maxBarriers;
	Vector3 enemyPos;
	public GameObject[] possibleDrop;
	// Use this for initialization
	void Start () {
		if (this.name == "BossRoom(Clone)")
						bigDoor = GameObject.FindGameObjectWithTag ("bigDoor");
		int n = 0;
		int i;
		allDoors = GameObject.FindGameObjectsWithTag ("door");
		doors = allDoors;
		for (i = 0; i<allDoors.Length; i++) {
			if(allDoors[i].transform.IsChildOf(this.transform)){
				doors[n] = allDoors[i];
	//			mydoor = allDoors[i];
				n++;
			}
		}
		while(n<i){
			doors[n] = null;
			n++;
		}
		CellScript = GameObject.FindGameObjectWithTag ("cell").GetComponent<CellMover>();
		for(int a = 0; a < Random.Range(minBarriers, maxBarriers); a++){
			Instantiate(barriers[Random.Range(0, barriers.Length)], new Vector2((transform.position.x+ Random.Range(2-width, width-2)),
			                               (transform.position.y+ Random.Range(2-height, height-2))), Quaternion.Euler(0, 0, Random.Range(0, 360)));
		}
	}
	// Update is called once per frame
	void Update () {
		if (CellScript.room == this.gameObject)
						isPlayerHere = true;
				else
						isPlayerHere = false;
		if (!ifPlayerEntered && isPlayerHere) {
			ifPlayerEntered = true;
			PlayerLevel = PlayerPrefs.GetFloat("level");
			if(enemies.Length!=0){
			while(levelOfSpawnedEnemies<Mathf.Sqrt(PlayerLevel)){
				enemy = enemies[Random.Range(0, enemies.Length)];
				levelOfSpawnedEnemies+=enemy.GetComponent<EnemyBehaviour>().levelOfEnemy;
				do{
							enemyPos = new Vector2(transform.position.x+ Random.Range(2-width, width-2), (transform.position.y+ Random.Range(2-height, height-2)));
				}while((enemyPos-GameObject.FindGameObjectWithTag ("cell").transform.position).magnitude<height);
				Instantiate(enemy, enemyPos, transform.rotation);
				if (this.name == "BossRoom(Clone)") break;

			}
			}

		}
		if (numberOfEnemies == 0||!isPlayerHere) {
			for (int i = 0; i<doors.Length; i++) {
				if(doors[i]!=null){
					doors[i].GetComponent<DoorScript>().closed = false;
					doors[i].GetComponent<SpriteRenderer>().sprite = openedDoorSprite;
					if(bigDoor!=null){
						bigDoor.GetComponent<DoorScript>().closed = false;
						bigDoor.GetComponent<SpriteRenderer>().sprite = openedDoorSprite;
					}
				}
			}
		}
		else{
			for (int i = 0; i<doors.Length; i++) {
				if(doors[i]!=null){
					doors[i].GetComponent<DoorScript>().closed = true;
					doors[i].GetComponent<SpriteRenderer>().sprite = closedDoorSprite;
				}
				if(bigDoor!=null){
					bigDoor.GetComponent<DoorScript>().closed = true;
					bigDoor.GetComponent<SpriteRenderer>().sprite = closedDoorSprite;
				}
			}
		}
		if (numberOfEnemies == 0 && isPlayerHere) {
			if(possibleDrop.Length!=0)
				Instantiate (possibleDrop[Random.Range(0, possibleDrop.Length-1)], transform.position, transform.rotation);
			isClear = true;
		}
		else
			isClear = false;
	}

}
