using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyController[] enemies;

    public List<EnemyController> enemiesList = new List<EnemyController>(); 

    public EnemyController tempEnemy;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesList.Add(enemies[i]);
        }

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

    void AttackingAI()
    {
        if (!CheckingIfAttacking() && RandomEnemy() != null)
        {
            EnemyController attackingEnemy = RandomEnemy();

            if (enemiesList.Count > 1)
            {
                while (attackingEnemy == tempEnemy)
                {
                    attackingEnemy = RandomEnemy();
                }
            }

            tempEnemy = attackingEnemy;

            tempEnemy.PreparingAttack();
        }
    }

    bool CheckingIfAttacking()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i].isPreparingAttack)
            {
                return true;
            }
        }

        return false;
    }

    public void DeleteEnemy(EnemyController enemy)
    {
        enemiesList.Remove(enemy);

        AttackingAI();
    }

    EnemyController RandomEnemy()
    {
        if (enemiesList.Count == 0)
        {
            return null;
        }

        EnemyController randomEnemy = null;

        while (randomEnemy == null)
        {
            int randomNumber = Random.Range(0, enemiesList.Count);

            Debug.Log(randomNumber);

            randomEnemy = enemiesList[randomNumber];
        }

        return randomEnemy;
    }


}
