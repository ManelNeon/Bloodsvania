using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [Header("References to other Scripts")]
    PlayerStats playerStats;

    PlayerController playerController;

    Animator playerAnimator;

    public EnemyController currentTarget;

    public EnemyController enemyAttacking;

    [Header("Enemy's Layer Mask")]
    [SerializeField] LayerMask enemyLayerMask;

    [Header("States")]
    [HideInInspector] public bool canAttack;

    [HideInInspector] public bool isCountering;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        playerController = GetComponent<PlayerController>();

        playerAnimator = GetComponentInChildren<Animator>();

        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isControlable)
        {
            //if the player left clicks and if he has a current target
            if (Input.GetMouseButtonDown(0) && canAttack && currentTarget != null)
            {
                //we rotate the player towards the enemy he's currently attacking and then we move towards him
                transform.DOLookAt(currentTarget.transform.position, .2f);
                transform.DOMove(TargetOffset(currentTarget.transform), .8f);

                playerController.enabled = false;

                //randomizing between the two animations
                int number = Random.Range(1, 3);
                if (number == 1)
                {
                    playerAnimator.Play("Attack");
                }
                else
                {
                    playerAnimator.Play("SecondAttack");
                }

                //the player cant attack now
                canAttack = false;
            }

            //if the player right clicks, he can attack and there's an enemy attacking
            if (Input.GetMouseButton(1) && enemyAttacking)
            {
                transform.DOLookAt(enemyAttacking.transform.position, .2f);
                transform.DOMove(TargetOffset(enemyAttacking.transform), .8f);

                playerController.enabled = false;

                playerAnimator.Play("Counter");

                //he is countering now
                isCountering = true;

                Attack();

                canAttack = false;
            }
        }

        EnemyRaycast();
    }

    //event that plays when the attack ends, if he's countering, hes nolonger countering, he can attack and he can move again
    public void AttackEnd()
    {
        if (isCountering)
        {
            isCountering = false;
        }

        canAttack = true;

        playerController.enabled = true;
    }

    //event that plays when player makes contact, except in counter because we want the enemy to stop moving immeaditly on button press
    public void Attack()
    {
        SFXManager.Instance.PlayPunch();

        if (isCountering)
        {
            enemyAttacking.TakeDamage(playerStats.damageValue * .75f);
            return;
        }

        currentTarget.TakeDamage(playerStats.damageValue * .75f);
    }

    //getting the offset between the player and the targets position
    Vector3 TargetOffset(Transform target)
    {
        Vector3 position;

        position = target.position;

        return Vector3.MoveTowards(position, transform.position, .95f);
    }

    //the enemy's raycast, in case he can attack he will store the enemy he's looking at in the current target variable
    void EnemyRaycast()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 3f, Camera.main.transform.forward, out hit, 10, enemyLayerMask) && canAttack)
        {
            if (hit.collider.GetComponent<EnemyController>().IsAttackable())
            {
                if (currentTarget != hit.collider.GetComponent<EnemyController>() && currentTarget != null)
                {
                    currentTarget.NoLongerSelected();
                }

                currentTarget = hit.collider.transform.GetComponent<EnemyController>();
            }
        }
    }
}
