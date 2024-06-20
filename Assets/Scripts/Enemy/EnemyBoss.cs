using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    BossEnemyManager bossEnemyManager;

    [SerializeField] bool isArm;

    private void Awake()
    {
        bossEnemyManager = GetComponentInParent<BossEnemyManager>();
    }

    public bool IsAttackable()
    {
        if (isArm)
        {
            if (!bossEnemyManager.canDamage)
            {
                Debug.Log("Identified Arm");
                return true;
            }
        }
        else
        {
            if (bossEnemyManager.canDamage)
            {
                Debug.Log("Identified Head");
                return true;
            }
        }

        return false;
    }

    void TakeDamage()
    {
        if (isArm)
        {
            bossEnemyManager.hitsOnArm -= 1;
        }
        else
        {
            bossEnemyManager.currentHP -= 25;
        }
    }
}
