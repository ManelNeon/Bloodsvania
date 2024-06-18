using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTriggerEvent : MonoBehaviour
{
    [SerializeField] GameObject[] eventObjects;

    bool hasPassed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPassed)
        {
            hasPassed = true;

            for (int i = 0; i < eventObjects.Length; i++)
            {
                eventObjects[i].SetActive(!eventObjects[i].activeInHierarchy);
            }
        }
    }
}
