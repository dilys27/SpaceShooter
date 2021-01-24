using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dlb_PauseManager : MonoBehaviour
{
	public static bool dlb_gameIsPaused = false;

	public Text dlb_pauseTxt;

	void Start()
	{
		dlb_pauseTxt.gameObject.SetActive(false);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			dlb_PauseGame();
		}
	}

	void dlb_PauseGame()
	{
		dlb_gameIsPaused = !dlb_gameIsPaused;
		if (dlb_gameIsPaused)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1;
		}
		dlb_pauseTxt.gameObject.SetActive(dlb_gameIsPaused);
	}


}
