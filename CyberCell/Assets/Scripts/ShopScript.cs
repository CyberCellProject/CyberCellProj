using UnityEngine;
using System.Collections;

public class ShopScript : MonoBehaviour {
	public GameObject ShopPanel;
	// Use this for initialization
	void Start () {
		ShopPanel = GameObject.FindGameObjectWithTag ("ShopPanel");
		ShopPanel.SetActive(false);
	}
	
	// Update is called once per frame

	void OnTriggerEnter2D(Collider2D trig){
		if(trig.gameObject.tag == "cell"){
			ShopPanel.SetActive(true);
		}
	}
	void OnTriggerExit2D(Collider2D trig){
		if(trig.gameObject.tag == "cell"){
			ShopPanel.SetActive(false);
		}
	}

	void OnDestroy(){
		ShopPanel.SetActive(true);
	}
}
