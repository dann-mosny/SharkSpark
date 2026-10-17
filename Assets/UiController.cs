using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
	public GameObject GameOverPanel;
	public Text LoseMessage;
	public GameObject WinPanel;
	bool GameOver;
	bool Win;
	bool Paused;
	public GameObject PausePanel;

	bool StartMenu;
	public GameObject StartMenuPanel;

	// Start is called before the first frame update
	void Start()
	{
		GameOver = false;
		Win = false;
		Paused = false;
		GameOverPanel.SetActive(false);
		WinPanel.SetActive(false);
		PausePanel.SetActive(false);

		StartMenuPanel.SetActive(true);
		StartMenu = true;
		Time.timeScale = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (StartMenu)
		{
			if ( !FirstStart.value || Input.GetMouseButtonDown(0))
			{
				StartMenu = false;
				StartMenuPanel.SetActive(false);
				Time.timeScale = 1;
			}
			return;
		}
		FirstStart.value = false;

		if ((GameOver||Win) && Input.GetKeyDown(KeyCode.R))
		{
			Time.timeScale = 1;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		else if (!GameOver && !Win && !Paused && Input.GetKeyDown(KeyCode.Escape))
		{
			Paused = true;
			Time.timeScale = 0;
			PausePanel.SetActive(true);
		}

		else if (!GameOver && !Win &&  Paused && Input.GetKeyDown(KeyCode.Escape))
		{
			Paused = false;
			Time.timeScale = 1;
			PausePanel.SetActive(false);
		}
	}

	public void SetGameOver(string message)
	{
		if (Win) return;
		GameOver = true;
		Time.timeScale = 0;
		LoseMessage.text = message;
		GameOverPanel.SetActive(true);
		GameOverPanel.GetComponent<AudioSource>().Play();
	}

	public void SetWin()
	{
		if (GameOver) return;
		Win = true;
		Time.timeScale = 0;
		WinPanel.SetActive(true);
		WinPanel.GetComponent<AudioSource>().Play();
	}
}

static class FirstStart
{
	public static bool value = true;
}
