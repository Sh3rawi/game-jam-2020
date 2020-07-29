using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	[SerializeField]
	Button play, again, audio;
	[SerializeField]
	Sprite audioOnImage, audioOffImage, playImage, againImage;
	[SerializeField]
	Text scoreText;
	GameManager manager;
	int score, highScore;
	// Start is called before the first frame update
	void Start() {
		play.onClick.AddListener(PlayAgain);
		manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		score = manager.score;
		highScore = manager.highScore;
		this.gameObject.SetActive(!manager.playing);
	}

	// Update is called once per frame
	void Update() {
		
	}

	void PlayAgain() {
		manager.ResetGame();
		manager.playing = true;
		this.gameObject.SetActive(false);
	}

	void Audio() {
		manager.audioEnabled = true;
	}
}
