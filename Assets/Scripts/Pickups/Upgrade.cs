using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Upgrade", order = 1)]
public class Upgrade : ScriptableObject
{

    public int m_healthUpgrade;
    public float m_maxPowerUpgrade;
    public float m_ambientRechargeUpgrade;
    public float m_speedUpgrade;
    public float m_sprintSpeedUpgrade;
    public float m_shotSpeedUpgrade;
    public float m_pickupRangeUpgrade;

    public string m_name;
    public string m_description;

}
