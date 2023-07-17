using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInput : MonoBehaviour
{

    internal Vector2 moveInput;
    internal Vector2 lookInput;

    Player player;
    PlayerStats stats;
    GameManager gameManager;

    private _playerActions _actions;
    public bool isSprinting;
    public bool isShooting;
    bool hasShotThisFrame;

    public float shotTimer;
    bool shouldFire;

    private void Awake()
    {
        _actions = new _playerActions();
        
    }



    public void Start(){
        player = GetComponent<Player>();
        stats = GetComponent<PlayerStats>();
        gameManager = GameManager.Instance;
        shotTimer = stats.timeBetweenRegularShots;
    }

    private void Update() {
        isShooting = _actions.Player.Fire.ReadValue<float>() > 0;
        isSprinting = _actions.Player.Sprint.ReadValue<float>() > 0;
        FireshotTimer();


    }

    private void LateUpdate() {
        IsFiring();
        IsSprinting();
    }

    private void FireshotTimer() {
        if (shotTimer > 0) {
            shotTimer -= Time.deltaTime;
            shouldFire = false;
        } else if (stats.powerAvail < stats.maxPower * 0.1f && shotTimer <= 0) {
            shouldFire = false;
        } else {
            shouldFire = true;
        }
    }

    void IsFiring() {
        if (isShooting && shouldFire)
        {
            player.FireWeapon();
            shotTimer = stats.timeBetweenRegularShots;
        }

    }

    void IsSprinting() {
        if (isSprinting) {
            player.isSprinting = true;
        } else {
            player.isSprinting = false;
        }
    }




    public void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    public void OnSprint() { 
 
    }

    public void OnLook(InputValue value) {
        lookInput = value.Get<Vector2>();
    }

    public void OnAltFire() { 
        
    }

    public void OnPause() {
        gameManager.isPaused = !gameManager.isPaused;
    }

    private void OnEnable()
    {
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }

}
