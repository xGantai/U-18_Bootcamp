using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerSystem
{
    public int MaxPower;
    [HideInInspector]
    public int Power = 0;

    public void PowerBoost(int PowerBoostData)
    {
        Power += Mathf.Clamp(PowerBoostData, 0, MaxPower);
    }

    public void SetPower(int PowerData)
    {
        Power = Mathf.Clamp(PowerData, 0, MaxPower);
    }

    public void PowerMinus(int PowerMinusData)
    {
        Power -= Mathf.Clamp(PowerMinusData, 0, MaxPower);
    }
}
