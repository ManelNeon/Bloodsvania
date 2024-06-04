using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float currentHealth;

    [SerializeField] GameObject selectedArrowParticle;

    [SerializeField] GameObject counterParticle;

    EnemyManager enemyManager;

    CharacterController enemyCharacterController;

    CombatController playerCombat;

    PlayerStats playerStats;

    float moveSpeed = 1;

    Animator enemyAnimator;

    [Header("States")]
    public bool isMoving;

    public bool isPreparingAttack;

    public bool isRetreating;

    public bool isLockedTarget;

    public bool isStunned;

    public bool isWaiting;

    public bool isDead;

    Vector3 moveDirection;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();

        enemyCharacterController = GetComponent<CharacterController>();

        enemyManager = GetComponentInParent<EnemyManager>();

        playerCombat = FindAnyObjectByType<CombatController>();

        playerStats = FindAnyObjectByType<PlayerStats>();

        EnemyDirection();
    }

    void EnemyDirection()
    {
        if (!isPreparingAttack && !isRetreating)
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
        }
        if (isPreparingAttack)
        {
            moveDirection = Vector3.forward;
        }
        if (isRetreating)
        {
            moveDirection = -Vector3.forward;
        }

        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (counterParticle.activeInHierarchy)
            {
                selectedArrowParticle.SetActive(false);
            }

            //Constantly look at player
            transform.LookAt(new Vector3(playerCombat.transform.position.x, transform.position.y, playerCombat.transform.position.z));

            //Only moves if the direction is set
            MoveEnemy(moveDirection);
        }   
    }

    void MoveEnemy(Vector3 direction)
    {
        //Set movespeed based on direction
        moveSpeed = 1;

        if (direction == Vector3.forward)
        {
            moveSpeed = 5;
        }

        if (direction == -Vector3.forward)
        {
            moveSpeed = 2;
        }

        //Set Animator values
        enemyAnimator.SetFloat("InputMagnitude", (enemyCharacterController.velocity.normalized.magnitude * direction.z) / (5 / moveSpeed), .2f, Time.deltaTime);

        enemyAnimator.SetBool("Strafe", (direction == Vector3.right || direction == Vector3.left));

        enemyAnimator.SetFloat("StrafeDirection", direction.normalized.x, .2f, Time.deltaTime);

        //Don't do anything if isMoving is false
        if (!isMoving)
        {
            return;
        }

        Vector3 dir = (playerCombat.transform.position - transform.position).normalized;

        Vector3 pDir = Quaternion.AngleAxis(90, Vector3.up) * dir; //Vector perpendicular to direction

        Vector3 movedir = Vector3.zero;

        Vector3 finalDirection = Vector3.zero;

        if (direction == Vector3.forward)
        {
            finalDirection = dir;
        }

        if (direction == Vector3.right || direction == Vector3.left)
        {
            finalDirection = (pDir * direction.normalized.x);
        }

        if (direction == -Vector3.forward)
        {
            finalDirection = -transform.forward;
        }


        if (direction == Vector3.right || direction == Vector3.left)
        {
            moveSpeed /= 1.5f;
        }

        movedir += finalDirection * moveSpeed * Time.deltaTime;

        enemyCharacterController.Move(movedir);

        if (!isPreparingAttack)
        {
            return;
        }

        if (Vector3.Distance(transform.position, playerCombat.transform.position) < 2)
        {
            Counter(true);

            isMoving = false;

            if (!playerCombat.isCountering && playerCombat.canAttack)
            {
                Attack();
            }
            else
            {
                isMoving = true;
            }
        }
    }

    void Counter(bool active)
    {
        if (active)
        {
            playerCombat.enemyAttacking = this;
            counterParticle.SetActive(true);
        }
        else
        {
            playerCombat.enemyAttacking = null;
            counterParticle.SetActive(false);
        }
    }

    private void Attack()
    {
        transform.DOMove(transform.position + (transform.forward / 1), .5f);

        enemyAnimator.SetTrigger("AirPunch");
    }

    public void TakeDamage(float damage)
    {
        Counter(false);

        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;

            StartCoroutine(GotPunch());

            return;
        }
        else
        {
            currentHealth -= damage;

            enemyManager.DeleteEnemy(this);

            int random = Random.Range(634, 1024);

            playerStats.AddFulgurite(random);

            isDead = true;

            if (!enemyManager.RandomEnemy())
            {
                playerCombat.currentTarget = null;
            }

            enemyAnimator.Play("Death");

            StartCoroutine(DeathTimer());
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);

        yield break;
    }

    public void NoLongerSelected()
    {
        selectedArrowParticle.SetActive(false);
    }

    IEnumerator GotPunch()
    {
        isPreparingAttack = false;

        enemyManager.StartingAI();

        isMoving = false;

        enemyAnimator.SetTrigger("Hit");

        yield return new WaitForSeconds(1.2f);

        EnemyDirection();

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

    public void AttackEvent()
    {
        Debug.Log("Attacked");

        Counter(false);

        float random = Random.Range(20,40);

        playerStats.TakeDamage(random);

        isPreparingAttack = false;
    }

    public void AttackEndEvent()
    {
        Retreating();
    }

    public void PreparingAttack()
    {
        isPreparingAttack = true;

        EnemyDirection();
    }

    public void Retreating()
    {
        isRetreating = true;

        StartCoroutine(StopRetreating());

        EnemyDirection();
    }

    IEnumerator StopRetreating()
    {
        yield return new WaitForSeconds(1.5f);

        isRetreating = false;

        EnemyDirection();

        enemyManager.StartingAI();

        yield break;
    }

    public void GettingCountered()
    {
        enemyAnimator.Play("");
    }
}
