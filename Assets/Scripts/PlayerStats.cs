using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
	private int _maxStamina;
	private float _currentStamina;
	private float _staminaRechargeDelay;
	private float _staminaRechargeRate;

	public PlayerStats()
	{
	_maxStamina = 0;
	_currentStamina = 0;
	_staminaRechargeDelay = 0;
	_staminaRechargeRate = 0;
	}
	/**
	 * @param currentStamina
	 * @param maxStamina
	 **/
	public PlayerStats(int maxStamina, float staminaRechargeDelay, float staminaRechargeRate , float currentStamina)
	{
		_maxStamina = maxStamina;
		_currentStamina = currentStamina;
		_staminaRechargeDelay = staminaRechargeDelay;
		_staminaRechargeRate = staminaRechargeRate;
	}
	/**
	 * @param  int currentStamina = player's current Stamina
	 * @return _currentStamina
	 **/
	public float getCurrentStamina()
	{
		return _currentStamina;
	}
	/**
	 * @param  int MaxStamina = player's Max
	 * @return _MaxStamina
	 **/
	public int getMaxStamina()
	{
		return _maxStamina;
	}
	/**
	 * @param  float stamina recharge delay = delay for stamina to recharge
	 * @return _staminarechargedelay
	 **/
	public float getStaminaRechargeDelay()
	{
		return _staminaRechargeDelay;
	}
	/**
	 * @param  float stamina recharge rate = rate of stamina recharge
	 * @return _staminarechargerate
	 **/
	public float getStaminaRechargeRate()
	{
		return _staminaRechargeRate;
	}
	/**
	 * @param currentStamina 
	 **/
	public void setCurrentStamina(float currentStamina)
	{
		_currentStamina = currentStamina;
	}
	/**
	 * @param MaxStamina 
	 **/
	public void setMaxStamina(int maxStamina)
	{
		_maxStamina = maxStamina;
	}
	/**
	 * @param recharge delay 
	 **/
	public void setStaminaRechargeDelay(float staminaRechargeDelay)
	{
		_staminaRechargeDelay = staminaRechargeDelay;
	}
	/**
	 * @param recharge rate 
	 **/
	public void setStaminaRechargeRate(float staminaRechargeRate)
	{
		_staminaRechargeRate = staminaRechargeRate;
	}

}

