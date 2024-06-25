using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : NPC
{
    [SerializeField] Transform teleportPosition;

    [SerializeField] GameObject[] lights;

    public override void NextLine()
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
                    cameras[i].GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2;
                    cameras[i].GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300;
                    cameras[i].SetActive(true);
                }
                else
                {
                    cameras[i].SetActive(false);
                }

            }

            GameManager.Instance.isControlable = false;

            GameObject other = GameObject.Find("Player");

            other.GetComponent<PlayerController>().direction = Vector3.zero;

            FadeManager.Instance.StartFadeOutAndIn(0);

            StartCoroutine(Sequence(other));

            npcTextBox.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isPlaying = false;
        }
    }

    IEnumerator Sequence(GameObject other)
    {
        other.GetComponent<CharacterController>().enabled = false;

        yield return new WaitForSeconds(1.5f);

        other.transform.position = teleportPosition.position;

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.SetActive(!lights[i].activeInHierarchy);
        }

        yield return new WaitForSeconds(1.5f);

        other.GetComponent<PlayerController>().enabled = true;

        other.GetComponent<CharacterController>().enabled = true;

        GameManager.Instance.isControlable = true;

        this.gameObject.SetActive(false);

        yield break;
    }
}
