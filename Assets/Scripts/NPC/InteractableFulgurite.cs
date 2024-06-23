using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFulgurite : NPC
{
    [SerializeField] int fulguriteToAdd;

    //function for the next line, that also deactivates the UI
    public override void NextLine()
    {
        if (index < dialogues.Length - 1)
        {
            index++;
            displayText.text = dialogues[index];
        }
        else
        {
            GameManager.Instance.AddFulgurite(fulguriteToAdd);

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

            npcTextBox.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isPlaying = false;

            GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
        }
    }
}
