using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLights : MonoBehaviour
{
    [SerializeField] GameObject light1;

    [SerializeField] GameObject light2;

    [SerializeField] GameObject light3;

    [SerializeField] GameObject secondRoomLights;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !light1.activeInHierarchy)
        {
            other.GetComponent<PlayerController>().direction = Vector3.zero;
            Debug.Log("Passed");
            StartCoroutine(LightShowcase());
        }
    }

    IEnumerator LightShowcase()
    {
        GameManager.Instance.isControlable = false;

        yield return new WaitForSeconds(1f);

        light1.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        light2.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        light3.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        GameManager.Instance.isControlable = true;

        yield return new WaitForSeconds(1.5f);

        secondRoomLights.SetActive(true);

        yield break;
    }
}
