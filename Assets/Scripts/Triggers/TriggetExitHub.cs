using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggetExitHub : MonoBehaviour
{
    [SerializeField] GameObject lights;

    [SerializeField] GameObject door;

    [SerializeField] GameObject enemyGroup3;

    bool hasPassed;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPassed && other.CompareTag("Player"))
        {
            lights.SetActive(false);

            door.SetActive(true);

            enemyGroup3.SetActive(true);

            hasPassed = true;
        }
    }
}
