using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAttackController : MonoBehaviour
{
    [SerializeField] BoxCollider triggerCollider;

    [SerializeField] GameObject damageParticles;

    [SerializeField] ParticleSystem[] warningParticles;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(DamageCoroutine());
    }

    IEnumerator DamageCoroutine()
    {
        yield return new WaitForSeconds(2f);

        damageParticles.SetActive(true);

        triggerCollider.enabled = true;

        yield return new WaitForSeconds(3f);

        damageParticles.SetActive(false);

        triggerCollider.enabled = false;

        FindObjectOfType<BossEnemyManager>().CooldownEarth();

        var main = damageParticles.GetComponent<ParticleSystem>().main;

        main.loop = false;

        for (int i = 0; i < warningParticles.Length; i++) 
        {
            main = warningParticles[i].GetComponent<ParticleSystem>().main;

            main.loop = false;
        }

        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);

        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CombatController>().TakeDamage(30);
        }
    }
}
