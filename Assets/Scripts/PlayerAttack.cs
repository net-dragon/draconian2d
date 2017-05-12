using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using System;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Ability ability;
    [SerializeField]
    private GameObject character;
    private Image myButtonImage;


    // Use this for initialization
    void Start()
    {
        Initialize(ability, character);
    }

    public void Initialize(Ability selectedAbility, GameObject character)
    {
        ability = selectedAbility;
        myButtonImage = GetComponent<Image>();
        myButtonImage.sprite = ability.Icon;
        ability.Initialize(character);
        myButtonImage.color = ability.color;
    }

    // Update is called once per frame
    void Update()
    {

    }//empty
    private void FixedUpdate() //clean
    {
        if (Input.GetButton(ability.abilityButton))
        {
            ability.TriggerAbility();
        }

        if (Input.GetButtonUp(ability.abilityButton))
        {
            ability.ReleaseAbility();
        }
    }
}
