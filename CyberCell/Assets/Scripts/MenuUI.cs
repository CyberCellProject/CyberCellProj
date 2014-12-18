using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {
	public GameObject buttonsPanel;
	public GameObject optionsPanel;
	public GameObject playerChoosePanel;
	public GameObject bioticChoosePanel;
	public GameObject cyborgChoosePanel;
	public GameObject nanobotChoosePanel;
	public GameObject youLosePanel;
	public float timer;
	public int rotSpeed;
	// Use this for initialization
	void Start () {
	 	if (PlayerPrefs.GetInt ("DidLosed") == 1) {
			youLosePanel.SetActive(true);
			buttonsPanel.SetActive(false);
			PlayerPrefs.SetInt ("DidLosed", 0);
		} 
		else {
			youLosePanel.SetActive(false);
			buttonsPanel.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonClicks(string name){
		switch (name) {
		case "Play":
			playerChoosePanel.SetActive(true);
			buttonsPanel.SetActive(false);
			//			Application.LoadLevel ("main");
			break;
		case "Options":
			//buttonsPanel.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 90, 0); 
			buttonsPanel.SetActive(false);
			break;
		case "Exit":
			Application.Quit();
			break;
		case "Menu":
			playerChoosePanel.SetActive(false);
			cyborgChoosePanel.SetActive(false);
			bioticChoosePanel.SetActive(false);
			nanobotChoosePanel.SetActive(false);
			buttonsPanel.SetActive(true);
			break;
		case "Replay":
			playerChoosePanel.SetActive(true);
			youLosePanel.SetActive(false);	
			break;
		case "Biotic":
			bioticChoosePanel.SetActive(true);
			playerChoosePanel.SetActive(false);
			break;
		case "Cyborg":
			cyborgChoosePanel.SetActive(true);
			playerChoosePanel.SetActive(false);
			//StartCoroutine(RotatePanel(90f, playerChoosePanel));
			break;
		case "Nanobot":
			nanobotChoosePanel.SetActive(true);
			playerChoosePanel.SetActive(false);
			break;
		case "Biotic1":
			PlayerPrefs.SetString("PlayerPers", "Biotic1");
			Application.LoadLevel("main");
			break;
		case "Biotic2":
			PlayerPrefs.SetString("PlayerPers", "Biotic2");
			Application.LoadLevel("main");
			break;
		case "Biotic3":
			PlayerPrefs.SetString("PlayerPers", "Biotic3");
			Application.LoadLevel("main");
			break;
		case "Cyborg1":
			PlayerPrefs.SetString("PlayerPers", "Cyborg1");
			Application.LoadLevel("main");
			break;
		case "Cyborg2":
			PlayerPrefs.SetString("PlayerPers", "Cyborg2");
			Application.LoadLevel("main");
			break;
		case "Cyborg3":
			PlayerPrefs.SetString("PlayerPers", "Cyborg3");
			Application.LoadLevel("main");
			break;
		case "Nanobot1":
			PlayerPrefs.SetString("PlayerPers", "Nanobot1");
			Application.LoadLevel("main");
			break;
		case "Nanobot2":
			PlayerPrefs.SetString("PlayerPers", "Nanobot2");
			Application.LoadLevel("main");
			break;
		case "Nanobot3":
			PlayerPrefs.SetString("PlayerPers", "Nanobot3");
			Application.LoadLevel("main");
			break;
		case "TwoJoys":
			PlayerPrefs.SetString("Controls", "TwoJoys");
			break;
		case "JoyAndTouch":
			PlayerPrefs.SetString("Controls", "JoyAndTouch");
			break;
		default:
			Debug.Log("Wrong button string");
			break;
		}
	}
	IEnumerator RotatePanel(float angle, GameObject panel){
		if (angle == 90) {
			timer = 0;
				}
		else {
			timer = -90;
		}
		for (timer +=0 ; timer <= angle; timer += Time.deltaTime*rotSpeed) {
			if(Mathf.Abs(angle-timer)<=2){
				timer = angle;
				panel.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, timer, 0); 
				return false;
			}
			panel.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, timer, 0); 
			yield return 0;
		}
	}
}
