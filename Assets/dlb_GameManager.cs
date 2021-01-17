using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dlb_GameManager : MonoBehaviour
{
	public enum States
	{
		wait, play, nextWave, dead
	}
	public static States state;

	int wave;
	int score;
	int lives;

	public Text waveTxt;
	public Text scoreTxt;
	public Text livesTxt;

	public Text messageTxt;

	GameObject player;
	public GameObject intruder; // le prefab
	public GameObject boom;

	Camera cam;
	float height, width;

	public GameObject waitToStart; // panel

	public GameObject networkPanel;

	NetworkManager networkManager;

	public GameObject spawnArea;
	float widthArea;
	float xArea, yArea;


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

		widthArea = spawnArea.GetComponent<BoxCollider2D>().bounds.size.x;
		xArea = spawnArea.transform.position.x;
		yArea = spawnArea.transform.position.y;

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
		LoadWave();
		UpdateTexts();
	}

	void InitGame()
	{
		wave = 1;
		score = 0;
		lives = 5;
	}

	void LoadWave()
	{
		state = States.play;

		// instancier 3 ennemis (selon la vague: 2 + wave)
		// - savoir ce qu'est un ennemi (public ...)
		// - faire une boucle 1 à 3
		//   instancier un ennemi : dans les limites de la spawn area (en dehors de l'écran)
		
		for (int i = 0; i < 3; i++)
		{
			float x = xArea + Random.Range(-widthArea/2, widthArea/2);
			float y = yArea;
			Instantiate(intruder, new Vector2(x, y), Quaternion.identity);
		}
		
	}

	void UpdateTexts()
	{
		waveTxt.text = "wave: " + wave;
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
			EndOfWave();
		}
	}


	void EndOfWave()
	{
		GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemys.Length == 0)
		{
			StartCoroutine(NextWave());
		}
	}

	IEnumerator NextWave()
	{
		state = States.nextWave;
		// afficher message "Next Wave"
		messageTxt.text = "Next Wave";
		messageTxt.gameObject.SetActive(true);
		// marquer une pause
		yield return new WaitForSecondsRealtime(3f);
		// cacher le message
		messageTxt.gameObject.SetActive(false);
		wave += 1;
		LoadWave();
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
