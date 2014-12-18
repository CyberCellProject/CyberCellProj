using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {
	public float lengthOfGun;
	public float force;
	public GameObject bullet;
	public float freq;
//	public float timer;
	GameObject clone;
	public CNAbstractController rotationJoystick;
	Vector3 rotation;
	public float RotationSpeed;
	GameObject player;
	public bool smartFire;
	public float k;
	public string TypeOfGun;
	// Laser
	RaycastHit2D hit;
	public float damage;
	public int thickness;
	public string laserColor;
	Color color;
	public GameObject EndOfLaser;
	GameObject EndOfLaserClone;
	bool FirstFrameShooting = true;
	public int numberOfBullets = 5;
	int p;
	public bool hasSeporator;
	//ShotGun
	public float angleOfSpreading;
	public float seporatorSpreading;
	GameObject cloneCopy;
	public bool hasPrism;
	bool prismJustSet;
	float randomTemp;
	int controlsType;
	Rect moveJoyRect;
	int currTouchID;
	public Touch currTouch;
	bool nullRotation;
	public bool isntCD = true;
	public bool hasDamagator;
	//public float cd;
	// Use this for initialization
	void Start () {
		if (TypeOfGun == "MachineGun") {
			StartCoroutine (MachinGun(freq));
		}
		else if(TypeOfGun == "Laser") {
			if (laserColor == "red") {
				color = Color.red;	
			} else if (laserColor == "green") {
				color = Color.green;	
			} else if (laserColor == "yellow") {
				color = Color.yellow;	
			} else if (laserColor == "blue") {
				color = Color.blue;	
			} else
				color = Color.green;
			StartCoroutine (Laser());
		}
		if (TypeOfGun == "ShotGun") {
			StartCoroutine (ShotGun(freq));
		}
		if (TypeOfGun == "Harpoon") {
			StartCoroutine (Harpoon());	
		}
		player = GameObject.FindGameObjectWithTag ("cell");
		if(tag == "MyGun"){
				if (PlayerPrefs.GetString ("Controls") == "TwoJoys") {
						rotationJoystick = GameObject.FindGameObjectWithTag ("rotationJoy").GetComponent<CNAbstractController> ();
						controlsType = 1;	
				} else if (PlayerPrefs.GetString ("Controls") == "JoyAndTouch") {
						controlsType = 2;
						if(GameObject.FindWithTag ("rotationJoy")!=null)
							GameObject.FindWithTag ("rotationJoy").SetActive (false);
				} else {
					controlsType = 1;
					rotationJoystick = GameObject.FindGameObjectWithTag ("rotationJoy").GetComponent<CNAbstractController>();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerPrefs.GetInt ("HasSeporator") == 1 && tag == "MyGun") {
			hasSeporator = true;	
		}
		if (PlayerPrefs.GetInt ("HasPrism") == 1 && tag == "MyGun") {
			hasPrism = true;	
		}
		if (PlayerPrefs.GetInt ("HasDamagator") == 1 && tag == "MyGun") {
			hasDamagator = true;	
		}
		if(this.gameObject.tag == "MyGun"){
			if(controlsType ==1){
				rotation = new Vector3(rotationJoystick.GetAxis("Horizontal"),rotationJoystick.GetAxis("Vertical"), 0f);
				if(rotation.sqrMagnitude>0){
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, rotation), Time.deltaTime*RotationSpeed);
				}
			}
			else if(controlsType == 2){
				if(Input.touchCount>0){
					for(int l = 0; l<Input.touchCount; l++){
						if(Input.GetTouch(l).phase == TouchPhase.Began &&
						   ((Input.GetTouch(l).position.x)>Screen.width*0.3||(Input.GetTouch(l).position.y)>Screen.height*0.3) ){
							currTouchID = Input.GetTouch (l).fingerId;
					//		nullRotation = false;

						}
						else if((Input.GetTouch(l).phase == TouchPhase.Moved||Input.GetTouch(l).phase == TouchPhase.Stationary)&&Input.GetTouch(l).fingerId == currTouchID){
							currTouch = Input.GetTouch (l);
							nullRotation = false;
							break;

						}
						else if(Input.GetTouch(l).phase == TouchPhase.Ended&&Input.GetTouch(l).fingerId==currTouchID){
							currTouchID = -1;
							nullRotation = true;
						}
						else{
							nullRotation = true;						}
					}
					rotation = Vector3.zero;
					if(!nullRotation)
						rotation = new Vector3(currTouch.position.x, currTouch.position.y)  - Camera.main.WorldToScreenPoint(transform.position);
					if(rotation.sqrMagnitude>0.01f)
						transform.rotation = Quaternion.Euler (new Vector3(0,0,Mathf.Atan2 (rotation.y,rotation.x) * Mathf.Rad2Deg-90f));
				}
				else{
					rotation = Vector3.zero;
				}
			}

			#if UNITY_EDITOR
			rotation = new Vector3(Input.GetAxis("Left-Right"),Input.GetAxis("Up-Down"), 0f);
			if(rotation.sqrMagnitude>0){
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, rotation), Time.deltaTime*RotationSpeed);
			}
			#endif
		}
		else{
			if(smartFire){
				rotation =force*k*(player.transform.position-transform.position).magnitude* new Vector3(player.rigidbody2D.velocity.x, player.rigidbody2D.velocity.y, 0)  + player.transform.position - transform.position;
			}
			else
				rotation = player.transform.position - transform.position;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, rotation), Time.deltaTime*RotationSpeed);
		}
	}

	public IEnumerator MachinGun(float freq){
		while (true) {
			if (((rotation.sqrMagnitude > 0 && !player.GetComponent<CellMover> ().isBigLaserFiring && tag == "MyGun") ||
				((this.gameObject.tag != "MyGun") && GetComponentInParent<EnemyBehaviour> ().seePlayer))) {
				clone = Instantiate (bullet, transform.rotation*(new Vector3(0, lengthOfGun, 0))+transform.position, transform.rotation) as GameObject;
				clone.rigidbody2D.AddForce ((transform.rotation * Vector3.up).normalized * force, ForceMode2D.Impulse);
				if (this.gameObject.tag != "MyGun")
					clone.GetComponent<BulletScript> ().isEnemy = true;
				if(hasDamagator&&Random.Range(0, 9)==0){
					clone.transform.localScale = clone.transform.localScale*2;
					clone.GetComponent<BulletScript> ().damage *=2f;		
				}
				if(hasSeporator){
					clone.rigidbody2D.AddForce((transform.rotation * Vector3.right).normalized * seporatorSpreading, ForceMode2D.Impulse);
					clone.transform.localScale = clone.transform.localScale/2;
					clone.GetComponent<BulletScript> ().damage /=1.5f;
					clone.GetComponent<BulletScript> ().range *=0.9f;
					cloneCopy = Instantiate (clone, transform.rotation*(new Vector3(0, lengthOfGun, 0))+transform.position, transform.rotation) as GameObject;
					cloneCopy.rigidbody2D.AddForce ((transform.rotation * Vector3.up).normalized * force + (transform.rotation * Vector3.left).normalized * seporatorSpreading, ForceMode2D.Impulse);

				}

			}
			yield return new WaitForSeconds (10/ freq);
				}
	}
	public IEnumerator Laser(){
		while (true) {
			if((rotation.sqrMagnitude>0&&tag == "MyGun")&&((this.gameObject.tag != "MyGun"&&GetComponentInParent<EnemyBehaviour>().seePlayer)||!player.GetComponent<CellMover>().isBigLaserFiring)){
				RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.rotation*Vector2.up, 100);
				for(int i = 1; i< hits.Length; i++){
					if(!hits[i].collider.isTrigger&&hits[i].collider.gameObject.tag != "drop"){
						hit = hits[i];
						if(hit.collider.gameObject.tag == "enemy"){
							if(hasDamagator&&Random.Range(0, 9)==0){
								hit.collider.gameObject.GetComponent<EnemyBehaviour>().health -= damage;
							}
							hit.collider.gameObject.GetComponent<EnemyBehaviour>().health -= damage;
						}
						else if(hit.collider.gameObject.tag == "cell" && tag != "MyGun"){
							if(hasDamagator&&Random.Range(0, 9)==0){
								hit.collider.gameObject.GetComponent<CellMover>().health -= damage*(1-hit.collider.gameObject.GetComponent<CellMover>().defense);
							}
							hit.collider.gameObject.GetComponent<CellMover>().health -= damage*(1-hit.collider.gameObject.GetComponent<CellMover>().defense);
						}
						break;
					}
				}
				
				if (hit.collider != null && !hit.collider.isTrigger) {
					if(FirstFrameShooting){
						EndOfLaserClone = Instantiate(EndOfLaser, hit.point,  Quaternion.LookRotation(hit.point-new Vector2(transform.position.x, transform.position.y))) as GameObject;
						EndOfLaserClone.GetComponent<SpriteRenderer>().color = color;
					}
					EndOfLaserClone.transform.position = hit.point;
					EndOfLaserClone.transform.rotation= Quaternion.LookRotation(Vector3.forward ,hit.point - new Vector2(transform.position.x, transform.position.y));
					for (int c = 0; c<thickness; c++){
						GLDebug.DrawLine(transform.position+ transform.rotation*(new Vector3(0, lengthOfGun, 0))+(transform.rotation*Vector3.right*0.015f*c),
						                 hit.point+new Vector2((transform.rotation*Vector2.right*0.015f*c).x, (transform.rotation*Vector2.right*0.015f*c).y), color);
						GLDebug.DrawLine(transform.position + transform.rotation*(new Vector3(0, lengthOfGun, 0))-(transform.rotation*Vector3.right*0.015f*c),
						                 hit.point-new Vector2((transform.rotation*Vector2.right*0.015f*c).x, (transform.rotation*Vector2.right*0.015f*c).y), color);
					}
				}
/////////////Prisma//////////////
				if(hasPrism){
					if(!prismJustSet){
						damage/=3;
						prismJustSet = true;
						thickness = 1;
					}
					hits = Physics2D.RaycastAll(transform.position, transform.rotation*Quaternion.Euler (0,0,Random.Range(0f, seporatorSpreading))*Vector2.up, 100);
					for(int i = 1; i< hits.Length; i++){
						if(!hits[i].collider.isTrigger&&hits[i].collider.gameObject.tag != "drop"){
							hit = hits[i];
							if(hit.collider.gameObject.tag == "enemy"){
								hit.collider.gameObject.GetComponent<EnemyBehaviour>().health -= damage;
							}
							else if(hit.collider.gameObject.tag == "cell" && tag != "MyGun"){
								hit.collider.gameObject.GetComponent<CellMover>().health -= damage;
							}
							break;
						}
					}
					if (hit.collider != null && !hit.collider.isTrigger) {
						for (int c = 0; c<thickness; c++){
							GLDebug.DrawLine(transform.position+ transform.rotation*(new Vector3(0, lengthOfGun, 0))+(transform.rotation*Vector3.right*0.015f*c),
							                 hit.point+new Vector2((transform.rotation*Vector2.right*0.015f*c).x, (transform.rotation*Vector2.right*0.015f*c).y), color);
							GLDebug.DrawLine(transform.position + transform.rotation*(new Vector3(0, lengthOfGun, 0))-(transform.rotation*Vector3.right*0.015f*c),
							                 hit.point-new Vector2((transform.rotation*Vector2.right*0.015f*c).x, (transform.rotation*Vector2.right*0.015f*c).y), color);
						}
					}
					hits = Physics2D.RaycastAll(transform.position, transform.rotation*Quaternion.Euler (0,0,Random.Range(-seporatorSpreading, 0f))*Vector2.up, 100);
					for(int i = 1; i< hits.Length; i++){
						if(!hits[i].collider.isTrigger&&hits[i].collider.gameObject.tag != "drop"){
							hit = hits[i];
							if(hit.collider.gameObject.tag == "enemy"){
								hit.collider.gameObject.GetComponent<EnemyBehaviour>().health -= damage;
							}
							else if(hit.collider.gameObject.tag == "cell" && tag != "MyGun"){
								hit.collider.gameObject.GetComponent<CellMover>().health -= damage*(1-hit.collider.gameObject.GetComponent<CellMover>().defense);
							}
							break;
						}
					}
					
					if (hit.collider != null && !hit.collider.isTrigger) {
						for (int c = 0; c<thickness; c++){
							GLDebug.DrawLine(transform.position+ transform.rotation*(new Vector3(0, lengthOfGun, 0))+(transform.rotation*Vector3.right*0.015f*c),
							                 hit.point+new Vector2((transform.rotation*Vector2.right*0.015f*c).x, (transform.rotation*Vector2.right*0.015f*c).y), color);
							GLDebug.DrawLine(transform.position + transform.rotation*(new Vector3(0, lengthOfGun, 0))-(transform.rotation*Vector3.right*0.015f*c),
							                 hit.point-new Vector2((transform.rotation*Vector2.right*0.015f*c).x, (transform.rotation*Vector2.right*0.015f*c).y), color);
						}
					}
				}
				FirstFrameShooting = false;
			}
			else{
				FirstFrameShooting = true;
				Destroy(EndOfLaserClone);
			}
			yield return 0;
		}
	}
	public IEnumerator ShotGun(float freq){
		while (true) {
			if (((rotation.sqrMagnitude >0 && !player.GetComponent<CellMover> ().isBigLaserFiring && tag == "MyGun") ||
			     ((this.gameObject.tag != "MyGun") && GetComponentInParent<EnemyBehaviour> ().seePlayer))) {
				for(p = 0; p<numberOfBullets; p++){
					clone = Instantiate (bullet, transform.rotation*(new Vector3(0, lengthOfGun, 0))+transform.position, transform.rotation) as GameObject;
					clone.rigidbody2D.AddForce ((transform.rotation*Quaternion.Euler(0, 0, Random.Range(-angleOfSpreading, angleOfSpreading)) * Vector3.up).normalized * force, ForceMode2D.Impulse);
					if (this.gameObject.tag != "MyGun")
						clone.GetComponent<BulletScript> ().isEnemy = true;
					if(hasDamagator&&Random.Range(0, 9)==0){
						clone.transform.localScale = clone.transform.localScale*2;
						clone.GetComponent<BulletScript> ().damage *=2f;		
					}
					if(hasSeporator){
						clone.rigidbody2D.AddForce((transform.rotation * Vector3.right).normalized * seporatorSpreading, ForceMode2D.Impulse);
						clone.transform.localScale = clone.transform.localScale/2;
						clone.GetComponent<BulletScript> ().damage /=2;
						cloneCopy = Instantiate (clone, transform.rotation*(new Vector3(0, lengthOfGun, 0))+transform.position, transform.rotation) as GameObject;
						cloneCopy.rigidbody2D.AddForce ((transform.rotation * Vector3.up).normalized * force + (transform.rotation * Vector3.left).normalized * seporatorSpreading, ForceMode2D.Impulse);
						
					}
				}
			}
			yield return new WaitForSeconds (10/ freq);
		}
	}
	public IEnumerator Harpoon(){
		while (true) {
			if (((rotation.sqrMagnitude >0 && !player.GetComponent<CellMover> ().isBigLaserFiring && tag == "MyGun") ||
			   ((this.gameObject.tag != "MyGun") && GetComponentInParent<EnemyBehaviour> ().seePlayer))) {
				if(isntCD){
					clone = Instantiate (bullet, transform.rotation*(new Vector3(0, lengthOfGun, 0))+transform.position, transform.rotation) as GameObject;
					isntCD = false;
					if(hasDamagator&&Random.Range(0, 9)==0){
						clone.transform.localScale = clone.transform.localScale*2;
						clone.GetComponent<BulletScript> ().damage *=2f;		
					}
				}

			}
			yield return 0;
		}
	}

}
