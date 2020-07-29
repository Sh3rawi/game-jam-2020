using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour {
	[SerializeField]
	Sprite vax, mask, lockdown, distancing, truck;
	public GameManager manager;
	float speedx, speedy, xDirection, timer = 0f, delta = 1f;
	// Start is called before the first frame update
	void Start() {
		speedy = Random.Range(0.07f,0.15f);
		speedx = Random.Range(0.07f,0.1f);

		transform.position = new Vector3(Random.Range(-3f,3f),15f,0);
		SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
		float power = Random.Range(0f, 1f);
		if (power < 0.05f) {
			renderer.sprite = vax;
			this.gameObject.name = "vax";
		}	else if (power < 0.3f) {
			renderer.sprite = mask;
			this.gameObject.name = "mask";
		}	else if (power < 0.55f) {
			renderer.sprite = lockdown;
			this.gameObject.name = "lockdown";
		}	else if (power < 0.8f) {
			renderer.sprite = distancing;
			this.gameObject.name = "distancing";
		}	else if (power < 1f) {
			renderer.sprite = truck;
			this.gameObject.name = "truck";
		}
	}

	// Update is called once per frame
	void Update() {
		timer += Time.deltaTime;
		if(timer >= delta){
			xDirection = Random.Range(-1f,1f);
			delta = Random.Range(0.75f,1.25f);
			timer = 0f;
		}

		Move();	
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
			Destroy(this.gameObject);
			return;
		}

		float x = xDirection * speedx;
		float y = -1 * speedy * (transform.localScale.y/0.5f);
		transform.Translate(new Vector3(x,y,0));
		ScalePlayer(System.Math.Abs(y));
  }
}
