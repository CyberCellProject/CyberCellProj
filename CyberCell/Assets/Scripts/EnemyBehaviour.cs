using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public float movementSpeed = 5f;
	Quaternion StartRotation;
	public float RotationSpeed;
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	public int framesPerSecond;
	GameObject player;
	public float health;
	public float maxHealth;
	public GameObject room;
	public float levelOfEnemy;
	public GameObject[] dropItems;
	public int[] chanses;
	GameObject clone;
	public float fieldOfView;
	public bool seePlayer;
	Vector3 direction;
	Vector3 prevPlayerPos;
	public bool isBoss;
	public string BossType;
	int state = 0;
	float waitForNextState;
	RaycastHit2D hit;
	bool areLasersShooting;
	int secondsBreak;
	public float repulsionStrength;
	public float touchDamage;
	//int c;
	// Use this for initialization
	void Start () {
		if (isBoss && BossType == "Goo") {
			StartCoroutine (BossGoo(0));	
		}
		health = maxHealth;
		GameObject[] rooms;
		rooms = GameObject.FindGameObjectsWithTag ("room");
		for(int i= 0; i<rooms.Length; i++){
			if(Mathf.Abs(rooms[i].transform.position.x-transform.position.x)<rooms[i].GetComponent<RoomScript>().width&&
			   Mathf.Abs(rooms[i].transform.position.y-transform.position.y)<rooms[i].GetComponent<RoomScript>().height){
				room = rooms[i];
				break;
			}
		}
		room.GetComponent<RoomScript> ().numberOfEnemies ++;
		spriteRenderer = renderer as SpriteRenderer;
		player = GameObject.FindGameObjectWithTag ("cell");
	}

	// Update is called once per frame
	void Update () {
		if (health != maxHealth&&!isBoss)
						seePlayer = true;
		if ((player.transform.position - transform.position).sqrMagnitude <= fieldOfView&&!isBoss)
						seePlayer = true;
		if(seePlayer||isBoss){
			if(!isBoss){
				direction = player.transform.position - transform.position;
			}
			rigidbody2D.AddForce (direction * movementSpeed* Time.deltaTime * 60);
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (Vector3.forward, player.transform.position - transform.position), Time.deltaTime * RotationSpeed);
			if (rigidbody2D.velocity.sqrMagnitude > 0.01f) {
				int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
				index = index % sprites.Length;
				spriteRenderer.sprite = sprites [index];
			}
		}
		if (health <= 0) {
			room.GetComponent<RoomScript> ().numberOfEnemies --;
			PlayerPrefs.SetFloat("level", PlayerPrefs.GetFloat ("level")+levelOfEnemy);
			if(dropItems.Length!=0){
				for(int c = 0; c<=dropItems.Length-1; c++){
					if(chanses[c]<0){
						for(int k = 0; k < -chanses[c]; k++){
							if(Random.Range(0, 2) == 0){
								clone = Instantiate(dropItems[c], transform.position, transform.rotation) as GameObject;
								clone.rigidbody2D.AddForce (new Vector2(Random.Range (-2f, 2f), Random.Range (-2f, 2f)), ForceMode2D.Impulse);
							}
						}
					}
					else{
						if(Random.Range(0, chanses[c])==0){
							Instantiate(dropItems[c], transform.position, transform.rotation);
						}
					}
				}
			}
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "cell") {
			coll.gameObject.GetComponent<CellMover>().health -=	touchDamage*(1-coll.gameObject.GetComponent<CellMover>().defense);
			coll.gameObject.rigidbody2D.AddForce((-transform.position + coll.gameObject.transform.position)*repulsionStrength);
		}
	/*	if (coll.gameObject.tag == "barrier"&&coll.gameObject.rigidbody2D != null) {
			coll.gameObject.rigidbody2D.AddForce((-transform.position + coll.gameObject.transform.position)*repulsionStrength);
		}
*/	}

	public IEnumerator BossGoo(int startState){
		while (true) {
			player = GameObject.FindWithTag("cell");
			if(state >= 3){
				state =0;
				secondsBreak = 8;
				StopCoroutine("fourLasers");
			}
			if(state == 0){
				direction = (player.transform.position-transform.position)*((player.transform.position-transform.position).magnitude-5)/5+
					new Vector3(Random.Range(-10, 10),Random.Range(-7, 7));
				seePlayer = false;
				waitForNextState = 3;
			}
			else if(state == 1){
				direction = 2*(player.transform.position-transform.position);
				seePlayer = true;
				waitForNextState = 5;
			}
			else if(state == 2){
				direction = (player.transform.position-transform.position)*((player.transform.position-transform.position).magnitude-5)/5+
					new Vector3(Random.Range(-10, 10),Random.Range(-7, 7));
				seePlayer = false;
				secondsBreak =0;
				StartCoroutine (fourLasers());
				waitForNextState = 5;

			}
			state ++;
			yield return new WaitForSeconds(waitForNextState);

		}

	}
	public IEnumerator fourLasers(){
	while(true){
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.up, 100);
		for(int i = 1; i< hits.Length; i++){
			if(!hits[i].collider.isTrigger){
				hit = hits[i];
				if(hit.collider.gameObject.tag == "cell"){
					hit.collider.gameObject.GetComponent<CellMover>().health -= 2.5f;
				}
				break;
			}
		}
		if (hit.collider != null && !hit.collider.isTrigger) {
			for (int c = 0; c<10; c++){
					GLDebug.DrawLine(transform.position+new Vector3(0,1,0)+(Vector3.right*0.015f*c),
					                 hit.point+new Vector2((Vector2.right*0.015f*c).x, (Vector2.right*0.015f*c).y), Color.magenta);
					GLDebug.DrawLine(transform.position+new Vector3(0,1,0) -(Vector3.right*0.015f*c),
					                 hit.point-new Vector2((Vector2.right*0.015f*c).x, (Vector2.right*0.015f*c).y), Color.magenta);
			}
		}
		hits = Physics2D.RaycastAll(transform.position, -Vector2.up, 100);
		for(int i = 1; i< hits.Length; i++){
			if(!hits[i].collider.isTrigger){
				hit = hits[i];
				if(hit.collider.gameObject.tag == "cell"){
					hit.collider.gameObject.GetComponent<CellMover>().health -= 2.5f;
				}
				break;
			}
		}
		if (hit.collider != null && !hit.collider.isTrigger) {
			for (int c = 0; c<10; c++){
					GLDebug.DrawLine(transform.position+new Vector3(0,-1,0)+(-Vector3.right*0.015f*c),
					                 hit.point+new Vector2((-Vector2.right*0.015f*c).x, (-Vector2.right*0.015f*c).y), Color.magenta);
					GLDebug.DrawLine(transform.position+new Vector3(0,-1,0) -(-Vector3.right*0.015f*c),
					                 hit.point-new Vector2((-Vector2.right*0.015f*c).x, (-Vector2.right*0.015f*c).y), Color.magenta);
			}
		}
		hits = Physics2D.RaycastAll(transform.position, Vector2.right, 100);
		for(int i = 1; i< hits.Length; i++){
			if(!hits[i].collider.isTrigger){
				hit = hits[i];
				if(hit.collider.gameObject.tag == "cell"){
					hit.collider.gameObject.GetComponent<CellMover>().health -= 2.5f;
				}
				break;
			}
		}
		if (hit.collider != null && !hit.collider.isTrigger) {
			for (int c = 0; c<10; c++){
					GLDebug.DrawLine(transform.position+new Vector3(1,0,0)+ (Vector3.up*0.015f*c),
					                 hit.point+new Vector2((Vector2.up*0.015f*c).x, (Vector2.up*0.015f*c).y), Color.magenta);
					GLDebug.DrawLine(transform.position+new Vector3(1,0,0) -(Vector3.up*0.015f*c),
					                 hit.point-new Vector2((Vector2.up*0.015f*c).x, (Vector2.up*0.015f*c).y), Color.magenta);
			}
		}
		hits = Physics2D.RaycastAll(transform.position, -Vector2.right, 100);
		for(int i = 1; i< hits.Length; i++){
			if(!hits[i].collider.isTrigger){
				hit = hits[i];
				if(hit.collider.gameObject.tag == "cell"){
					hit.collider.gameObject.GetComponent<CellMover>().health -= 2.5f;
				}
				break;
			}
		}
		if (hit.collider != null && !hit.collider.isTrigger) {
			for (int c = 0; c<10; c++){
					GLDebug.DrawLine(transform.position+new Vector3(-1,0,0)+(-Vector3.up*0.015f*c),
				                 hit.point+new Vector2((-Vector2.up*0.015f*c).x, (-Vector2.up*0.015f*c).y), Color.magenta);
					GLDebug.DrawLine(transform.position+new Vector3(-1,0,0) -(-Vector3.up*0.015f*c),
				                 hit.point-new Vector2((-Vector2.up*0.015f*c).x, (-Vector2.up*0.015f*c).y), Color.magenta);
			}
		}
			yield return new WaitForSeconds(secondsBreak);
	}
	}
}	