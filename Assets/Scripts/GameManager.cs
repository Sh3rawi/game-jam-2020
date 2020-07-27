using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	int countPeople = 0, critical, recovered, deaths, maxPeople = 10;
	public int infected = 0;
	float spawnTimer = 1.25f, timer1 = 0f, timer2 = 0f;
	[SerializeField]
	GameObject npc;
	bool playing = true;
	List<GameObject> people = new List<GameObject>();
	void Start()
	{
		people.Clear();
	}

	// Update is called once per frame
	void Update()
	{
		timer1 += Time.deltaTime;
		if(timer1 >= spawnTimer){
			GeneratePerson();
			timer1 = 0f;
		}

		timer2 += Time.deltaTime;
		if(timer2%5 == 0){
			maxPeople++;
		}
		if(timer2%10 == 0){
			spawnTimer -= 0.05f;
		}

		GeneratePowerUp();
	}

	void GeneratePerson() {
		if (countPeople < maxPeople) {
			var temp = Instantiate(npc, new Vector3(0, 0, 0), Quaternion.identity);
			temp.GetComponent<NPC>().manager = this;
			countPeople++;
		} else {
			if (people.Count > 0) {
				var person = people[0];
				person.SetActive(true);
				person.GetComponent<NPC>().Reset();
				people.Remove(person);
			}
		}
	}

	public void AddPerson(GameObject person) {
		if(!people.Contains(person) && person != null) {
			people.Add(person);
			person.SetActive(false);
		}
	}

	void GeneratePowerUp() {

	}
}
