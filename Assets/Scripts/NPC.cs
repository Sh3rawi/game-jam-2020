using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
  private float speedy, speedx, timer = 0f, xDirection, delta = 1f;
	public bool sick = false;
	public GameManager manager;
	// Start is called before the first frame update
	void Start()
	{
		speedy = Random.Range(0.02f,0.06f);
		speedx = Random.Range(0.015f,0.005f);
		transform.position = new Vector3(Random.Range(-3f,3f),5f,0);
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
		sick = false;
		transform.position = new Vector3(Random.Range(-4f,4f),5f,0);
		this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		this.gameObject.GetComponent<Collider2D>().isTrigger = false;
		Destroy(this.gameObject.GetComponent<Rigidbody2D>());
	}

  private void Move() {
		if(this.transform.position.x > 40 || this.transform.position.x < -40){
			xDirection = xDirection * -1;
		}

		if(this.transform.position.y < -60){
			manager.AddPerson(this.gameObject);
			return;
		}

		float x = xDirection * speedx;
		transform.Translate(new Vector3(xDirection,-1,0) * speedy);
  }

  private void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Collided");
		GameObject them = other.gameObject;
		bool infected;

    if(them.name.Contains("NPC")) {
			infected = them.GetComponent<NPC>().sick;
		} else {
			infected = true;	
		}

		if (infected) {
			this.gameObject.GetComponent<Collider2D>().isTrigger = true;
			sick = true;
			this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
			this.gameObject.AddComponent<Rigidbody2D>();
			this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			this.gameObject.GetComponent<Rigidbody2D>().mass = 0.0001f;
			manager.infected++;
		}
  }
}
