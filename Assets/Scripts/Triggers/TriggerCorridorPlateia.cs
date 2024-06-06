using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCorridorPlateia : MonoBehaviour
{
    [SerializeField] GameObject enemies;

    [SerializeField] GameObject lights;

    bool hasPassed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPassed)
        {
            hasPassed = true;

            enemies.SetActive(true);

            lights.SetActive(true);
        }
    }
}
