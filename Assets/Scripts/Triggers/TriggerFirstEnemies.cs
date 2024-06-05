using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFirstEnemies : MonoBehaviour
{
    [SerializeField] GameObject enemyGroup1;

    [SerializeField] GameObject doorIn;

    [SerializeField] GameObject[] lights;

    bool hasPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            hasPlayed = true;

            StartCoroutine(EventCoroutine(other.transform));
        }
    }

    IEnumerator EventCoroutine(Transform playerPosition)
    {
        doorIn.SetActive(true);

        FadeManager.Instance.StartFadeOutAndIn();

        playerPosition.GetComponent<PlayerController>().enabled = false;

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.SetActive(true);
        }

        playerPosition.GetComponent<CharacterController>().enabled = false;

        playerPosition.position = new Vector3(-53.4f, playerPosition.transform.position.y , -5.6f);

        enemyGroup1.SetActive(true);

        yield return new WaitForSeconds(1.7f);

        playerPosition.GetComponent<CharacterController>().enabled = true;

        playerPosition.GetComponent<PlayerController>().enabled = true;

        yield break;
    }
}
