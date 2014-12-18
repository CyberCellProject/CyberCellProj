using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CellMover : MonoBehaviour
{
	public bool GodMode;
	public float movementSpeed = 5f;
	Quaternion StartRotation;
	Vector3 movement;
	public CNAbstractController MovementJoystick;
	public float RotationSpeed;
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	public int framesPerSecond;
	public float health;
	public GameObject room;
	GameObject[] rooms;
	int findedRoom = 1;
	public float level;
	public Slider healthSlider;
	public Slider energySlider;
	public Text levelText;
	public int medKitValue;
	public float energy;
	public float maxEnergy;
	public float maxHealth;
	public int accelerationForce;
	public bool isBigLaserFiring;
	RaycastHit2D hit;
	public float BigShotDuration;
	float k;
	bool FirstFrameShooting = true;
	public GameObject EndOfLaser;
	GameObject EndOfLaserClone;
	public float defense;

	void Start () {
		healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();
		healthSlider.maxValue = maxHealth;
		energySlider = GameObject.Find("energySlider").GetComponent<Slider>();
		energySlider.maxValue = maxEnergy;
		MovementJoystick = GameObject.FindGameObjectWithTag ("movementJoy").GetComponent<CNAbstractController>();
		levelText = GameObject.FindGameObjectWithTag ("levelText").GetComponent<Text>();
		level = PlayerPrefs.GetFloat ("level");
		if (level <= 0) {
			level = 1;
			PlayerPrefs.SetFloat ("level", level);
		}

		spriteRenderer = renderer as SpriteRenderer;
		health = maxHealth;
		energy = maxEnergy;
	}

    
    void Update()
    {	

		if (health > maxHealth) health = maxHealth;
		if (energy > maxEnergy) energy = maxEnergy;
		energySlider.value = energy;
		healthSlider.value = health;
		level = PlayerPrefs.GetFloat ("level");
		if (findedRoom == 1) {
			findedRoom = 2;
			rooms = GameObject.FindGameObjectsWithTag ("room");
			rooms = GameObject.FindGameObjectsWithTag ("room");
			for (int i= 0; i<rooms.Length; i++) {
				if (rooms[i].name == "StartRoom(Clone)") {
					transform.position = rooms[i].transform.position;
					room = rooms[i];
					break;
				}
			}
		}
		if (findedRoom == 2) {
		rooms = GameObject.FindGameObjectsWithTag ("room");
		if(rooms.Length!=0){
		for (int i= 0; i<rooms.Length; i++) {
			if (Mathf.Abs (rooms [i].transform.position.x - transform.position.x) < rooms [i].GetComponent<RoomScript> ().width &&
				Mathf.Abs (rooms [i].transform.position.y - transform.position.y) < rooms [i].GetComponent<RoomScript> ().height) {
				room = rooms [i];
				break;
			}
					if(i == rooms.Length-1&&GameObject.Find("Transaction Camera(Clone)")== null){
					for (int b= 0; b<rooms.Length; b++) {
						if (rooms[b].name == "StartRoom(Clone)") {
							transform.position = rooms[b].transform.position;
							room = rooms[b];
							break;
						}
					}
				}
			}
		}
		movement = new Vector3 (MovementJoystick.GetAxis ("Horizontal"), MovementJoystick.GetAxis ("Vertical"), 0f);
		#if UNITY_EDITOR
			movement = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0f);
		#endif
		rigidbody2D.AddForce (movement * movementSpeed * Time.deltaTime * 60); 
		if (movement.sqrMagnitude != 0) {
			transform.rotation = Quaternion.Lerp (transform.rotation,
		    Quaternion.LookRotation (Vector3.forward, movement),
		    Time.deltaTime * RotationSpeed);
			}
		if (rigidbody2D.velocity.sqrMagnitude > 0.01f) {
			int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
			index = index % sprites.Length;
			spriteRenderer.sprite = sprites [index];
		}
		if (health <= 0&&!GodMode) {
			Application.LoadLevel ("Menu");
			PlayerPrefs.SetInt("DidLosed", 1);
			PlayerPrefs.SetFloat ("level", 1);
			PlayerPrefs.SetInt ("HasSeporator", 0);
			PlayerPrefs.SetInt ("HasDamagator", 0);
			PlayerPrefs.SetInt ("NumberOfDNA", 0);

		}
		}
		levelText.text = level.ToString ();

/////////////////////////////////////////////////////////////BIGLASER////////////////////////////////////////////////////////////////////////////////////////////

		if (isBigLaserFiring) {
								k += Time.deltaTime;
			if (k >= BigShotDuration) {
					isBigLaserFiring = false;
					k = 0;
			}
			if (k >= 1f) {
				GameObject gun = GameObject.FindGameObjectWithTag ("MyGun");
				Quaternion rotation = gun.transform.rotation;
				Vector3 position = gun.transform.position;
				RaycastHit2D[] hits = Physics2D.RaycastAll (position, rotation * Vector2.up, 100);
				for (int i = 1; i< hits.Length; i++) {
					if (!hits [i].collider.isTrigger && hits [i].collider.gameObject.tag != "barrier" && hits [i].collider.gameObject.tag != "dynamicBarrier"&&hits[i].collider.gameObject.tag != "drop") {
						hit = hits [i];
							if (hit.collider.gameObject.tag == "enemy") {
								hit.collider.gameObject.GetComponent<EnemyBehaviour> ().health -= 10;
							}
					break;
					}
				}	
				if (hit.collider != null && !hit.collider.isTrigger) {
					if(FirstFrameShooting){
						EndOfLaserClone = Instantiate(EndOfLaser, hit.point,  Quaternion.LookRotation(hit.point-new Vector2(transform.position.x, transform.position.y))) as GameObject;
						EndOfLaserClone.GetComponent<SpriteRenderer>().color = Color.magenta;
					}
					EndOfLaserClone.transform.position = hit.point;
					EndOfLaserClone.transform.rotation= Quaternion.LookRotation(Vector3.forward ,hit.point - new Vector2(transform.position.x, transform.position.y));
					for (int c = 0; c<5; c++) {
						GLDebug.DrawLine (position + (rotation * Vector3.right * 0.015f * c),
				 		   		         hit.point + new Vector2 ((rotation * Vector2.right * 0.015f * c).x, (rotation * Vector2.right * 0.015f * c).y), Color.magenta);
						GLDebug.DrawLine (position - (rotation * Vector3.right * 0.015f * c),
						                 hit.point - new Vector2 ((rotation * Vector2.right * 0.015f * c).x, (rotation * Vector2.right * 0.015f * c).y), Color.magenta);
					}
				}
				FirstFrameShooting = false;

			}
		}
		else{
			FirstFrameShooting = true;
			Destroy(EndOfLaserClone);
		}
		//	StartCoroutine (GameObject.FindGameObjectWithTag("MyGun").GetComponent<GunScript>().StartCoroutine (MachinGun()));
	}


	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.name == "MedKit(Clone)"){
			if(health< maxHealth){
				health+=medKitValue;
				Destroy(coll.gameObject);
			}
		}
		else if(coll.gameObject.name == "meat(Clone)"){
			if(energy < maxEnergy){
				energy+=10;
				Destroy(coll.gameObject);
			}
		}
		else if(coll.gameObject.name == "DNA(Clone)"){
			if(PlayerPrefs.GetInt("NumberOfDNA") < 999){
				PlayerPrefs.SetInt(("NumberOfDNA"),(PlayerPrefs.GetInt("NumberOfDNA"))+1);
				if(UnityEngine.Random.Range(0, 20)==0){
					GameObject.Find("MiniMapCam").GetComponent<MiniMapScript>().Omnomnom ("Omnomnom", 1f);
				}
				Destroy(coll.gameObject);
			}
		}
		else if(coll.gameObject.name == "greenMark(Clone)"){
			GameObject.FindGameObjectWithTag("CameraParent").GetComponent<CameraMove>().lsdBool = true;
			Destroy(coll.gameObject);

		}
	}

	public void InstantiateGameobject(GameObject item){
		Instantiate(item, transform.position, Quaternion.Euler(0, 0, 0));
	}
}