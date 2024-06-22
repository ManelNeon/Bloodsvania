using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebreathController : MonoBehaviour
{
    Vector3 currentEulerAngles;

    ParticleSystem fireParticles;

    bool isGoingLeft;

    bool isCenter;

    bool isOver;

    private void Start()
    {
        fireParticles = GetComponent<ParticleSystem>();

        currentEulerAngles = transform.localEulerAngles;

        StartCoroutine(EndAttack());
    }

    private void Update()
    {
        if (!isGoingLeft && !isCenter)
        {
            currentEulerAngles += Vector3.up * Time.deltaTime * 45f;
        }
        else if (isGoingLeft && !isCenter)
        {
            currentEulerAngles += Vector3.down * Time.deltaTime * 45f;
        }
        else if (isCenter & isOver)
        {
            ParticleSystem.MainModule main = fireParticles.main;

            main.loop = false;
        }

        transform.eulerAngles = currentEulerAngles;
    }

    IEnumerator EndAttack()
    {
        isCenter = true;

        yield return new WaitForSeconds(1f);

        isCenter = false;

        yield return new WaitForSeconds(1.5f);

        isGoingLeft = true;

        yield return new WaitForSeconds(3);

        isGoingLeft = false;

        yield return new WaitForSeconds(1.5f);

        isCenter = true;

        isOver = true;

        FindObjectOfType<BossEnemyManager>().CooldownBreath();

        GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(2);

        Destroy(gameObject);

        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CombatController>().TakeDamage(35);
        }
    }
}
