using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [TextArea] public string[] dialogues;

    public  GameObject npcTextBox;

    public TextMeshProUGUI displayText;

    [HideInInspector] public int index;

    [HideInInspector] public bool isPlaying;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isPlaying)
            {
                if (displayText.text == dialogues[index])
                {
                    NextLine();
                }
                else 
                {
                    StopAllCoroutines();
                    displayText.text = dialogues[index];
                }
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;

        isPlaying = true;

        npcTextBox.SetActive(true);

        displayText.text = "";

        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;

        GameObject.Find("Player").GetComponent<PlayerController>().enabled = false;

    }

    public virtual void NextLine()
    {
        if (index < dialogues.Length - 1)
        {
            index++;
            displayText.text = "";
        }
        else
        {
            npcTextBox.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isPlaying = false;

            GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
        }
    }
}