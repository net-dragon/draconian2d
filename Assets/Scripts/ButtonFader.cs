﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonFader : MonoBehaviour {

    public bool faded = false; //This will be used to determine if the next button can fade in yet

    Image buttonImage; //to change the color of the button image
    Text txt; //to change the color of the text
    Color buttonColor;
    Color textColor;
    bool startFade = false; //to determine if the button should start fading in to the screen
    float smooth = 0; //how fast the button fades in
    bool initialized = false; //whether the proper values have been initialized or not

	// Use this for initialization
	void Start ()
    {
        Initialize();
	}
	
    void Initialize()
    {
        startFade = false;
        faded = false;
        buttonImage = GetComponent<Image>();
        buttonColor = buttonImage.color;
        if (GetComponentInChildren<Text>())
        {
            txt = GetComponentInChildren<Text>();
            textColor = txt.color;
        }
        initialized = true;
    }

	// Update is called once per frame
	void Update ()
    {
	    if (startFade)
        {
            Fade(smooth);
            if (buttonColor.a > 0.9)
                faded = true;
        }
	}

    public void Fade(float rate)
    {
        //make sure the proper values have been initialized
        if (!initialized)
            Initialize();
        //set the fade speed
        smooth = rate;
        startFade = true;

        //increase the alpha of the colors

        //button image color
        buttonColor.a += rate;
        buttonImage.color = buttonColor;
        //text color
        if (txt)
        {
            textColor.a += rate;
            txt.color = textColor;
        }
    }
}
