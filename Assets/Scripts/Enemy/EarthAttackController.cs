using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAttackController : MonoBehaviour
{
    [SerializeField] BoxCollider triggerCollider;

    [SerializeField] GameObject damageParticles;

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

        yield return new WaitForSeconds(2);

        Destroy(gameObject);

        yield break;
    }
}
