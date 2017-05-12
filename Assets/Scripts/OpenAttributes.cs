using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OpenAttributes : MonoBehaviour {

    private GameObject attributesMenu;

    void Start()
    {
        attributesMenu = GameObject.Find("Canvas/Attributes");
        attributesMenu.SetActive(false);

    }

    void Update()
    {
        
    }

    public void OpenMenu()
    {
        attributesMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        attributesMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
