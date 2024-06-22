using Cinemachine;
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

    public GameObject[] cameras;

    [Header("Event Variables")]
    public bool hasEvent;

    public GameObject[] eventObjects;

    public bool isFading;

    public bool isMother;

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
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i].name != "FocusCamera")
            {
                cameras[i].GetComponent<CinemachineFreeLook>().enabled = false;
                cameras[i].SetActive(false);
            }
            else
            {
                cameras[i].GetComponent<CinemachineVirtualCamera>().Follow = this.transform;

                cameras[i].SetActive(true);
            }
        }

        GameManager.Instance.isControlable = false;

        index = 0;

        isPlaying = true;

        npcTextBox.SetActive(true);

        displayText.text = dialogues[index];

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
            if (hasEvent)
            {
                if (isFading)
                {
                    FadeManager.Instance.StartFadeOutAndIn(0);

                    if (isMother)
                    {
                        GetComponentInParent<MotherEvent>().StartMotherEvent();
                    }
                }
                else
                {
                    for (int i = 0; i < eventObjects.Length; i++)
                    {
                        eventObjects[i].SetActive(!eventObjects[i].activeInHierarchy);
                    }

                    hasEvent = false;
                }
            }

            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i].name != "FocusCamera")
                {
                    cameras[i].GetComponent<CinemachineFreeLook>().enabled = true;
                    cameras[i].SetActive(true);
                }
                else
                {
                    cameras[i].SetActive(false);
                }
                
            }

            GameManager.Instance.isControlable = true;

            npcTextBox.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isPlaying = false;

            GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
        }
    }
}