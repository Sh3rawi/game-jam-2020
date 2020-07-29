using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public VariableJoystick joystick;
  private float speed = 0.3f;
	public static bool sick = true;
	private bool joystickChanged = false, mask = false;
	GameManager manager;
	Sprite image;
	[SerializeField]
	AudioClip powerUpSound, powerDownSound, truckSound;
	AudioSource source;

  // Start is called before the first frame update
  void Start() {
		manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<VariableJoystick>();
		source = GameObject.Find("PowerUpManager").GetComponent<AudioSource>();
		transform.position = new Vector3(0,-5f,0);
  }

  // Update is called once per frame
  void Update() {
		if(manager.playing) {
			Move();
			if (joystick.Direction.x != 0 && !joystickChanged) {
				joystickChanged = true;
				joystick.SetMode(JoystickType.Dynamic);
				joystick.gameObject.transform.Find("Background").gameObject.SetActive(true);
			}
		} else {
			Destroy(this.gameObject);
		}
  }

  private void Move() {
		if ((joystick.Direction.x > 0 && transform.position.x < 6) || (joystick.Direction.x < 0 && transform.position.x > -6)){
			transform.Translate(Vector3.right * speed * joystick.Direction.x);
		}
  }

	IEnumerator Mask() {
		mask = true;
		manager.ShowImage("mask");
		yield return new WaitForSeconds (7f);
		mask = false;
		manager.RemoveImage();
	}

	IEnumerator Lockdown() {
		float temp = manager.spawnTimer;
		manager.spawnTimer = 3f;
		manager.ShowImage("lockdown");
		yield return new WaitForSeconds (10f);
		manager.spawnTimer = temp;
		manager.RemoveImage();
	}

	IEnumerator Distancing() {
		manager.distancing = true;
		manager.ShowImage("distancing");
		yield return new WaitForSeconds (10f);
		manager.distancing = false;
		manager.RemoveImage();
	}

	public void PlaySound(AudioClip clip) {
		if (!manager.audioEnabled)
			return;

		source.clip = clip;
		source.PlayOneShot(clip);
	}


  private void OnTriggerEnter2D(Collider2D other) {
		if(!manager.playing) {
			return;
		}

		if(other.tag == "Healthy" && !mask) {
			other.GetComponent<NPC>().GetInfected();
		} else if(other.tag == "Infected" || other.tag == "Lockdown") {
			Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
		} if (other.tag == "Powerup") {
			Destroy(other.gameObject);
			if (other.name == "vax") {
				PlaySound(powerUpSound);
				manager.recovered += manager.critical;
				manager.recovered += manager.infected;
				manager.recoveredText.text = manager.recovered.ToString();

				manager.infected = 0;
				manager.infectedText.text = manager.infected.ToString();

				manager.critical = 0;
				manager.criticalText.text = manager.critical.ToString();
			}	else if (other.name == "mask") {
				PlaySound(powerUpSound);
				StartCoroutine(Mask());
			}	else if (other.name == "lockdown") {
				PlaySound(powerUpSound);
				StartCoroutine(Lockdown());
				//change spawn delay to 3 seconds for 15 seconds
			}	else if (other.name == "distancing") {
				PlaySound(powerUpSound);
				StartCoroutine(Distancing());
				//make colliders chunky boys for 10 seconds
			}	else if (other.name == "truck") {
				PlaySound(powerDownSound);
				manager.critical *= 2;
				manager.criticalText.text = manager.critical.ToString();

				manager.infected *= 2;
				manager.infectedText.text = manager.infected.ToString();
			}
		}
  }

	void OnDestroy() {
		source.clip = truckSound;
		source.PlayOneShot(truckSound);
	}
}