using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	[SerializeField]
	Sprite normal, infected;
  private float speedy, speedx, timer = 0f, xDirection, delta = 1f,distance = 0f;
	public GameManager manager;
	// Start is called before the first frame update

	void OnEnable() {
		speedx *= 0.2f/transform.localScale.x;
		speedy *= 0.2f/transform.localScale.x;
	}

	void Start()
	{
		speedy = Random.Range(0.07f,0.15f);
		speedx = Random.Range(0.07f,0.1f);
		transform.position = new Vector3(Random.Range(-3f,3f),15f,0);

		if(manager.distancing) {
			this.gameObject.tag = "Distancing";
			this.gameObject.GetComponent<CircleCollider2D>().radius = 7;
			this.gameObject.AddComponent<Rigidbody2D>();
			this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			this.gameObject.GetComponent<Rigidbody2D>().mass = 0.0001f;
		}
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
		float simp = Random.Range(0.15f, 0.45f);
		speedy = Random.Range(0.07f,0.15f);
		speedx = Random.Range(0.07f,0.1f);
		transform.localScale = new Vector3(simp, simp, 0);

		this.gameObject.tag = "Healthy";
		this.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
		this.gameObject.GetComponent<SpriteRenderer>().sprite = normal;
		this.gameObject.GetComponent<CircleCollider2D>().radius = 3;

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

		if(this.transform.position.y < -20) {
			if(this.gameObject.tag == "Distancing") {
				Destroy(this.gameObject);
			} else {
				manager.AddPerson(this.gameObject);
			}

			return;
		}

		float x = xDirection * speedx;
		float y = -1 * speedy * (transform.localScale.y/0.3f);
		transform.Translate(new Vector3(x,y,0));
		ScalePlayer(System.Math.Abs(y));
  }

	public void GetInfected() {
		this.gameObject.tag = "Infected";
		this.gameObject.GetComponent<SpriteRenderer>().sprite = infected;
		this.gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
		this.gameObject.AddComponent<Rigidbody2D>();
		this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		this.gameObject.GetComponent<Rigidbody2D>().mass = 0.0001f;
		manager.infected++;
		manager.infectedText.text = manager.infected.ToString();
	}

  private void OnTriggerStay2D(Collider2D other) {
		if(other.tag == "Infected" && this.gameObject.tag != "Infected") {
			GetInfected();
		}
  }
}
