using UnityEngine;
using System.Collections;

public class TransactionScript : MonoBehaviour {
	public GameObject Cell;
	public GameObject TopWall;
	public GameObject BottomWall;
	public Vector3 bottomLeft;
	public Vector3 topRight;
	public float speed;
	public GameObject[] Objects;
	public float freq;
	public float movementSpeed;
	float timer;
	public float force;
	GameObject clone;
	Vector2[] collPoints;
	public GameObject cameraParentPrefab;
	//public GameObject cam;
	// Use this for initialization
	void Start () {
		Cell = GameObject.FindGameObjectWithTag ("cell");
		bottomLeft = GetComponent<Camera>().ScreenToWorldPoint (new Vector3 (0, 0, 0));
		topRight =  GetComponent<Camera>().ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0));
		collPoints = new Vector2[2];
		collPoints [0] = new Vector3(bottomLeft.x-2, bottomLeft.y, 0) - transform.position; collPoints [1] = new Vector3 (bottomLeft.x - 2, topRight.y, 0)-transform.position;
		GetComponent<EdgeCollider2D>().points = collPoints;
		TopWall.transform.position = new Vector3 (transform.position.x, topRight.y, 0);
		BottomWall.transform.position = new Vector3 (transform.position.x, bottomLeft.y, 0);
		Cell.transform.position = new Vector3 (bottomLeft.x, transform.position.y, 0);
	}
	
	// Update is called once per frame
	void Update () {
		Cell.transform.rotation = Quaternion.Euler (0, 0, 270);
		Cell.rigidbody2D.AddForce (-Vector3.right*speed*Cell.GetComponent<CellMover>().movementSpeed/10* Time.deltaTime * 60);

		timer += Time.deltaTime;
		if (timer > freq) {
			clone = Instantiate(Objects[Random.Range(0, Objects.Length-1)],
			                    new Vector2 (topRight.x, Random.Range(topRight.y-1, bottomLeft.y+1)),
			                    Quaternion.Euler (0, 0, Random.Range(0, 360))) as GameObject;
			clone.rigidbody2D.AddForce(Quaternion.Euler(0,0,180 + Random.Range(-30, 30))*Vector3.right*force, ForceMode2D.Impulse);
			timer = 0;
		}

		if (Cell.transform.position.x > topRight.x) {
			Destroy(GameObject.FindGameObjectWithTag ("CameraParent"));
		//	Instantiate(cam, new Vector3(0, 0, 0), Quaternion.Euler (0, 0, 0));
			Instantiate(cameraParentPrefab, new Vector3(0, 0, 0), Quaternion.Euler (0, 0, 0));
			foreach(GameObject item in GameObject.FindGameObjectsWithTag("room")){
				Destroy (item.gameObject);
			}
			foreach(GameObject item in GameObject.FindGameObjectsWithTag("barrier")){
				Destroy (item.gameObject);
			}
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D trig){
		if (trig.gameObject.tag == "cell") {
			trig.gameObject.GetComponent<CellMover>().health = 0;
		}
		else{
			Destroy (trig.gameObject);
		}
	}
}