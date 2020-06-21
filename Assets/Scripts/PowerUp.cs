﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    #region variables 
    public float amount;

    public bool speed, damage, health;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerMovement player = other.GetComponent<playerMovement>();
        if (player != null)
        {
            Apply(player);

            Destroy(gameObject);
        }
    }

    public void Apply(playerMovement player)
    {
        if (speed)
            player.ChangeSpeed(amount);
        else if (damage)
            player.gameObject.GetComponentInChildren<PlayerCombat>().ChangeDamage(amount);
        else if (health)
            player.gameObject.GetComponent<Combat>().ChangeHealth(amount);
    }

    public void Randomize()
    {
        int rand = Random.Range(1, 3);
        if (rand == 1)
        {
            damage = true;
            amount = 7;
        }
        else if (rand == 2)
        {
            speed = true;
            amount = 3;
        }
        else
        {
            health = true;
            amount = 20;
        }
    }
}
