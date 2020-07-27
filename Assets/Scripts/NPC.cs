using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
  private float speedy, speedx, timer = 0f, xDirection, delta = 1f,distance = 0f;
	public GameManager manager;
	// Start is called before the first frame update
	void Start()
	{
		speedy = Random.Range(0.02f,0.06f);
		speedx = Random.Range(0.03f,0.05f);
		transform.position = new Vector3(Random.Range(-3f,3f),15f,0);
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		if(timer >= delta){
			xDirection = Random.Range(-1f,1f);
			delta = Random.Range(0.75f,1.25f);
			timer = 0f;
		}
		Move();	
	}

	public void Reset() {
		transform.position = new Vector3(Random.Range(-10f,10f),15f,0);
		transform.localScale = new Vector3(0.2f, 0.2f, 0);

		this.gameObject.tag = "Healthy";
		this.gameObject.GetComponent<Collider2D>().isTrigger = false;
		this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

		Destroy(this.gameObject.GetComponent<Rigidbody2D>());
	}

	private void ScalePlayer(float scaling) {
		float scalingFactor = 0.065f * scaling;
		Vector3 currentScale = transform.localScale;
		Vector3 nextScale = new Vector3(0.2f * scalingFactor, 0.2f * scalingFactor, 0);
		transform.localScale += nextScale;
	}

  private void Move() {
		if(this.transform.position.x > 10 || this.transform.position.x < -10){
			xDirection = xDirection * -1;
		}

		if(this.transform.position.y < -20){
			manager.AddPerson(this.gameObject);
			return;
		}

		float x = xDirection * speedx;
		float y = -1 * speedy * (transform.localScale.y/0.3f);
		transform.Translate(new Vector3(x,y,0));
		ScalePlayer(System.Math.Abs(y));
  }

  private void OnTriggerEnter2D(Collider2D other) {
		GameObject them = other.gameObject;


    if((them.tag == "Infected" || them.tag == "Player") && this.gameObject.tag == "Healthy") {
			this.gameObject.tag = "Infected";
			this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
			this.gameObject.GetComponent<Collider2D>().isTrigger = true;
			this.gameObject.AddComponent<Rigidbody2D>();
			this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			this.gameObject.GetComponent<Rigidbody2D>().mass = 0.0001f;
			manager.infected++;
		}
  }
}
