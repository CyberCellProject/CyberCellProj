using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	public float range;
	Vector3 start;
	public bool isEnemy;
	public float damage;
	public bool isThisEnemy = true;
	int prevColl;
	float index;
	float indexDamage;
	public Sprite[] sprites;
	public Sprite[] spritesDamage;
	SpriteRenderer spriteRenderer;
	public int framesPerSecond;
	bool isDestroying;
	public int damageFramesPerSecond;
	public bool isThorn;
	float currDist;
	public float speed;
	bool dir = true;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		start = transform.position;
		prevColl = collider2D.GetInstanceID ();
		if (isThorn) {
			StartCoroutine(Thorn());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ((start - transform.position).sqrMagnitude > range&&!isThorn) {
			index += Time.deltaTime * framesPerSecond;
			spriteRenderer.sprite = sprites [(int)(index)];
			if (index >= sprites.Length-1) {
				Destroy (this.gameObject);
			}

		}
		if(isDestroying&&!isThorn){
			indexDamage += Time.deltaTime * damageFramesPerSecond;
			spriteRenderer.sprite = spritesDamage [(int)(indexDamage)];
			if (indexDamage >= spritesDamage.Length-1) {
				Destroy (this.gameObject);
			}
		}
	}
	void OnTriggerEnter2D(Collider2D trig){
		if (trig.gameObject.tag == "enemy" && !isEnemy) {
			isDestroying = true;
			rigidbody2D.velocity = new Vector2(0, 0);
			dir = false;
			trig.rigidbody2D.AddForceAtPosition((transform.rotation * Vector3.up).normalized*0.5f, transform.position,  ForceMode2D.Impulse);
			trig.gameObject.GetComponent<EnemyBehaviour> ().health -= damage;
		} else if (trig.gameObject.tag == "cell" && isEnemy) {
			isDestroying = true;
			rigidbody2D.velocity = new Vector2(0, 0);
			dir = false;
			trig.rigidbody2D.AddForceAtPosition((transform.rotation * Vector3.up).normalized*0.5f, transform.position,  ForceMode2D.Impulse);
			trig.gameObject.GetComponent<CellMover> ().health -= damage*(1-trig.gameObject.GetComponent<CellMover>().defense);
		} else if (!trig.isTrigger && trig.gameObject.tag != "cell" && (!isThisEnemy||!isEnemy)) {
			if(trig.gameObject.tag == "dynamicBarrier" ) trig.gameObject.rigidbody2D.AddForceAtPosition((transform.rotation * Vector3.up).normalized, transform.position,  ForceMode2D.Impulse);
			if(trig.gameObject.tag!="drop"){
				isDestroying = true;
				dir = false;
				rigidbody2D.velocity = new Vector2(0, 0);
			}
		}
		}

	void OnTriggerStay2D(Collider2D trig)
	{
		if (trig.GetInstanceID()!=prevColl||trig.gameObject.tag!="enemy"||trig.gameObject.tag!="cell") {
			isThisEnemy = false;
		}

	}
	IEnumerator Thorn(){
		while(true){
			GameObject gun = GameObject.FindWithTag("MyGun");
			transform.position = gun.transform.rotation*Vector3.up*currDist + gun.transform.position;
			transform.rotation = gun.transform.rotation;
			GLDebug.DrawLine(transform.position,gun.transform.position+ transform.rotation*(new Vector3(0, 0.05f, 0)), Color.black);
			GLDebug.DrawLine(transform.position+(transform.rotation*Vector3.right*0.015f),gun.transform.position+ transform.rotation*(new Vector3(0, 0.05f, 0))+(transform.rotation*Vector3.right*0.015f), Color.black);
			GLDebug.DrawLine(transform.position-(transform.rotation*Vector3.right*0.015f),gun.transform.position+ transform.rotation*(new Vector3(0, 0.05f, 0))-(transform.rotation*Vector3.right*0.015f), Color.black);

			if(currDist<range&&dir){
				currDist += Time.deltaTime * speed;
			}
			else{
				dir = false;
				currDist -= Time.deltaTime * speed *5;
				if(currDist<=0.1f){
					Destroy (this.gameObject);
					gun.GetComponent<GunScript>().isntCD = true;
				}
			}
			yield return 0;
		}
	}
}
