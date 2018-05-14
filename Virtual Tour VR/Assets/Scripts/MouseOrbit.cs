﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOrbit : MonoBehaviour {

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    float x = 0.0f;
    float y = 0.0f;

    public bool hide;
    public bool inMenu;

    private GameState gameState = GameState.PLAYING;
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        DialogueSystem.inMenu += changeMenu;
    }
    private void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        DialogueSystem.inMenu -= changeMenu;
    }

    void LateUpdate()
    {

        if (gameState == GameState.PAUSED || gameState == GameState.WIN || inMenu)
        {
            return;
        }


        if (target)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            /*
            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                distance -= hit.distance;
            }
            */
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    void playerHide()
    {
        hide = !hide;
    }

    public void changeMenu(bool status)
    {

        inMenu = status;
    }
}
