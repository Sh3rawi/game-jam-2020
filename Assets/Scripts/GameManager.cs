using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	public Text recoveredText, criticalText, infectedText, scoreText;
	public int countPeople = 0, critical = 0, recovered = 0, deaths = 0, maxPeople = 50, score = 0, layercounter = 32766, highScore = 0, infected = 0;
	public float spawnTimer = 0.75f;
	float timer1 = 0f, timer2 = 0f, timer3 = 0f, timer4 = 0f, timer5 = 0f;
	[SerializeField]
	Sprite maskImage, lockdownImage, distancingImage;
	[SerializeField]
	Image red, yellow, green;
	[SerializeField]
	Image powerupImage;
	[SerializeField]
	GameObject npc, powerup, player;
	MainMenu menu;
	public bool playing = false, distancing = false, audioEnabled = true;
	List<GameObject> people = new List<GameObject>();
	[SerializeField]
	AudioClip main, inGame;
	AudioSource source;

	void Start()
	{
		playing = false;
		critical = 0;
		recoveredText.text = recovered.ToString();
		infectedText.text = infected.ToString();
		criticalText.text = critical.ToString();
		scoreText.text = score.ToString();
		layercounter = 32766;

		source = GameObject.Find("SoundManager").GetComponent<AudioSource>();

		source.clip = main;
		source.Play();

		people.Clear();
		menu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
		source.loop = true;
	}

	// Update is called once per frame
	void Update()
	{
		if(!playing) {
			return;
		}

		timer1 += Time.deltaTime;
		timer2 += Time.deltaTime;
		timer3 += Time.deltaTime;
		timer4 += Time.deltaTime;
		timer5 += Time.deltaTime;

		if(timer1 >= spawnTimer){
			timer1 = 0f;
			GeneratePerson();

		if(infected > 20) {
			yellow.enabled = true;
		} else {
			yellow.enabled = false;
		}

		if(infected > 35) {
			red.enabled = true;
		} else {
			red.enabled = false;
		}

		
		if(infected > 50) {
			GameOver();
		}
		}

		if(timer2 > 5f && playing){
			timer2 = 0;

			CalculateInfections();
			CalculateRecovered();

			if(maxPeople < 150) {
			maxPeople++;
			}

	
		}

		if(timer3 > 10f){
			timer3 = 0f;

		if(spawnTimer > 0.2f) {
				spawnTimer -= 0.05f;
			}

			CalculateCritical();
		}

		if(timer4 > 20f) {
			timer4 = 0f;
			GeneratePowerup();
		}

		// if(timer5 > 0.1f) {
			score += (int)(Time.deltaTime * (100 + maxPeople));
			scoreText.text = (score/50).ToString();
		// }
	}

	public void ResetGame() {
		countPeople = 0; 
		critical = 0; 
		recovered = 0; 
		deaths = 0; 
		maxPeople = 15; 
		score = 0; 
		layercounter = 32766; 
		highScore = 0; 
		infected = 0;
		spawnTimer = 0.75f;
		timer1 = 0f; 
		timer2 = 0f;
		timer3 = 0f;
		timer4 = 0f;
		timer5 = 0f;
		playing = false;
		distancing = false;
		green.enabled = true;
		red.enabled = false;
		yellow.enabled = false;

		recoveredText.text = recovered.ToString();
		infectedText.text = infected.ToString();
		criticalText.text = critical.ToString();
		scoreText.text = score.ToString();

		source.clip = inGame;
		source.Play();

		Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
	}

	void GeneratePerson() {
		if (countPeople < maxPeople) {
			layercounter--;
			var temp = Instantiate(npc, new Vector3(0, 0, 0), Quaternion.identity);
			temp.GetComponent<NPC>().manager = this;
			temp.GetComponent<SpriteRenderer>().sortingOrder = layercounter;
			countPeople++;
		} else {
			if (people.Count > 0) {
				layercounter--;
				var person = people[0];
				person.SetActive(true);
				person.GetComponent<NPC>().Reset();
				person.GetComponent<SpriteRenderer>().sortingOrder = layercounter;
				people.Remove(person);
			}
		}
	}

	void CalculateInfections() {
		if(infected < 10) {
			return;
		}

		if((int)(infected/15) > critical) {
			critical++;
			criticalText.text = critical.ToString();
		}

		double multiplier = 1.1 + (0.05 * infected/75);
		if (Random.Range(0f, 1f) <= 0.2f) {
			infected = (int)(infected*multiplier);
			infectedText.text = infected.ToString();
		}
	}

	void GeneratePowerup() {
		var temp = Instantiate(powerup, new Vector3(0, 0, 0), Quaternion.identity);
		temp.GetComponent<PowerUp>().manager = this;
		temp.GetComponent<SpriteRenderer>().sortingOrder = 32766;
	}

	void CalculateRecovered() {
		if(infected < 30) {
			return;
		}

		if (Random.Range(0f, 1f) <= 0.5) {
			int recovering = (int)(infected * 0.1);
			recovered += recovering;
			recoveredText.text = recovered.ToString();
			infected -= recovering;
			infectedText.text = infected.ToString();
		}
	}

	void CalculateCritical() {


		double multiplier = 1.1 + (0.05 * infected/50);
		if (Random.Range(0f, 1f) <= 0.1f) {
			int temp =  (int)(critical * multiplier);
			critical = critical + temp;
			criticalText.text = critical.ToString();

			infected -= temp;
			infectedText.text = infected.ToString();
		}

	}

	void GameOver() {
		playing = false;
		menu.gameObject.SetActive(true);
		source.clip = main;
		source.Play();
		if (highScore < score/50) {
			highScore = score/50;
		}
		menu.SetScore();
	}

	public void AddPerson(GameObject person) {
		if(!people.Contains(person) && person != null) {
			people.Add(person);
			person.SetActive(false);
		}
	}

	public void ShowImage(string image) {
		powerupImage.color = Color.white;
		if (image == "mask") {
			powerupImage.sprite = maskImage;
		} else if (image == "lockdown") {
			powerupImage.sprite = lockdownImage;
		} else if (image == "distancing") {
			powerupImage.sprite = distancingImage;
		}
	}

	public void RemoveImage() {
		powerupImage.color = Color.black;
	}
}
