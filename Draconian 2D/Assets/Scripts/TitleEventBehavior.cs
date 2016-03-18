using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleEventBehavior : MonoBehaviour {

	void Start()
	{
		
	}

	public void PlayGame()
	{
		SceneManager.LoadScene ("Scenes/Test");
	}
}
