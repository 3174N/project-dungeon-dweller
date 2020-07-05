﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("SHOP")]
    public int gems;

    public int speedUses;
    public int damageUses;
    public int healthUses;
    public int coinsUses;

    [Header("STARTING BONUSES")]
    public int startingCoins;
    public int startingSpeed;
    public int maxHealth = 300;
    public int startingDamage;

    [Header("REWARDS")]
    public int enemies;

    private void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(Instance);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        LoadShop();
    }

    public void SaveShop()
    {
        SaveSystem.SaveShop(this);
    }

    public void LoadShop()
    {
        ShopData data = SaveSystem.LoadShop();

        gems = data.gems;

        speedUses = data.speedUses;
        damageUses = data.damageUses;
        healthUses = data.healthUses;
        coinsUses = data.coinsUses;
    }
}