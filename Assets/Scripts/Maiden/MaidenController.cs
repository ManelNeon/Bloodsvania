using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaidenController : NPC //it is a children of the NPC script
{
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

    [SerializeField] TextMeshProUGUI levelText;

    public override void StartDialogue()
    {
        base.StartDialogue();

        GetComponent<Animator>().SetTrigger("Talk");
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
            GetComponent<Animator>().SetTrigger("EndTalk");

            if (hasEvent)
            {
                for (int i = 0; i < eventObjects.Length; i++)
                {
                    eventObjects[i].SetActive(!eventObjects[i].activeInHierarchy);
                }

                hasEvent = false;
            }

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
    }

    //we change the stats on the maiden screen to the one that player has
    void ChangeStats()
    {
        levelText.text = "Level: " + GameManager.Instance.level;

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

            GetComponent<Animator>().SetTrigger("LevelUp");

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

            GetComponent<Animator>().SetTrigger("LevelUp");

            ChangeStats();
        }
    }

    void AddingDamage()
    {
        if (GameManager.Instance.fulguriteValue > GameManager.Instance.fulguriteToLevel)
        {
            GameManager.Instance.AddDamage(25);

            GameManager.Instance.fulguriteValue -= GameManager.Instance.fulguriteToLevel;

            GameManager.Instance.FulguriteNeeded();

            GetComponent<Animator>().SetTrigger("LevelUp");

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

            GetComponent<Animator>().SetTrigger("LevelUp");

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

            GetComponent<Animator>().SetTrigger("LevelUp");

            ChangeStats();
        }
    }

    //adding exit menu stat
    void ExitMenu()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i].name != "FocusCamera")
            {
                cameras[i].GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2;
                cameras[i].GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300;
                cameras[i].SetActive(true);
            }
            else
            {
                cameras[i].SetActive(false);
            }
        }

        GameManager.Instance.isControlable = true;

        maidenScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
    }
}
