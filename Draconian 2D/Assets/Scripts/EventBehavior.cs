using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class EventBehavior : MonoBehaviour{

	private GameObject pauseMenu;

	void Start()
	{
		pauseMenu = GameObject.Find ("Canvas/PauseMenu");
		pauseMenu.SetActive (false);
        
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.P))
			PauseGame();
	}

	public void PauseGame()
	{
		pauseMenu.SetActive (true);
		Time.timeScale = 0;
	}

	public void ContinueGame()
	{
		pauseMenu.SetActive (false);
		Time.timeScale = 1;
	}

	public void RestartGame()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void ExitGame()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene ("Scenes/Title");
	}
}
