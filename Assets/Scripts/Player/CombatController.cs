using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    public EnemyController currentTarget;

    [SerializeField] LayerMask enemyLayerMask;

    [SerializeField] Animator playerAnimator;

    [HideInInspector] public bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            if (currentTarget != null)
            {
                transform.DOLookAt(currentTarget.transform.position, .2f);
                transform.DOMove(TargetOffset(currentTarget.transform), .8f);
                playerController.enabled = false;
                int number = Random.Range(1,3);
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
        }

        EnemyRaycast();
    }

    public void AttackEnd()
    {
        canAttack = true;
        playerController.enabled = true;
    }

    public void Attack()
    {
        currentTarget.TakeDamage(1);
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
