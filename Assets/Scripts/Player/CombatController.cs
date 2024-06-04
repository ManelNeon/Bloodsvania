using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    PlayerStats playerStats;

    PlayerController playerController;

    public EnemyController currentTarget;

    public EnemyController enemyAttacking;

    [SerializeField] LayerMask enemyLayerMask;

    Animator playerAnimator;

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
            if (Input.GetMouseButtonDown(0) && canAttack && currentTarget != null)
            {
                transform.DOLookAt(currentTarget.transform.position, .2f);
                transform.DOMove(TargetOffset(currentTarget.transform), .8f);

                playerController.enabled = false;

                int number = Random.Range(1, 3);
                if (number == 1)
                {
                    playerAnimator.Play("Attack");
                }
                else
                {
                    playerAnimator.Play("SecondAttack");
                }

                canAttack = false;
            }

            if (Input.GetMouseButton(1) && canAttack && enemyAttacking)
            {
                transform.DOLookAt(enemyAttacking.transform.position, .2f);
                transform.DOMove(TargetOffset(enemyAttacking.transform), .8f);

                playerController.enabled = false;

                playerAnimator.Play("Counter");

                isCountering = true;

                Attack();

                canAttack = false;
            }
        }

        EnemyRaycast();
    }

    public void AttackEnd()
    {
        if (isCountering)
        {
            isCountering = false;
        }

        canAttack = true;
        playerController.enabled = true;
    }

    public void Attack()
    {
        if (isCountering)
        {
            enemyAttacking.TakeDamage(playerStats.damageValue * .75f);
            return;
        }

        currentTarget.TakeDamage(playerStats.damageValue * .75f);
    }

    Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, .95f);
    }

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
