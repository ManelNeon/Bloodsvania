using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerChangeScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.isControlable = false;

            other.GetComponent<PlayerController>().direction = Vector3.zero;

            StartCoroutine(ChangeSceneTrigger());
        }
    }

    IEnumerator ChangeSceneTrigger()
    {
        FadeManager.Instance.StartFadeOut();

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("BossFight");

        yield break;
    }
}
