using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	public Text recoveredText, criticalText, infectedText, scoreText;
	public int countPeople = 0, critical = 0, recovered = 0, deaths = 0, maxPeople = 15, score = 0, layercounter = 32766, highScore = 0, infected = 0;
	public float spawnTimer = 1.25f;
	float timer1 = 0f, timer2 = 0f, timer3 = 0f, timer4 = 0f, timer5 = 0f;
	[SerializeField]
	Sprite maskImage, lockdownImage, distancingImage;
	[SerializeField]
	Image powerupImage;
	[SerializeField]
	GameObject npc, powerup, player;
	MainMenu menu;
	public bool playing = false, distancing = false, audioEnabled = true;
	List<GameObject> people = new List<GameObject>();

	void Start()
	{
		playing = false;
		critical = 0;
		recoveredText.text = recovered.ToString();
		infectedText.text = infected.ToString();
		criticalText.text = critical.ToString();
		scoreText.text = score.ToString();
		layercounter = 32766;

		people.Clear();
		menu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
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
		}

		if(timer2 > 1f && playing){
			timer2 = 0;

			CalculateInfections();
			CalculateCritical();
			CalculateRecovered();
		}

		if(timer3 > 10f){
			if(maxPeople < 50) {
				maxPeople++;
			}

			if(spawnTimer > 0.20) {
				spawnTimer -= 0.05f;
			}

			timer3 = 0f;
		}

		if(timer4 > 20f) {
			timer4 = 0f;
			GeneratePowerup();
		}

		score += (int)(Time.deltaTime * maxPeople);
		scoreText.text = (score/5).ToString();
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
		spawnTimer = 1.25f;
		timer1 = 0f; 
		timer2 = 0f;
		timer3 = 0f;
		timer4 = 0f;
		timer5 = 0f;
		playing = false;
		distancing = false;

		recoveredText.text = recovered.ToString();
		infectedText.text = infected.ToString();
		criticalText.text = critical.ToString();
		scoreText.text = score.ToString();

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
		} else {
		}

		double multiplier = 1.2 + (0.05 * infected/20);
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

		if (Random.Range(0f, 1f) <= 0.1) {
			int recovering = (int)(infected * 0.1);
			recovered += recovering;
			recoveredText.text = recovered.ToString();
			infected -= recovering;
			infectedText.text = infected.ToString();
		}
	}

	void CalculateCritical() {
		if(critical < 5) {
			return;
		}

		double multiplier = 1.2 + (0.05 * infected/25);
		if (Random.Range(0f, 1f) <= 0.2f) {
			critical = critical + (int)(critical * multiplier);
			criticalText.text = critical.ToString();
		}

		if(critical > 50) {
			GameOver();
		}
	}

	void GameOver() {
		playing = false;
		menu.gameObject.SetActive(true);
		if (highScore < score/5) {
			highScore = score/5;
		}
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
