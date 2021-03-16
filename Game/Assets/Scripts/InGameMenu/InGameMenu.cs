using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
	[SerializeField] private GameObject Menu;
	[SerializeField] private PlayerScript playerScript;

	void Start()
	{
		Menu.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			playerScript.Pause();
			PausePressed();
		}
	}

	public void PausePressed()
	{
		if (GameIsPaused)
		{
			Resume();
		}
		else
		{
			Pause();
		}
	}

	public void Resume()
	{
		Menu.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	public void Pause()
	{
		Menu.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}

	public void MainMenu()
	{
		Time.timeScale = 1f;
		GameIsPaused = false;
		SceneManager.LoadScene("MainMenu");
	}

	public void Exit()
	{
		Application.Quit();
	}

}
