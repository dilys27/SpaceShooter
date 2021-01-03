using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public enum States
	{
		wait, play, levelup, dead
	}
	public static States state;

	int level;
	int score;
	int lives;

	public Text levelTxt;
	public Text scoreTxt;
	public Text livesTxt;

	public Text messageTxt;


	GameObject player;
	public GameObject asteroid; // le grand prefab
	public GameObject boom;


	Camera cam;
	float height, width;


	public GameObject waitToStart; // panel


	public GameObject networkPanel;

	NetworkManager networkManager;


	void Start()
	{
		networkManager = GetComponent<NetworkManager>();
		networkPanel.gameObject.SetActive(true);


		messageTxt.gameObject.SetActive(false);

		player = GameObject.FindWithTag("Player");

		cam = Camera.main;
		height = cam.orthographicSize;
		width = height * cam.aspect;


		waitToStart.gameObject.SetActive(true);
		int highscore = PlayerPrefs.GetInt("highscore");
		if (highscore > 0)
		{
			messageTxt.text = "highscore: " + highscore;
			messageTxt.gameObject.SetActive(true);
		}

		state = States.wait;
	}

	public void LaunchGame()
	{
		// interface
		networkPanel.gameObject.SetActive(false);
		waitToStart.gameObject.SetActive(false);
		messageTxt.gameObject.SetActive(false);
		// restaurer après une partie
		player.SetActive(true);
		GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemys)
		{
			Destroy(enemy);
		}
		// lancer une partie
		InitGame();
		LoadLevel();
		UpdateTexts();
	}



	void LoadLevel()
	{
		state = States.play;

		// instancier 3 asteroids (selon le niveau)
		// - savoir ce qu'est un asteroid (public ...)
		// - faire une boucle 1 à 3
		//       instancier un asteroid : dans les limites de l'écran
		for (int i = 0; i < 2 + level; i++)
		{
			float x = Random.Range(-width, width);
			float y = Random.Range(-height, height);
			Instantiate(asteroid, new Vector2(x, y), Quaternion.identity);
		}
	}

	void InitGame()
	{
		level = 1;
		score = 0;
		lives = 5;
	}

	void UpdateTexts()
	{
		levelTxt.text = "level: " + level;
		scoreTxt.text = "score: " + score;
		livesTxt.text = "lives: " + lives;
	}


	public void AddScore(int points)
	{
		score += points;
		UpdateTexts();
	}

	private void Update()
	{
		if (state == States.play)
		{
			EndOfLevel();
		}
	}


	void EndOfLevel()
	{
		GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemys.Length == 0)
		{
			StartCoroutine(LevelUp());
		}
	}

	IEnumerator LevelUp()
	{
		state = States.levelup;
		// afficher message "level up"
		messageTxt.text = "level up";
		messageTxt.gameObject.SetActive(true);
		// marquer une pause
		yield return new WaitForSecondsRealtime(3f);
		// cacher le message
		messageTxt.gameObject.SetActive(false);
		level += 1;
		LoadLevel();
		UpdateTexts();
	}


	public void KillPlayer()
	{
		StartCoroutine(PlayerAgain());
	}

	IEnumerator PlayerAgain()
	{
		state = States.dead;

		GameObject boomGO = Instantiate(boom, player.transform.position, Quaternion.identity);

		lives -= 1;
		player.SetActive(false);
		UpdateTexts();
		if (lives <= 0)
		{
			yield return new WaitForSecondsRealtime(2f);
			Destroy(boomGO);
			GameOver();
		}
		else
		{
			yield return new WaitForSecondsRealtime(3f);
			Destroy(boomGO);
			player.SetActive(true);
			state = States.play;
		}
	}

	void GameOver()
	{
		state = States.wait;

		int highscore = PlayerPrefs.GetInt("highscore");
		if (score > highscore)
		{
			PlayerPrefs.SetInt("highscore", score);
			messageTxt.text = "new highscore: " + score;
		}
		else
		{
			messageTxt.text = "game over\nhighscore: " + highscore;
		}

		networkManager.SendScore(score);
		networkPanel.gameObject.SetActive(true);

		messageTxt.gameObject.SetActive(true);
		waitToStart.gameObject.SetActive(true);
	}




}
