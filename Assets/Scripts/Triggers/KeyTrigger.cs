using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : MonoBehaviour
{
    [SerializeField] Transform teleportPosition;

    [SerializeField] GameObject[] lights;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.isControlable = false;

            other.GetComponent<PlayerController>().direction = Vector3.zero;

            other.GetComponent<PlayerController>().enabled = false;

            FadeManager.Instance.StartFadeOutAndIn();

            StartCoroutine(Sequence(other.gameObject));
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
