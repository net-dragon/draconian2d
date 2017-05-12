using UnityEngine;
using System.Collections;

public class OpenDash : MonoBehaviour {

    private GameObject DashMenu;

    void Start()
    {
        DashMenu = GameObject.Find("Canvas/Dash");
        DashMenu.SetActive(false);

    }

    void Update()
    {

    }

    public void OpenMenu()
    {
        DashMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        DashMenu.SetActive(false);
        Time.timeScale = 1;
    }
}

