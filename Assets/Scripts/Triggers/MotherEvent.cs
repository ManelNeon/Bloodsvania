using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherEvent : MonoBehaviour
{
    [SerializeField] GameObject bossObject;

    [SerializeField] GameObject motherObject;

    public void StartMotherEvent()
    {
        StartCoroutine(EventCoroutine());
    }

    IEnumerator EventCoroutine()
    {
        GameObject player = FindObjectOfType<PlayerController>().gameObject;

        GameManager.Instance.isControlable = false;

        player.GetComponent<CombatController>().isFightingBoss = true;

        player.GetComponent<CharacterController>().enabled = false;

        player.GetComponent<PlayerController>().enabled = false;

        yield return new WaitForSeconds(1.5f);

        player.transform.position = transform.position;

        bossObject.SetActive(true);

        motherObject.SetActive(false);

        AudioManager.Instance.PlayMusic(AudioManager.Instance.bossMusic, false);

        yield return new WaitForSeconds(1.5f);

        bossObject.GetComponent<BossEnemyManager>().isFading = true;

        player.GetComponent<CombatController>().EnableFighting();

        player.GetComponent<CharacterController>().enabled = true;

        player.GetComponent<PlayerController>().enabled = true;

        GameManager.Instance.isControlable = true;

        yield break;
    }
}
