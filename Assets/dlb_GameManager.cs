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
	public static States dlb_state;

	int dlb_wave;
	int dlb_score;
	int dlb_lives;

	public Text dlb_waveTxt;
	public Text dlb_scoreTxt;
	public Text dlb_livesTxt;

	public Text dlb_messageTxt;

	GameObject dlb_player;
	public GameObject dlb_intruder; // le prefab
	public GameObject dlb_boom;

	Camera cam;
	float height, width;

	public GameObject dlb_waitToStart; // panel

	public GameObject dlb_networkPanel;

	dlb_NetworkManager dlb_networkManager;

	public GameObject dlb_spawnArea;
	float dlb_widthArea;
	float dlb_xArea, dlb_yArea;

	public GameObject dlb_attackArea;

	void Start()
	{
		dlb_networkManager = GetComponent<dlb_NetworkManager>();
		dlb_networkPanel.gameObject.SetActive(true);

		dlb_messageTxt.gameObject.SetActive(false);

		dlb_player = GameObject.FindWithTag("Player");

		cam = Camera.main;
		height = cam.orthographicSize;
		width = height * cam.aspect;

		dlb_waitToStart.gameObject.SetActive(true);
		int highscore = PlayerPrefs.GetInt("highscore");
		if (highscore > 0)
		{
			dlb_messageTxt.text = "highscore: " + highscore;
			dlb_messageTxt.gameObject.SetActive(true);
		}

		dlb_widthArea = dlb_spawnArea.GetComponent<BoxCollider2D>().bounds.size.x;
		dlb_xArea = dlb_spawnArea.transform.position.x;
		dlb_yArea = dlb_spawnArea.transform.position.y;

		dlb_state = States.wait;
	}

	public void LaunchGame()
	{
		// interface
		dlb_networkPanel.gameObject.SetActive(false);
		dlb_waitToStart.gameObject.SetActive(false);
		dlb_messageTxt.gameObject.SetActive(false);
		// restaurer après une partie
		dlb_player.SetActive(true);
		GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemys)
		{
			Destroy(enemy);
		}
		// lancer une partie
		dlb_InitGame();
		dlb_LoadWave();
		dlb_UpdateTexts();
	}

	void dlb_InitGame()
	{
		dlb_wave = 1;
		dlb_score = 0;
		dlb_lives = 5;
	}

	void dlb_LoadWave()
	{
		dlb_state = States.play;

		// instancier 3 ennemis (selon la vague: 2 + wave)
		// - savoir ce qu'est un ennemi (public ...)
		// - faire une boucle 1 à 3
		//   instancier un ennemi : dans les limites de la spawn area (en dehors de l'écran)

		for (int i = 0; i < 3; i++)
		{
			float x = dlb_xArea + Random.Range(-dlb_widthArea/3, dlb_widthArea/3);
			float y = dlb_yArea;
			Instantiate(dlb_intruder, new Vector2(x, y), Quaternion.identity);
		}
		
	}

	void dlb_UpdateTexts()
	{
		dlb_waveTxt.text = "wave: " + dlb_wave;
		dlb_scoreTxt.text = "score: " + dlb_score;
		dlb_livesTxt.text = "lives: " + dlb_lives;
	}


	public void dlb_AddScore(int points)
	{
		dlb_score += points;
		dlb_UpdateTexts();
	}

	private void Update()
	{
		if (dlb_state == States.play)
		{
			dlb_EndOfWave();
		}
	}


	void dlb_EndOfWave()
	{
		GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemys.Length == 0)
		{
			StartCoroutine(dlb_NextWave());
		}
	}

	IEnumerator dlb_NextWave()
	{
		dlb_state = States.nextWave;
		// afficher message "Next Wave"
		dlb_messageTxt.text = "Next Wave";
		dlb_messageTxt.gameObject.SetActive(true);
		// marquer une pause
		yield return new WaitForSecondsRealtime(3f);
		// cacher le message
		dlb_messageTxt.gameObject.SetActive(false);
		dlb_wave += 1;
		dlb_LoadWave();
		dlb_UpdateTexts();
	}


	public void dlb_KillPlayer()
	{
		StartCoroutine(dlb_PlayerAgain());
	}

	IEnumerator dlb_PlayerAgain()
	{
		dlb_state = States.dead;

		GameObject dlb_boomGO = Instantiate(dlb_boom, dlb_player.transform.position, Quaternion.identity);

		dlb_lives -= 1;
		dlb_player.SetActive(false);
		dlb_UpdateTexts();
		if (dlb_lives <= 0)
		{
			yield return new WaitForSecondsRealtime(2f);
			Destroy(dlb_boomGO);
			dlb_GameOver();
		}
		else
		{
			yield return new WaitForSecondsRealtime(3f);
			Destroy(dlb_boomGO);
			dlb_player.SetActive(true);
			dlb_state = States.play;
		}
	}

	void dlb_GameOver()
	{
		dlb_state = States.wait;

		int highscore = PlayerPrefs.GetInt("highscore");
		if (dlb_score > highscore)
		{
			PlayerPrefs.SetInt("highscore", dlb_score);
			dlb_messageTxt.text = "new highscore: " + dlb_score;
		}
		else
		{
			dlb_messageTxt.text = "game over\nhighscore: " + highscore;
		}

		dlb_networkManager.SendScore(dlb_score);
		dlb_networkPanel.gameObject.SetActive(true);

		dlb_messageTxt.gameObject.SetActive(true);
		dlb_waitToStart.gameObject.SetActive(true);
	}




}
