using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaidenController : NPC
{
    [SerializeField] Button hpButton;

    [SerializeField] Button bloodButton;

    [SerializeField] Button compositionButton;

    [SerializeField] Button exitButton;

    [SerializeField] GameObject maidenScreen;

    [SerializeField] TextMeshProUGUI hpValue;

    [SerializeField] TextMeshProUGUI bloodValue;

    [SerializeField] TextMeshProUGUI compositionValue;

    [SerializeField] TextMeshProUGUI fulguriteAvailableText;

    [SerializeField] TextMeshProUGUI fulguriteNeededText;

    PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public override void NextLine()
    {
        if (index < dialogues.Length - 1)
        {
            index++;
            displayText.text = "";
        }
        else
        {
            npcTextBox.SetActive(false);

            ChangeStats();

            AddingListeners();

            maidenScreen.SetActive(true);

            isPlaying = false;
        }
    }

    void ChangeStats()
    {
        hpValue.text = "HP: " + playerStats.hpValue;

        bloodValue.text = "Blood: " + playerStats.bloodValue;

        compositionValue.text = "Composition: " + playerStats.compositionValue;

        fulguriteAvailableText.text = "Fulgurite: " + playerStats.fulguriteValue;

        fulguriteNeededText.text = playerStats.fulguriteToLevel + " Fulgurite to Level Up";
    }

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

    void ExitMenu()
    {
        maidenScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
    }
}
