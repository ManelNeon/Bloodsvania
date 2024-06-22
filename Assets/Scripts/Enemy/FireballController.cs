using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    Vector3 playerPosition;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    public void Setup(Vector3 playerPosition)
    {
        this.playerPosition = new Vector3( playerPosition.x - this.transform.position.x , playerPosition.y - this.transform.position.y, playerPosition.z - this.transform.position.z);
    }

    void Update()
    {
        transform.position += playerPosition * Time.deltaTime * 2.75f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CombatController>().TakeDamage(30);

            Destroy(gameObject);
        }
    }
}