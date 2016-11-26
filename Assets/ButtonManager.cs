using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
    //Calls up all of the secondary buttons
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    RectTransform rect;

    GameObject mainButton;

    public Transform Button; //transform of the main button

    public Vector3 buttonPos; //position of the main button

    //width and height of the main button
    float width;
    float height;




    // Use this for initialization
    void Start ()
    {
        rect = GetComponent<RectTransform>();
       // RectTransform.rect.height = height;
       // RectTransform.rect.width = width;
        /*
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        height = corners[2].y - corners[0].y;
        */
        /*
        buttonPos = transform.position;
        button1.transform.position=  new Vector3(buttonPos.x + RectTransform.rect.width, buttonPos.y + RectTransform.rect.height);
        button2.transform.position=buttonPos;
        button3.transform.position=buttonPos;
        button4.transform.position=buttonPos;
        button5.transform.position=buttonPos;
        */
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
