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

            GameManager.Instance.isControlable = false;

            other.GetComponent<PlayerController>().direction = Vector3.zero;

            StartCoroutine(EventCoroutine(other.transform));
        }
    }

    IEnumerator EventCoroutine(Transform playerPosition)
    {
        doorIn.SetActive(true);

        FadeManager.Instance.StartFadeOutAndIn();

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

        GameManager.Instance.isControlable = true;

        yield break;
    }
}
