using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [Header("Stats")]
    public float m_Speed;
    public float m_SprintSpeed;
    public float m_BulletSpeed;

    public float v_SprintDraw;
    public float v_ShootDraw;
    public float v_BigShootDraw;
    public float v_AmbientRecharge;
    public float v_PickupRange;

    public float powerAvail;
    public float maxPower;
    public float timer;

    public float timeBetweenRegularShots;
    public float timeBetweenBigShots;

    public int playerHP;
    public int playerScore;
    public int playerExp;
    public int playerLevel;

    float playerHPLastFrame;

    // Start is called before the first frame update
    void Start() {
        playerHPLastFrame = playerHP;
        powerAvail = maxPower;
        timer = 0;
    }


    void LateUpdate() {
        playerHPLastFrame = playerHP;
    }

    // Update is called once per frame
    void Update(){
        powerAvail = Mathf.Clamp(powerAvail, 0, maxPower);
        playerHP = Mathf.Clamp(playerHP, 0, 5);
    }
}
