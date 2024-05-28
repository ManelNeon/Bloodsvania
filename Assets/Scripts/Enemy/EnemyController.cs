using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float currentHealth;

    [SerializeField] GameObject selectedArrowParticle;

    CharacterController enemyCharacterController;

    CombatController playerCombat;

    float moveSpeed = 1;

    Animator enemyAnimator;

    bool isMoving;

    Vector3 moveDirection;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();

        enemyCharacterController = GetComponent<CharacterController>();

        playerCombat = FindAnyObjectByType<CombatController>();

        EnemyDirection();

        //StartCoroutine(EnemyMovement());
    }

    /*
    IEnumerator EnemyMovement()
    {
        int randomDir = Random.Range(0, 2);

        if (randomDir == 0)
        {
            moveDirection = Vector3.right;
        }
        else
        {
            moveDirection = Vector3.left;
        }

        isMoving = true;

        yield break;
    }
    */

    void EnemyDirection()
    {
        int randomDir = Random.Range(0, 2);

        if (randomDir == 0)
        {
            moveDirection = Vector3.right;
        }
        else
        {
            moveDirection = Vector3.left;
        }

        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Constantly look at player
        transform.LookAt(new Vector3(playerCombat.transform.position.x, transform.position.y, playerCombat.transform.position.z));

        //Only moves if the direction is set
        MoveEnemy(moveDirection);
    }

    void MoveEnemy(Vector3 direction)
    {
        //Set movespeed based on direction
        moveSpeed = 1;

        if (direction == Vector3.forward)
            moveSpeed = 5;
        if (direction == -Vector3.forward)
            moveSpeed = 2;

        //Set Animator values
        enemyAnimator.SetFloat("InputMagnitude", (enemyCharacterController.velocity.normalized.magnitude * direction.z) / (5 / moveSpeed), .2f, Time.deltaTime);
        enemyAnimator.SetBool("Strafe", (direction == Vector3.right || direction == Vector3.left));
        enemyAnimator.SetFloat("StrafeDirection", direction.normalized.x, .2f, Time.deltaTime);

        //Don't do anything if isMoving is false
        if (!isMoving)
            return;

        Vector3 dir = (playerCombat.transform.position - transform.position).normalized;
        Vector3 pDir = Quaternion.AngleAxis(90, Vector3.up) * dir; //Vector perpendicular to direction
        Vector3 movedir = Vector3.zero;

        Vector3 finalDirection = Vector3.zero;

        if (direction == Vector3.forward)
            finalDirection = dir;
        if (direction == Vector3.right || direction == Vector3.left)
            finalDirection = (pDir * direction.normalized.x);
        if (direction == -Vector3.forward)
            finalDirection = -transform.forward;

        if (direction == Vector3.right || direction == Vector3.left)
            moveSpeed /= 1.5f;

        movedir += finalDirection * moveSpeed * Time.deltaTime;

        enemyCharacterController.Move(movedir);
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            StartCoroutine(GotPunch());
            return;
        }
        else
        {
            currentHealth = 0;
            Destroy(gameObject);
        }
    }

    public void NoLongerSelected()
    {
        selectedArrowParticle.SetActive(false);
    }

    IEnumerator GotPunch()
    {
        isMoving = false;

        enemyAnimator.SetTrigger("Hit");

        yield return new WaitForSeconds(1.2f);

        MoveEnemy(moveDirection);

        yield break;
    }

    public bool IsAttackable()
    {
        if (currentHealth > 0)
        {
            selectedArrowParticle.SetActive(true);
            return true;
        }

        return false;
    }
}
