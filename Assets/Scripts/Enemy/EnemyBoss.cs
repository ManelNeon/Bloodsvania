using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    BossEnemyManager bossEnemyManager;

    public GameObject selectedPointer;

    [Header("States")]
    
    [SerializeField] bool isArm;

    bool isStunned;

    CombatController playerCombat;

    private void Awake()
    {
        bossEnemyManager = GetComponentInParent<BossEnemyManager>();

        playerCombat = FindObjectOfType<CombatController>();
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

        yield return new WaitForSeconds(2.5f);

        isStunned = false;

        yield break;
    }
}
