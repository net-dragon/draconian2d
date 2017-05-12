using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : ScriptableObject
{
    public string aName = "New Ability";
    public Sprite Icon;
    public float staminaCost = 0f;
    public string abilityButton;
    public Color color;

    public abstract void Initialize(GameObject obj);

    public abstract void TriggerAbility();

    public abstract void ReleaseAbility();
}
