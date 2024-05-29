using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyController[] enemies;

    EnemyController tempEnemy;

    // Start is called before the first frame update
    void Start()
    {
        StartingAI();
    }

    public void StartingAI()
    {
        StartCoroutine(StartAI());
    }

    IEnumerator StartAI()
    {
        yield return new WaitForSeconds(2f);

        AttackingAI();
    }

    public void AttackingAI()
    {
        EnemyController attackingEnemy = RandomEnemy();

        while (attackingEnemy == tempEnemy)
        {
            attackingEnemy = RandomEnemy();
        }

        tempEnemy = attackingEnemy;

        tempEnemy.PreparingAttack();
    }

    EnemyController RandomEnemy()
    {
        if (AvailableEnemyCount() == 0)
        {
            return null;
        }

        EnemyController randomEnemy = null;

        while (randomEnemy == null)
        {
            int randomNumber = Random.Range(0, enemies.Length);

            Debug.Log(randomNumber);

            randomEnemy = enemies[randomNumber];
        }

        return randomEnemy;
    }

    int AvailableEnemyCount()
    {
        int count = 0;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                count++;
            }
        }

        return count;
    }

}
