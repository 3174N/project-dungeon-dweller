﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    #region variables
    public float playerSpeed = 3.0f;
    public FixedJoystick joystick;

    public Text speedText;

    Vector2 movement;
    public Vector2 lookDirection = new Vector2(1, 0);

    Rigidbody2D rigidBody2;
    Animator animator;
    AudioSource audioSource;

    bool isPlaying;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        speedText.text = "SPEED: " + playerSpeed.ToString();

        playerSpeed += GameObject.FindObjectOfType<GameManager>().startingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");

        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;

        if (!Mathf.Approximately(movement.x, 0.0f) || !Mathf.Approximately(movement.y, 0.0f))
        {
            lookDirection.Set(movement.x, movement.y);
            lookDirection.Normalize();

            if (!isPlaying && FindObjectOfType<GameManager>().sfxOn)
            {
                isPlaying = true;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
            isPlaying = false;
        }

        animator.SetFloat("Look X", Mathf.RoundToInt(lookDirection.x));
        animator.SetFloat("Speed", movement.magnitude);
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        rigidBody2.MovePosition(rigidBody2.position + movement * playerSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Cahnges the speed of the player
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeSpeed(float amount)
    {
        playerSpeed += amount;

        if (playerSpeed == 15)
        {
            speedText.text = "SPEED: " + playerSpeed.ToString() + " (MAX)";
        }
        else
        {
            speedText.text = "SPEED: " + playerSpeed.ToString();
        }

        playerSpeed = Mathf.Clamp(playerSpeed, 3, 15);
    }
}

