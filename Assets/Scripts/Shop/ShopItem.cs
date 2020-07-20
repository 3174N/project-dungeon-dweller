﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    #region variables
    public bool sellWeapon;
    public Weapon[] weapons;
    public bool sellPowerUp;
    public GameObject[] powerUps;

    public Text priceText;

    int price;
    public int GetPrice { get { return price; } }

    Weapon weapon;
    GameObject powerUp;
    GameObject[] takenWeapons;

    Image image;
#endregion

    private void Start()
    {
        image = GetComponent<Image>();

        if (sellWeapon)
        {
            weapon = weapons[(int)Random.Range(0f, weapons.Length)];
            price = (int)Random.Range(weapon.minPrice, weapon.maxPrice);
            image.sprite = weapon.sprite;
        }
        else
        {
            powerUp = powerUps[(int)Random.Range(0f, powerUps.Length)];
            powerUp.GetComponent<PowerUp>().Randomize();
            price = (int)Random.Range(0f, 20f);
            image.sprite = powerUp.GetComponent<SpriteRenderer>().sprite;
        }
           
        priceText.text = price.ToString();
    }

    public void Refresh()
    {
        gameObject.SetActive(true);

        if (sellWeapon)
        {
            weapon = weapons[(int)Random.Range(0f, weapons.Length)];
            price = (int)Random.Range(weapon.minPrice, weapon.maxPrice);
            image.sprite = weapon.sprite;
        }
        else
        {
            powerUp = powerUps[(int)Random.Range(0f, powerUps.Length)];
            powerUp.GetComponent<PowerUp>().Randomize();
            price = (int)Random.Range(0f, 20f);
            image.sprite = powerUp.GetComponent<SpriteRenderer>().sprite;
        }

        priceText.text = price.ToString();
    }

    public void Buy()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (player.GetCoins >= price)
        {
            player.ChangeCoins(-price);

            if (sellWeapon)
                player.gameObject.GetComponentInChildren<PlayerCombat>().ChangeWeapon(weapon);
            else
            {
                powerUp.GetComponent<PowerUp>().Apply(player.GetComponent<playerMovement>());
            }

            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(player.GetCoins + " / " + price);
            // Need to find a way to inform the player
        }
    }
}
