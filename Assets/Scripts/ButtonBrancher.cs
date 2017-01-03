using UnityEngine;
//using System;
using System.Collections;
using UnityEngine.UI; //Because we will use UI elements
using System.Collections.Generic; //Because we will be using dynamic lists


public class ButtonBrancher : MonoBehaviour {
    
    public class ButtonScaler
    {
        enum ScaleMode { MatchWidthHeight, IndependentWidthHeight }
        ScaleMode mode;
        Vector2 referenceButtonSize;

        [HideInInspector]
        public Vector2 referenceScreenSize;
        public Vector2 newButtonSize;

        public void Initialize (Vector2 refButtonSize, Vector2 refScreenSize, int scaleMode)
        {
            mode = (ScaleMode)scaleMode;
            referenceButtonSize = refButtonSize;
            referenceScreenSize = refScreenSize;
            SetNewButtonSize();
        }

        void SetNewButtonSize()
        {
            if (mode == ScaleMode.IndependentWidthHeight)
            {
                newButtonSize.x = (referenceButtonSize.x * Screen.width) / referenceScreenSize.x;
                newButtonSize.y = (referenceButtonSize.x * Screen.height) / referenceScreenSize.y;
            }
            else if (mode == ScaleMode.MatchWidthHeight)
            {
                newButtonSize.x = (referenceButtonSize.x * Screen.width) / referenceScreenSize.x;
                newButtonSize.y = newButtonSize.x;
            }
        }
    }
    
    [System.Serializable] //We want our class and its memebers to be seen by the inspector
    public class RevealSettings
    {
        public enum RevealOptions { Linear, Circular};
        public RevealOptions options;
        public float translateSmooth = 5f; //How fast the buttons move to their positions
        public float fadeSmooth = .01f; //How fast the buttons fade in, if they fade in
        public bool revealOnStart = false;

        [HideInInspector] //We do not need to see these in the inspector
        public bool spawned = false;
        [HideInInspector]
        public bool opening = false;
    }
    
    [System.Serializable]
    public class LinearSpawner
    {
        public enum RevealStyle { SlideToPosition , FadeInAtPosition};
        public RevealStyle revealStyle;
        public Vector2 direction = new Vector2( 0, 1); //slide down
        public float baseButtonSpacing = 5f; //How much space between each button
        public int buttonNumOffset = 0; //How many button spaces offset? Sometimes necessary for multiple button branches

        [HideInInspector]
        public float buttonSpacing = 5f;

        public void FitSpacingToScreen(Vector2 refScreenSize)
        {
            float refScreenFloat = (refScreenSize.x + refScreenSize.y) / 2;
            float screenFloat = (Screen.width + Screen.height) / 2;
            buttonSpacing = (baseButtonSpacing * screenFloat) / refScreenFloat;
        }
    }

    [System.Serializable]
    public class CircularSpawner
    {
        public enum RevealStyle { SlideToPosition, FadeInAtPosition };
        public RevealStyle revealStyle;
        public Angle angle;
        public float baseDistFromBrancher = 20;

        [HideInInspector]
        public float distFromBrancher = 0;

        [System.Serializable]
        public struct Angle { public float minAngle; public float maxAngle; }

        public void FitSpacingToScreen(Vector2 refScreenSize)
        {
            float refScreenFloat = (refScreenSize.x + refScreenSize.y) / 2;
            float screenFloat = (Screen.width + Screen.height) / 2;
            distFromBrancher = (distFromBrancher * screenFloat) / refScreenFloat;
        }
    }


    public GameObject[] buttonRefs; // PREFABS
    [HideInInspector]
    public List<GameObject> buttons;

    public enum ScaleMode { MatchWidthHeight, IndependentWidthHeight };
    public ScaleMode scaleMode;
    public Vector2 referenceButtonSize;
    public Vector2 referenceScreenSize;

    ButtonScaler buttonScaler = new ButtonScaler();
    public RevealSettings revealSettings = new RevealSettings();
    public LinearSpawner linearSpawner = new LinearSpawner();
    public CircularSpawner circularSpawner = new CircularSpawner();

    float lastScreenWidth = 0;
    float lastScreenHeight = 0;

    void Start()
    {
        buttons = new List<GameObject>();
        buttonScaler = new ButtonScaler();
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        buttonScaler.Initialize(referenceButtonSize, referenceScreenSize, (int)scaleMode); //was originally "(int)mode"
        circularSpawner.FitSpacingToScreen(buttonScaler.referenceScreenSize);
        linearSpawner.FitSpacingToScreen(buttonScaler.referenceScreenSize);

        if (revealSettings.revealOnStart)
        {
            SpawnButtons();
        }

    }

