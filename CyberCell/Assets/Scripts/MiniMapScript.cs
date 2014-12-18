using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniMapScript : MonoBehaviour {
	public GameObject Cell;
	bool isPaused = false;
	public GameObject topLeftPannel;
	public GameObject controlsPanel;
	public Button pauseButton;
	public Sprite BG;
	public Sprite transparentSprite;
	public GameObject buyGun;
	public GameObject buyUpgrade;
	public GameObject seporator;
	public GameObject damagator;
	GameObject gun;
	GameObject clone;
	public GameObject prism;
	GameObject clone1;
	public Text debugtext;
	public Button skill1;
	public Button skill2;
	public Text DNAtext;
	public Text perSpeach;
	public GameObject pausePanel;
	public GameObject upgradePanel;
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetString ("Controls") != "TwoJoys") {
			skill1.GetComponent<RectTransform>().anchorMin = new Vector2( 0.3865279f, 0.03726071f);
			skill1.GetComponent<RectTransform>().anchorMax = new Vector2( 0.6724466f, 0.33f);
			skill2.GetComponent<RectTransform>().anchorMin = new Vector2( 0.6877767f, 0.298f);
			skill2.GetComponent<RectTransform>().anchorMax = new Vector2( 0.9843301f, 0.6006837f);
			
		}

	}
	
	// Update is called once per frame
	void Update () {
		perSpeach.rectTransform.localPosition = new Vector3(Camera.main.WorldToScreenPoint(new Vector3 (transform.position.x, transform.position.y)).x,
		                                               Camera.main.WorldToScreenPoint(new Vector3 (transform.position.x, transform.position.y)).y+80, 0)-
												new Vector3(Screen.width, Screen.height, 0)/2;
	//	perSpeach.text = "";
		DNAtext.text = PlayerPrefs.GetInt ("NumberOfDNA").ToString ();
		gun = GameObject.FindGameObjectWithTag ("MyGun");
		Cell = GameObject.FindGameObjectWithTag ("cell");
		transform.position = new Vector3(Cell.transform.position.x, Cell.transform.position.y, -1);
//		debugtext.text = gun.GetComponent<GunScript>().currTouch.fingerId.ToString ();
		if (Input.GetKeyDown (KeyCode.Q)) {
			if(Cell.GetComponent<CellMover> ().energy >= 0){
				Cell.rigidbody2D.AddForce (Cell.transform.rotation*Vector2.up*Cell.GetComponent<CellMover> ().accelerationForce, ForceMode2D.Impulse);
				Cell.GetComponent<CellMover> ().energy-=0;
			}
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			if(Cell.GetComponent<CellMover> ().energy >= 5 && !Cell.GetComponent<CellMover> ().isBigLaserFiring){
				Cell.GetComponent<CellMover> ().energy-=5;
				Cell.GetComponent<CellMover> ().isBigLaserFiring = true;
			}	
		}


	}

	public void changeGun(GameObject gun){
		GameObject clone = Instantiate(gun, GameObject.FindGameObjectWithTag("MyGun").transform.position, transform.rotation) as GameObject;
		Destroy (GameObject.FindGameObjectWithTag("MyGun"));
		clone.transform.parent = Cell.transform;
		clone.tag = "MyGun";
		buyGun.SetActive (false);
		if (PlayerPrefs.GetInt ("HasSeporator") == 1) {
			clone1 = Instantiate(seporator, gun.transform.position+gun.transform.rotation*Vector3.up*gun.GetComponent<GunScript>().lengthOfGun,
			                    gun.transform.rotation) as GameObject;
			clone1.transform.parent = clone.transform;
		}
		if (PlayerPrefs.GetInt ("HasPrism") == 1) {
			clone1 = Instantiate(prism, gun.transform.position+gun.transform.rotation*Vector3.up*gun.GetComponent<GunScript>().lengthOfGun,
			                    gun.transform.rotation) as GameObject;
			clone1.transform.parent = clone.transform;
		}
	}
	
	public void	OnAnyButtonClick (string name){
		switch (name) {
		case "pause":
			if(!isPaused){
				isPaused = true;
				Time.timeScale = 0;
				pausePanel.SetActive (true);
	//			camera.rect = new Rect(0, 0, Screen.width, Screen.height);
	//			topLeftPannel.SetActive(false);
	//			controlsPanel.SetActive (false);
	//			pauseButton.image.sprite = BG;
	//			pauseButton.transform.FindChild ("Text").GetComponent<Text>().text = "Back";
			}
			else{
				isPaused = false;
				Time.timeScale = 1;
				pausePanel.SetActive (false);

				//			camera.rect = new Rect(-0.75f, 0.75f, 1, 1);
	//			topLeftPannel.SetActive(true);
	//			controlsPanel.SetActive (true);
	//			pauseButton.image.sprite = transparentSprite;
	//			pauseButton.transform.FindChild ("Text").GetComponent<Text>().text = "";
			}
			break;
		case "GodMode":
			Cell.GetComponent<CellMover> ().GodMode = !Cell.GetComponent<CellMover> ().GodMode;
			break;
		case "acceleration":
			if(Cell.GetComponent<CellMover> ().energy >= 10){
				Cell.rigidbody2D.AddForce (Cell.transform.rotation*Vector2.up*Cell.GetComponent<CellMover> ().accelerationForce, ForceMode2D.Impulse);
				Cell.GetComponent<CellMover> ().energy-=10;
			}
			break;
		case "LaserPowerShot":
			if(Cell.GetComponent<CellMover> ().energy >= 30 && !Cell.GetComponent<CellMover> ().isBigLaserFiring){
				Cell.GetComponent<CellMover> ().energy-=30;
				Cell.GetComponent<CellMover> ().isBigLaserFiring = true;
			}
			break;
		case "Seporator":
			clone = Instantiate(seporator, gun.transform.position+gun.transform.rotation*Vector3.up*gun.GetComponent<GunScript>().lengthOfGun,
			                                 gun.transform.rotation) as GameObject;
			clone.transform.parent = gun.transform;
			PlayerPrefs.SetInt ("HasSeporator", 1);
			buyUpgrade.SetActive (false);
			break;
		case "Prism":
			clone = Instantiate(prism, gun.transform.position+gun.transform.rotation*Vector3.up*gun.GetComponent<GunScript>().lengthOfGun,
			                                 gun.transform.rotation) as GameObject;
			clone.transform.parent = gun.transform;
			PlayerPrefs.SetInt ("HasPrism", 1);
			buyUpgrade.SetActive (false);
			break;
		case "Damagator":
			clone = Instantiate(damagator, gun.transform.position+gun.transform.rotation*Vector3.up*gun.GetComponent<GunScript>().lengthOfGun,
			                    gun.transform.rotation) as GameObject;
			clone.transform.parent = gun.transform;
			PlayerPrefs.SetInt ("HasDamagator", 1);
			buyUpgrade.SetActive (false);
			break;
		case "buyGun":
			buyGun.SetActive(true);
			break;
		case "buyUpgrade":
			buyUpgrade.SetActive(true);
			break;
		case "Upgrade":
			upgradePanel.SetActive (true);
			break;
		case "BackFromUpgrades":
			upgradePanel.SetActive (false);
			break;
		default:
			Debug.Log("Wrong button string");
			break;
		}

	}
	public void Omnomnom(string omnom, float time){
		StartCoroutine (Omnomnomer(omnom, time));
	}
	IEnumerator Omnomnomer(string omnomer, float timer){
			perSpeach.text = omnomer;
			yield return new WaitForSeconds(timer);
		perSpeach.text = "";
	}

}
