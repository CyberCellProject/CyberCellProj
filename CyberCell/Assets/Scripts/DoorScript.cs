using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {
	public bool closed = true;
	public bool isSomethingInDoors;
	Vector3 prevPos;
	public GameObject transAction;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(this.gameObject.tag == "door"||this.gameObject.tag == "bigDoor"){
			if (!closed) {
				this.collider2D.isTrigger = true;
			}
			else if(!isSomethingInDoors)
				this.collider2D.isTrigger = false;
		}
		else
			this.collider2D.isTrigger = false;
	}

	void OnTriggerEnter2D(Collider2D trig){
		if(trig.gameObject.tag == "cell"){
			isSomethingInDoors = true;
		}
	}

	void OnTriggerStay2D(Collider2D trig){
		if(trig.gameObject.tag == "cell"){
			if(closed){
				if(prevPos.sqrMagnitude == 0)
					prevPos = trig.transform.position;
				if(this.transform.parent.gameObject.name == "Top"&&trig.transform.position.y>prevPos.y){
					trig.transform.position = new Vector3(trig.transform.position.x, prevPos.y, trig.transform.position.z);
				}
				if(this.transform.parent.gameObject.name == "Bottom"&&trig.transform.position.y<prevPos.y){
					trig.transform.position = new Vector3(trig.transform.position.x, prevPos.y, trig.transform.position.z);
				}
				if(this.transform.parent.gameObject.name == "Left"&&trig.transform.position.x<prevPos.x){
					trig.transform.position = new Vector3(prevPos.x, trig.transform.position.y, trig.transform.position.z);
				}
				if(this.transform.parent.gameObject.name == "Right"&&trig.transform.position.x>prevPos.x){
					trig.transform.position = new Vector3(prevPos.x, trig.transform.position.y, trig.transform.position.z);
				}
				prevPos = trig.transform.position;
			}
		}
	}

	void OnTriggerExit2D(Collider2D trig){
		if(trig.gameObject.tag == "cell"){
			if(tag == "bigDoor"){			
				Instantiate(transAction, new Vector3(1000, 1000, -1), Quaternion.Euler(0,0,0));
			}
			isSomethingInDoors = false;
		}
	}
}
