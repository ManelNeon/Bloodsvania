using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaidenController : NPC //it is a children of the NPC script
{
    [SerializeField] bool willEndDemo;

    [Header("Buttons")]
    [SerializeField] Button hpButton;

    [SerializeField] Button bloodButton;

    [SerializeField] Button compositionButton;

    [SerializeField] Button exitButton;

    [Header("Maiden Screen Object")]
    [SerializeField] GameObject maidenScreen;

    [Header("Stats Text Values")]
    [SerializeField] TextMeshProUGUI hpValue;

    [SerializeField] TextMeshProUGUI bloodValue;

    [SerializeField] TextMeshProUGUI compositionValue;

    [SerializeField] TextMeshProUGUI fulguriteAvailableText;

    [SerializeField] TextMeshProUGUI fulguriteNeededText;

    [Header("Players Stats")]
    PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        //because there will ever only be one Player Stats we can use the FindObjectOfType function
        playerStats = FindObjectOfType<PlayerStats>();
    }

    //we override the NextLine function, the difference is that when the dialogue ends, we activate the maiden screen and do the ChangeStats() and AddingListeners()
    public override void NextLine()
    {
        if (index < dialogues.Length - 1)
        {
            index++;
            displayText.text = dialogues[index];
        }
        else
        {
            if (!willEndDemo)
            {
                npcTextBox.SetActive(false);

                ChangeStats();

                playerStats.currentHP = playerStats.hpValue;

                playerStats.ChangingHPUI();

                AddingListeners();

                maidenScreen.SetActive(true);

                isPlaying = false;
            }
            else
            {
                npcTextBox.SetActive(false);

                Time.timeScale = 1;

                GameManager.Instance.MainMenuSequenceFunction();
            }
        }
    }

    //we change the stats on the maiden screen to the one that player has
    void ChangeStats()
    {
        hpValue.text = "HP: " + playerStats.hpValue;

        bloodValue.text = "Blood: " + playerStats.bloodValue;

        compositionValue.text = "Composition: " + playerStats.compositionValue;

        fulguriteAvailableText.text = "Fulgurite: " + playerStats.fulguriteValue;

        fulguriteNeededText.text = playerStats.fulguriteToLevel + " Fulgurite to Level Up";
    }

    //we firstly remove the listeners and then add them again (so that there isnt two listeners on the same button)
    void AddingListeners()
    {
        hpButton.onClick.RemoveAllListeners();
        bloodButton.onClick.RemoveAllListeners();
        compositionButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        hpButton.onClick.AddListener(AddingHealth);
        bloodButton.onClick.AddListener(AddingBlood);
        compositionButton.onClick.AddListener(AddingComposition);
        exitButton.onClick.AddListener(ExitMenu);
    }

    //adding health stat
    void AddingHealth()
    {
        if (playerStats.fulguriteValue > playerStats.fulguriteToLevel)
        {
            playerStats.AddHealth(25);

            playerStats.fulguriteValue -= playerStats.fulguriteToLevel;

            playerStats.FulguriteNeeded();

            ChangeStats();
        }
    }

    //adding blood stat
    void AddingBlood()
    {
        if (playerStats.fulguriteValue > playerStats.fulguriteToLevel)
        {
            playerStats.AddBlood(25);

            playerStats.fulguriteValue -= playerStats.fulguriteToLevel;

            playerStats.FulguriteNeeded();

            ChangeStats();
        }
    }

    //adding composition stat
    void AddingComposition()
    {
        if (playerStats.fulguriteValue > playerStats.fulguriteToLevel)
        {
            playerStats.AddComposition(25);

            playerStats.fulguriteValue -= playerStats.fulguriteToLevel;

            playerStats.FulguriteNeeded();

            ChangeStats();
        }
    }

    //adding exit menu stat
    void ExitMenu()
    {
        Time.timeScale = 1;

        maidenScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
    }
}
