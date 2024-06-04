using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //the array with the enemies
    [SerializeField] EnemyController[] enemies;

    //a list where we will store the enemies
    public List<EnemyController> enemiesList = new List<EnemyController>(); 

    //a variable where we store the attacking enemy
    public EnemyController tempEnemy;

    // Start is called before the first frame update
    void Start()
    {
        //we add the enemies to the list
        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesList.Add(enemies[i]);
        }

        //start the AI
        StartingAI();
    }

    //in here we start the coroutine
    public void StartingAI()
    {
        StartCoroutine(StartAI());
    }

    IEnumerator StartAI()
    {
        //waiting two seconds to give the player breating room
        yield return new WaitForSeconds(2f);

        AttackingAI();
    }

    void AttackingAI()
    {
        //we check if there is any enemy attacking, if there isnt and there are enemies we then proceed
        if (!CheckingIfAttacking() && RandomEnemy() != null)
        {
            //we get a random enemy
            EnemyController attackingEnemy = RandomEnemy();

            //in case there's more than 1 enemy, we dont want the same enemy to attack so we check if the temp enemy is the same as the random enemy
            if (enemiesList.Count > 1)
            {
                while (attackingEnemy == tempEnemy)
                {
                    attackingEnemy = RandomEnemy();
                }
            }

            tempEnemy = attackingEnemy;

            //we prepare the attack on the attacking enemy
            tempEnemy.PreparingAttack();
        }
    }

    //in here we run throught the list and check if any are attacking
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

    //function to delete the enemy from the list
    public void DeleteEnemy(EnemyController enemy)
    {
        enemiesList.Remove(enemy);

        StartingAI();
    }

    //in here we get a random enemy, in case there arent any enemies we return null
    public EnemyController RandomEnemy()
    {
        if (enemiesList.Count == 0)
        {
            return null;
        }

        EnemyController randomEnemy = null;

        while (randomEnemy == null)
        {
            int randomNumber = Random.Range(0, enemiesList.Count);

            randomEnemy = enemiesList[randomNumber];
        }

        return randomEnemy;
    }


}
