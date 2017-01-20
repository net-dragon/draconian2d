using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuOpenner : MonoBehaviour {
    private GameObject menu;
    public string menuName;

	void Start()
    {
        menu = GameObject.Find(menuName);
       // if (menu.active == true)
       // menu.SetActive(false);

    }


    void TaskOnClick()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        DestroyObject(gameObject);
        Time.timeScale = 1;
    }
}
