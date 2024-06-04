using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [TextArea] public string[] dialogues;

    [Header("UI References")]
    public  GameObject npcTextBox;

    public TextMeshProUGUI displayText;

    [HideInInspector] public int index;

    [HideInInspector] public bool isPlaying;

    void Update()
    {
        //if the player left clicks he gets the next line
        if (Input.GetMouseButtonDown(0))
        {
            if (isPlaying)
            {
                if (displayText.text == dialogues[index])
                {
                    NextLine();
                }
            }
        }
    }

    //function that starts the dialogue, is called when raycasted
    public void StartDialogue()
    {
        Time.timeScale = 0;

        index = 0;

        isPlaying = true;

        npcTextBox.SetActive(true);

        displayText.text = "";

        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;

        GameObject.Find("Player").GetComponent<PlayerController>().enabled = false;

    }

    //function for the next line, that also deactivates the UI
    public virtual void NextLine()
    {
        if (index < dialogues.Length - 1)
        {
            index++;
            displayText.text = dialogues[index];
        }
        else
        {
            Time.timeScale = 1;

            npcTextBox.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isPlaying = false;

            GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
        }
    }
}