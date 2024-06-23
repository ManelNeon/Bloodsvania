using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    BossEnemyManager bossEnemyManager;

    [SerializeField] GameObject glowyParticle;

    public GameObject selectedPointer;

    [Header("States")]
    
    [SerializeField] bool isArm;

    bool isStunned;

    CombatController playerCombat;

    private void Awake()
    {
        if (isArm)
        {
            glowyParticle.SetActive(true);
        }

        bossEnemyManager = GetComponentInParent<BossEnemyManager>();

        playerCombat = FindObjectOfType<CombatController>();
    }

    private void Update()
    {
        if (bossEnemyManager.canDamage && isArm)
        {
            glowyParticle.SetActive(false);
        }
    }

    public bool IsAttackable()
    {
        if (isArm && !isStunned && !bossEnemyManager.canDamage && !bossEnemyManager.isDead)
        {
            selectedPointer.SetActive(true);
            return true;
        }
        else if (!isArm && bossEnemyManager.canDamage)
        {
            selectedPointer.SetActive(true);
            return true;
        }

        return false;
    }

    public void TakeDamage(float damage)
    {
        if (isArm)
        {
            if (bossEnemyManager.hitsOnArm - 1 == 0)
            {
                bossEnemyManager.hitsOnArm = 0;

                glowyParticle.SetActive(false);

                selectedPointer.SetActive(false);

                playerCombat.bossTarget = null;

                bossEnemyManager.canDamage = true;
            }
            else
            {
                bossEnemyManager.hitsOnArm -= 1;

                playerCombat.bossTarget = null;

                selectedPointer.SetActive(false);

                StartCoroutine(StunnedCoroutine());
            }
        }
        else
        {
            bossEnemyManager.TakeDamage(damage);
        }
    }

    IEnumerator StunnedCoroutine()
    {
        isStunned = true;

        glowyParticle.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        glowyParticle.SetActive(true);

        isStunned = false;

        yield break;
    }
}
