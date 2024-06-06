using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterPlateia : MonoBehaviour
{
    [SerializeField] GameObject[] lightsAndDoor;

    bool hasPassed;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPassed && other.CompareTag("Player"))
        {
            hasPassed = true;

            for (int i = 0; i < lightsAndDoor.Length; i++)
            {
                lightsAndDoor[i].SetActive(!lightsAndDoor[i].activeInHierarchy);
            }
        }
    }
}
