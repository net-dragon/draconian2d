using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (menuName = "Abilities/DashAbility")]
public class DashAbility : Ability
{
    public float dashCost = 50f;
    public float chargeStart = 100f;
    public float initialDashForce = 50f;
    public float dashIncrement = 1f;
    public float maxDashForce = 100f;
    public float dashStop = 1f;

    private BasicDashAbility dash;

    public float currentStamina
    {
        get { return currentStamina; }
        set { currentStamina = value; }
    }

    //Already in BasicDashAbility script. Shows error
    //In this script.
    /*public bool dashing
    {
        get { return dashDuration < dashStop; }
    }*/

    public override void Initialize(GameObject obj)
    {
        dash = obj.GetComponent<BasicDashAbility>();
        dash.Initialize();
        dash.dashCost = dashCost;
        dash.chargeStart = chargeStart;
        dash.initialDashForce = initialDashForce;
        dash.dashIncrement = dashIncrement;
        dash.maxDashForce = maxDashForce;
        dash.dashStop = dashStop;
    }

    public override void TriggerAbility()
    {
        dash.StartAbility();
    }

    public override void ReleaseAbility()
    {
        dash.StopAbility();
    }
}
