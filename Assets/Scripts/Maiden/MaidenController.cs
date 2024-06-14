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

    [SerializeField] Button dmgButton;

    [SerializeField] Button compositionButton;

    [SerializeField] Button reflexButton;

    [SerializeField] Button exitButton;

    [Header("Maiden Screen Object")]
    [SerializeField] GameObject maidenScreen;

    [Header("Stats Text Values")]
    [SerializeField] TextMeshProUGUI hpValue;

    [SerializeField] TextMeshProUGUI bloodValue;

    [SerializeField] TextMeshProUGUI dmgValue;

    [SerializeField] TextMeshProUGUI compositionValue;

    [SerializeField] TextMeshProUGUI reflexValue;

    [SerializeField] TextMeshProUGUI fulguriteAvailableText;

    [SerializeField] TextMeshProUGUI fulguriteNeededText;

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

                GameManager.Instance.currentHP = GameManager.Instance.hpValue;

                GameManager.Instance.currentRage = GameManager.Instance.bloodValue;

                GameManager.Instance.ChangingHPUI();

                GameManager.Instance.ChangingRageUI();

                AddingListeners();

                maidenScreen.SetActive(true);

                isPlaying = false;
            }
            else
            {
                npcTextBox.SetActive(false);

                GameManager.Instance.isControlable = true;

                GameManager.Instance.MainMenuSequenceFunction();
            }
        }
    }

    //we change the stats on the maiden screen to the one that player has
    void ChangeStats()
    {
        hpValue.text = "HP: " + GameManager.Instance.hpValue;

        bloodValue.text = "Blood: " + GameManager.Instance.bloodValue;

        dmgValue.text = "Damage: " + GameManager.Instance.dmgValue;

        compositionValue.text = "Composition: " + GameManager.Instance.compositionValue;

        reflexValue.text = "Reflex: " + GameManager.Instance.reflexValue;

        fulguriteAvailableText.text = "Fulgurite: " + GameManager.Instance.fulguriteValue;

        fulguriteNeededText.text = GameManager.Instance.fulguriteToLevel + " Fulgurite to Level Up";
    }

    //we firstly remove the listeners and then add them again (so that there isnt two listeners on the same button)
    void AddingListeners()
    {
        hpButton.onClick.RemoveAllListeners();
        bloodButton.onClick.RemoveAllListeners();
        dmgButton.onClick.RemoveAllListeners();
        compositionButton.onClick.RemoveAllListeners();
        reflexButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        hpButton.onClick.AddListener(AddingHealth);
        bloodButton.onClick.AddListener(AddingBlood);
        dmgButton.onClick.AddListener(AddingDamage);
        compositionButton.onClick.AddListener(AddingComposition);
        reflexButton.onClick.AddListener(AddingReflex);
        exitButton.onClick.AddListener(ExitMenu);
    }

    //adding health stat
    void AddingHealth()
    {
        if (GameManager.Instance.fulguriteValue > GameManager.Instance.fulguriteToLevel)
        {
            GameManager.Instance.AddHealth(25);

            GameManager.Instance.fulguriteValue -= GameManager.Instance.fulguriteToLevel;

            GameManager.Instance.FulguriteNeeded();

            ChangeStats();
        }
    }

    //adding blood stat
    void AddingBlood()
    {
        if (GameManager.Instance.fulguriteValue > GameManager.Instance.fulguriteToLevel)
        {
            GameManager.Instance.AddBlood(10);

            GameManager.Instance.fulguriteValue -= GameManager.Instance.fulguriteToLevel;

            GameManager.Instance.FulguriteNeeded();

            ChangeStats();
        }
    }

    void AddingDamage()
    {
        if (GameManager.Instance.fulguriteValue > GameManager.Instance.fulguriteToLevel)
        {
            GameManager.Instance.AddDamage(5);

            GameManager.Instance.fulguriteValue -= GameManager.Instance.fulguriteToLevel;

            GameManager.Instance.FulguriteNeeded();

            ChangeStats();
        }
    }

    //adding composition stat
    void AddingComposition()
    {
        if (GameManager.Instance.fulguriteValue > GameManager.Instance.fulguriteToLevel)
        {
            GameManager.Instance.AddComposition(1);

            GameManager.Instance.fulguriteValue -= GameManager.Instance.fulguriteToLevel;

            GameManager.Instance.FulguriteNeeded();

            ChangeStats();
        }
    }

    void AddingReflex()
    {
        if (GameManager.Instance.fulguriteValue > GameManager.Instance.fulguriteToLevel)
        {
            GameManager.Instance.AddReflex(5);

            GameManager.Instance.fulguriteValue -= GameManager.Instance.fulguriteToLevel;

            GameManager.Instance.FulguriteNeeded();

            ChangeStats();
        }
    }

    //adding exit menu stat
    void ExitMenu()
    {
        GameManager.Instance.isControlable = true;

        maidenScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
    }
}
