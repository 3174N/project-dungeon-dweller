﻿using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    #region
    public Weapon currentWeapon;
    public StaminaBar staminaBar;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public GameObject deathMenu;
    public GameObject noRevDeathMenu;
    public GameObject[] canvases;

    public FixedJoystick joystick;

    float waitTime;

    public float damageBonus;
    public float GetDamage { get { return damageBonus + currentWeapon.damage; } }

    public Text damageText;

    bool hasRevived;

    playerMovement player;
    float playerSpeed;

    public Animator animator;
    SpriteRenderer spriteRenderer;
    GameManager gameManager;
    #endregion

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        player = GetComponentInParent<playerMovement>();

        waitTime = currentWeapon.delay;
        staminaBar.SetMaxStamina(currentWeapon.delay);

        damageText.text = "DAMAGE: " + (damageBonus + currentWeapon.damage).ToString();

        hasRevived = false;

        gameManager = GameObject.FindObjectOfType<GameManager>();
        damageBonus = gameManager.startingDamage;
    }

    // Update is called once per frame
    void Update()
    { 
        spriteRenderer.sprite = currentWeapon.sprite;

        if (!Mathf.Approximately(joystick.Vertical, 0f) || !Mathf.Approximately(joystick.Horizontal, 0f))
        {
            player.lookDirection.Set(joystick.Horizontal, joystick.Vertical);

            if (waitTime <= 0)
            {
                animator.SetTrigger("Attack");

                currentWeapon.Attack(attackPoint, enemyLayers);

                waitTime = currentWeapon.delay;
                if (player.playerSpeed < 15)
                { 
                    waitTime = currentWeapon.delay - ((Mathf.Pow(-0.05f * player.playerSpeed, 2f) + (player.playerSpeed) / 10) / 2);
                    if (waitTime <= 0)
                    {
                        waitTime = 0.1f;
                    }
                    staminaBar.SetMaxStamina(waitTime);
                }
                else
                {
                    waitTime = currentWeapon.delay - ((Mathf.Pow(-0.05f * 15, 2f) + (15) / 10) / 2);
                    staminaBar.SetMaxStamina(waitTime);
                }
                staminaBar.SetStamina(waitTime);
            }
        }

        waitTime -= Time.deltaTime;
        staminaBar.SetStamina(waitTime);

    }
    
    /// <summary>
    /// Changes the weapon
    /// </summary>
    /// <param name="weapon"></param>
    public void ChangeWeapon(Weapon weapon)
    {
        if (currentWeapon.weaponObject != null)
        {
            Vector2 newPos = new Vector2(transform.position.x + UnityEngine.Random.Range(0, 1),
                    transform.position.y + UnityEngine.Random.Range(0, 1));
            Instantiate(currentWeapon.weaponObject, newPos, Quaternion.identity);
        }

        currentWeapon = weapon;

        waitTime = currentWeapon.delay;
        staminaBar.SetMaxStamina(currentWeapon.delay);

        damageText.text = "DAMAGE: " + (damageBonus + currentWeapon.damage).ToString();
    }

    public void OnDeath()
    {
        if (!hasRevived)
            deathMenu.SetActive(true);
        else
            noRevDeathMenu.SetActive(true);

        foreach (GameObject canvas in canvases)
        {
            canvas.SetActive(false);
        }

        hasRevived = true;
    }

    public void ChangeDamage(float amount)
    {
        damageBonus += amount;

        damageText.text = "DAMAGE: " + (damageBonus + currentWeapon.damage).ToString();
    }

    private void OnDrawGizmos()
    {
        // cache previous Gizmos settings
        Color prevColor = Gizmos.color;
        Matrix4x4 prevMatrix = Gizmos.matrix;

        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;

        Vector3 boxPosition = attackPoint.position;

        // convert from world position to local position 
        boxPosition = transform.InverseTransformPoint(boxPosition);

        Vector3 boxSize = new Vector3(1.5f, currentWeapon.range, 0);
        Gizmos.DrawWireCube(boxPosition, boxSize);

        // restore previous Gizmos settings
        Gizmos.color = prevColor;
        Gizmos.matrix = prevMatrix;
    }
}
