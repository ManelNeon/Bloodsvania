using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterHub : MonoBehaviour
{
    [SerializeField] GameObject[] doorsHubs;

    [SerializeField] GameObject[] lights;

    bool hasPassed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPassed)
        {
            hasPassed = true;

            for (int i = 0; i < doorsHubs.Length; i++)
            {
                doorsHubs[i].SetActive(!doorsHubs[i].activeInHierarchy);
            }

            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(!lights[i].activeInHierarchy);
            }
        }
    }
}
