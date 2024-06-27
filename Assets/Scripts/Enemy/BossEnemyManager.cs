using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using static UnityEngine.Rendering.DebugUI;

public class BossEnemyManager : MonoBehaviour
{
    [Header("Boss Variables")]
    [SerializeField] float maxHP;

    [HideInInspector] public float currentHP;

    [HideInInspector] public bool canDamage;

    [HideInInspector] public int hitsOnArm;

    [SerializeField] EnemyBoss enemyHead;

    [SerializeField] CombatController playerCombat;

    [SerializeField] GameObject[] armsGlowyParticles;

    [SerializeField] GameObject headGlowyParticle;

    [SerializeField] Transform playerPosition;

    [SerializeField] GameObject bossModel;

    [Header("Boss Bar")]
    [SerializeField] RectTransform healthBarMaskTransform;

    [SerializeField] RectTransform damageBarMaskTransform;

    float healthBarMaskWidth;

    [Header("Boss Bar Fade")]
    [SerializeField] CanvasGroup bossHealthbar;

    [SerializeField] float fadeDuraction;

    [HideInInspector] public bool isFading;

    bool isFadingOut;

    float m_Timer;

    [Header("Boss Attacks")]
    [SerializeField] GameObject fireballPrefab;
    
    [SerializeField] Transform fireballSpawnPoint;

    [SerializeField] GameObject firebreathPrefab;

    [SerializeField] GameObject earthAttackPrefab;

    public bool isCooldown;

    float cooldownTime;

    bool hasAttacked;

    public bool isDead;

    Vector3 originalSpawnPosition;

    [SerializeField] GameObject videoPlayer;

    void Awake()
    {
        hitsOnArm = 4;

        originalSpawnPosition = fireballSpawnPoint.position;

        healthBarMaskWidth = healthBarMaskTransform.sizeDelta.x;

        currentHP = maxHP;

        isCooldown = true;

        cooldownTime = 8;

        StartCoroutine(DamageCycle());

        StartCoroutine(AttackCooldown());
    }

    public void ChangingHPUI()
    {
        Vector2 barMaskSizeDelta = healthBarMaskTransform.sizeDelta;

        barMaskSizeDelta.x = (currentHP / maxHP) * healthBarMaskWidth;

        healthBarMaskTransform.sizeDelta = barMaskSizeDelta;
    }

    void Update()
    {
        Attacks();

        Fades();

        if (damageBarMaskTransform.sizeDelta.x != healthBarMaskTransform.sizeDelta.x)
        {
            damageBarMaskTransform.sizeDelta = new Vector2(Mathf.Lerp(damageBarMaskTransform.sizeDelta.x, (currentHP / maxHP) * healthBarMaskWidth, .01f), damageBarMaskTransform.sizeDelta.y);
        }

        if (canDamage)
        {
            headGlowyParticle.SetActive(true);
        }
    }

    void Attacks()
    {
        if (!isCooldown && !hasAttacked && !canDamage && !isDead)
        {
            Vector3 newPlayerPosition = new Vector3(0, 0, playerPosition.position.z);

            Vector3 newBossPosition = new Vector3(0, 0, this.transform.position.z);

            fireballSpawnPoint.position = originalSpawnPosition;

            if (Vector3.Distance(newBossPosition, newPlayerPosition) > 4)
            {
                hasAttacked = true;

                //bossAnimator.SetTrigger("BreathAttack");

                Instantiate(firebreathPrefab, fireballSpawnPoint);
            }
            else
            {
                hasAttacked = true;

                fireballSpawnPoint.position = fireballSpawnPoint.position - new Vector3(0, 2.30f, -.31f);

                Instantiate(earthAttackPrefab, fireballSpawnPoint);
            }
        }
    }

    public void CooldownBreath()
    {
        cooldownTime = 2;

        isCooldown = true;
    }

    public void CooldownEarth()
    {
        cooldownTime = 2;

        isCooldown = true;
    }

    void Fades()
    {
        if (isFading)
        {
            m_Timer += Time.deltaTime;

            bossHealthbar.alpha = m_Timer / fadeDuraction;

            if (bossHealthbar.alpha == 1)
            {
                isFading = false;

                m_Timer = fadeDuraction;
            }
        }

        if (isFadingOut)
        {
            m_Timer -= Time.deltaTime;

            bossHealthbar.alpha = m_Timer / fadeDuraction;

            if (bossHealthbar.alpha == 0)
            {
                isFadingOut = false;
            }
        }
    }

    IEnumerator DamageCycle()
    {
        while (true)
        {
            if (canDamage)
            {
                headGlowyParticle.SetActive(true);

                bossModel.transform.position = new Vector3(bossModel.transform.position.x, bossModel.transform.position.y - 2.6f, bossModel.transform.position.z);

                yield return new WaitForSeconds(4);

                bossModel.transform.position = new Vector3(bossModel.transform.position.x, this.transform.position.y, bossModel.transform.position.z);

                canDamage = false;

                headGlowyParticle.SetActive(false);

                for (int i = 0; i < armsGlowyParticles.Length; i++)
                {
                    armsGlowyParticles[i].SetActive(true);
                }

                enemyHead.selectedPointer.SetActive(false);

                playerCombat.bossTarget = null;

                hitsOnArm = 4;
            }

            yield return null;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            if (currentHP - damage > 0)
            {
                //bossAnimator.SetTrigger("Hit");

                currentHP -= damage;

                ChangingHPUI();
            }
            else
            {
                currentHP = 0;

                isDead = true;

                canDamage = false;

                enemyHead.selectedPointer.SetActive(false);

                playerCombat.bossTarget = null;

                bossModel.transform.position = new Vector3(bossModel.transform.position.x, this.transform.position.y, bossModel.transform.position.z);

                //bossAnimator.SetTrigger("Die");

                ChangingHPUI();

                StartCoroutine(DeathCoroutine());
            }
        }
    }


    IEnumerator DeathCoroutine()
    {
        isFadingOut = true;

        bossModel.gameObject.SetActive(false);

        AudioManager.Instance.PlayMusic(null, true);

        FadeManager.Instance.StartFadeOut();

        yield return new WaitForSeconds(2.5f);

        AudioManager.Instance.PlayMusic(AudioManager.Instance.cutsceneMusic, false);

        videoPlayer.SetActive(true);

        yield return new WaitForSeconds(8);

        GameManager.Instance.MainMenuSequenceFunction();

        yield break;
    }

    IEnumerator AttackCooldown()
    {
        while (true)
        {
            if (isCooldown)
            {
                yield return new WaitForSeconds(cooldownTime);

                isCooldown = false;

                hasAttacked = false;
            }

            yield return null;
        }
    }
}