    void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            buttonScaler.Initialize(referenceButtonSize, referenceScreenSize, (int)scaleMode); //was originally "(int)mode"
            circularSpawner.FitSpacingToScreen(buttonScaler.referenceScreenSize);
            linearSpawner.FitSpacingToScreen(buttonScaler.referenceScreenSize);
            SpawnButtons();
        }

        if (revealSettings.opening)
        {
            if (!revealSettings.spawned)
                SpawnButtons();

            switch (revealSettings.options)
            {
                case RevealSettings.RevealOptions.Linear:

                    switch (linearSpawner.revealStyle)
                    {
                        case LinearSpawner.RevealStyle.SlideToPosition: RevealLinearlyNormal(); break;
                        case LinearSpawner.RevealStyle.FadeInAtPosition: ReavealLinearlyFade(); break;
                    }

                    break;
                case RevealSettings.RevealOptions.Circular:

                    switch (circularSpawner.revealStyle)
                    {
                        case CircularSpawner.RevealStyle.SlideToPosition: RevealCircularNormal(); break;
                        case CircularSpawner.RevealStyle.FadeInAtPosition: RevealCircularFade(); break;
                    }

                    break;
            }
        }
    }

    public void SpawnButtons() //if revealOnStart = false, this method will be called by the button click event
    {
        revealSettings.opening = true;
        //clear button list, in case there are some already on it
        for (int i = buttons.Count - 1; i >= 0; i--)
            Destroy(buttons[i]);
        buttons.Clear();

        //clear buttons on any other button brancher that has the same parent as this brancher
        ClearCommonButtonBranchers();

        for (int i = 0; i < buttonRefs.Length; i++)
        {
            GameObject b = Instantiate(buttonRefs[i] as GameObject);
            b.transform.SetParent(transform); //Make button child of the button brancher
            b.transform.position = transform.position; //zeroing the position places the button on the button brancher
            //check if the button will fade or not
            if (linearSpawner.revealStyle == LinearSpawner.RevealStyle.FadeInAtPosition || circularSpawner.revealStyle == CircularSpawner.RevealStyle.FadeInAtPosition)
            {
                //change color alpha of the button and its text to 0
                Color c = b.GetComponent<Image>().color;
                c.a = 0;
                b.GetComponent<Image>().color = c;
                if (b.GetComponentInChildren<Text>()) //button may not have text component
                {
                    c = b.GetComponentInChildren<Text>().color;
                    c.a = 0;
                    b.GetComponentInChildren<Text>().color = c;
                }
            }
            buttons.Add(b);
        }

        revealSettings.spawned = true;
    
    }

    void RevealLinearlyNormal()
    {
        for (int i = 0; i <buttons.Count; i++)
        {
            //give the buttons a position to move towards
            Vector3 targetPos;
            RectTransform buttonRect = buttons[i].GetComponent<RectTransform>();
            //set size
            buttonRect.sizeDelta = new Vector2(buttonScaler.newButtonSize.x, buttonScaler.newButtonSize.y);
            //set pos
            targetPos.x = linearSpawner.direction.x * ((i + linearSpawner.buttonNumOffset) * (buttonRect.sizeDelta.x + linearSpawner.buttonSpacing)) + transform.position.x;
            targetPos.y = linearSpawner.direction.y * ((i + linearSpawner.buttonNumOffset) * (buttonRect.sizeDelta.y + linearSpawner.buttonSpacing)) + transform.position.y;
            targetPos.z = 0;

            buttonRect.position = Vector3.Lerp(buttonRect.position, targetPos, revealSettings.translateSmooth * Time.deltaTime); 
        }
    }

    void ReavealLinearlyFade()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            //give the buttons a position to move towards
            Vector3 targetPos;
            RectTransform buttonRect = buttons[i].GetComponent<RectTransform>();
            //set size
            buttonRect.sizeDelta = new Vector2(buttonScaler.newButtonSize.x, buttonScaler.newButtonSize.y);
            //set pos
            targetPos.x = linearSpawner.direction.x * ((i + linearSpawner.buttonNumOffset) * (buttonRect.sizeDelta.x + linearSpawner.buttonSpacing)) + transform.position.x;
            targetPos.y = linearSpawner.direction.y * ((i + linearSpawner.buttonNumOffset) * (buttonRect.sizeDelta.y + linearSpawner.buttonSpacing)) + transform.position.y;
            targetPos.z = 0;

            ButtonFader previousButtonFader;
            if (i > 0)
                previousButtonFader = buttons[i - 1].GetComponent<ButtonFader>();
            else
                previousButtonFader = null;
            ButtonFader buttonFader = buttons[i].GetComponent<ButtonFader>();

            if (previousButtonFader) //first button won't have a previous button
            {
                if (previousButtonFader.faded)
                {
                    buttons[i].transform.position = targetPos;
                    if (buttonFader)
                        buttonFader.Fade(revealSettings.fadeSmooth);
                    else
                        Debug.LogError("You want to fade your buttons, but they need a ButtonFader script to be attatched first");
                }
            }
            else
                buttons[i].transform.position = targetPos;
            if (buttonFader)
                buttonFader.Fade(revealSettings.fadeSmooth); //for the first button in the array
            else
                Debug.LogError("You want to fade your buttons, but they need a ButtonFader script to be attatched first");
        }
    }

    void RevealCircularNormal() 
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            //find angle
            float angleDistance = circularSpawner.angle.maxAngle - circularSpawner.angle.minAngle;
            float targetAngle = circularSpawner.angle.maxAngle + (angleDistance / buttons.Count) * i;
            //find pos
            Vector3 targetPos = transform.position + Vector3.right * circularSpawner.distFromBrancher;
            targetPos = RotateAroundPivot(targetPos, transform.position, targetAngle);
            RectTransform buttonRect = buttons[i].GetComponent<RectTransform>();
            //resize buttons
            buttonRect.sizeDelta = new Vector2(buttonScaler.newButtonSize.x, buttonScaler.newButtonSize.y);

            buttonRect.position = Vector3.Lerp(buttonRect.position, targetPos, revealSettings.translateSmooth * Time.deltaTime);
        }
    }

    void RevealCircularFade()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            //find angle
            float angleDistance = circularSpawner.angle.maxAngle - circularSpawner.angle.minAngle;
            float targetAngle = circularSpawner.angle.maxAngle + (angleDistance / buttons.Count) * i;
            //find pos
            Vector3 targetPos = transform.position + Vector3.right * circularSpawner.distFromBrancher;
            targetPos = RotateAroundPivot(targetPos, transform.position, targetAngle);
            RectTransform buttonRect = buttons[i].GetComponent<RectTransform>();
            //resize buttons
            buttonRect.sizeDelta = new Vector2(buttonScaler.newButtonSize.x, buttonScaler.newButtonSize.y);

            ButtonFader previousButtonFader;
            if (i > 0)
                previousButtonFader = buttons[i - 1].GetComponent<ButtonFader>();
            else
                previousButtonFader = null;
            ButtonFader buttonFader = buttons[i].GetComponent<ButtonFader>();

            if (previousButtonFader) //first button won't have a previous button
            {
                if (previousButtonFader.faded)
                {
                    buttons[i].transform.position = targetPos;
                    if (buttonFader)
                        buttonFader.Fade(revealSettings.fadeSmooth);
                    else
                        Debug.LogError("You want to fade your buttons, but they need a ButtonFader script to be attatched first");
                }
            }
            else
                buttons[i].transform.position = targetPos;
            if (buttonFader)
                buttonFader.Fade(revealSettings.fadeSmooth); //for the first button in the array
            else
                Debug.LogError("You want to fade your buttons, but they need a ButtonFader script to be attatched first");
        }
    }


    Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, float angle)
    {

        Vector3 targetPoint = point - pivot;
        targetPoint = Quaternion.Euler(0, 0, angle) * targetPoint;
        targetPoint += pivot;
        return targetPoint;

    }

    void ClearCommonButtonBranchers()
    {
        GameObject[] branchers = GameObject.FindGameObjectsWithTag("ButtonBrancher");
        foreach(GameObject brancher in branchers)
        {

            //check if the parent of this brancher is the same as the brancher we are currently looking at
            if (brancher.transform.parent == transform.parent)
            {
                //remove the brancher's buttons to keep things tidy
                ButtonBrancher bb = brancher.GetComponent<ButtonBrancher>();
                for (int i = bb.buttons.Count - 1; i >= 0; i--)
                    Destroy(bb.buttons[i]);
                bb.buttons.Clear();
            }
        }
    }
}

