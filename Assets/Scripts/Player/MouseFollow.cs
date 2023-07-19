using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class MouseFollow : MonoBehaviour
{

    enum CurrentInputType { Keyboard, Gamepad }


    private Vector2 previousCursorPosition;

    [SerializeField] float maxDistFromPlayer;
    GameObject player;
    PlayerInput input;
    private bool isFirstMove = true;


    Keyboard keyboard;
    Gamepad gamepad;

    CurrentInputType currentInputType;

    private void OnEnable()
    {
        InputUser.onChange += onInputDeviceChange;
    }

    private void OnDisable()
    {
        InputUser.onChange -= onInputDeviceChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        keyboard = Keyboard.current;
        gamepad = Gamepad.current;
        currentInputType = CurrentInputType.Keyboard;
        player = GameObject.Find("Player");
        input = player.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseMove();
        JoyStickMove();
    }


    void onInputDeviceChange(InputUser user, InputUserChange change, InputDevice device) {
        if (change == InputUserChange.ControlSchemeChanged) {
            UpdateControllerScheme(user.controlScheme.Value.name);
        }
    }

    void MouseMove() {
        if (currentInputType != CurrentInputType.Keyboard) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(input.lookInput);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;

        Vector3 playerToCursor = mousePosition - player.transform.position;
        Vector3 dir = playerToCursor.normalized;
        Vector3 cursorVector = dir * maxDistFromPlayer;
        Vector3 finalPos = player.transform.position + cursorVector;

        transform.position = finalPos;
    }

    void JoyStickMove()
    {
        if (currentInputType != CurrentInputType.Gamepad) return;

        Vector2 joystickInput = gamepad.rightStick.value;
        Vector2 playerPosition = player.transform.position;

        // Calculate the rotation angle based on the right thumbstick input
        float rotationAngle = Mathf.Atan2(joystickInput.y, joystickInput.x) * Mathf.Rad2Deg;

        // Calculate the position of the cursor around the player at the specified distance
        Vector2 cursorOffset = Quaternion.Euler(0f, 0f, rotationAngle) * Vector2.right * maxDistFromPlayer;
        Vector2 cursorPosition = playerPosition + cursorOffset;

        // Save the current cursor position
        previousCursorPosition = cursorPosition;

        transform.position = previousCursorPosition;
    }





    void UpdateControllerScheme(string schemeName) {
        if (schemeName.Equals("Gamepad")) {
            currentInputType = CurrentInputType.Gamepad;
        } else if (schemeName.Equals("Keyboard")) {
            currentInputType = CurrentInputType.Keyboard;
        }
    }
}
