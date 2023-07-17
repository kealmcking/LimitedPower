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
    GameManager gameManager;

    private _playerActions _actions;
    public bool isSprinting;
    public bool isShooting;
    bool hasShotThisFrame;

    private void Awake()
    {
        _actions = new _playerActions();
    }



    public void Start(){
        player = GetComponent<Player>();
        gameManager = GameManager.Instance;
    }

    private void Update() {
        isShooting = _actions.Player.Fire.ReadValue<float>() > 0;
        isSprinting = _actions.Player.Sprint.ReadValue<float>() > 0;

        if (isShooting)
        {
            player.shouldFire = true;
        }
        else {
            player.shouldFire = false;
        }

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
