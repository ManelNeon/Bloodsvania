using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float currentHealth;

    float moveSpeed = 1;

    Vector3 moveDirection;

    [Header("Particles Objects")]
    [SerializeField] GameObject selectedArrowParticle;

    [SerializeField] GameObject counterParticle;

    [Header("References to other scritps")]
    EnemyManager enemyManager;

    CharacterController enemyCharacterController;

    CombatController playerCombat;

    PlayerStats playerStats;

    Animator enemyAnimator;

    [Header("States")]
    public bool isMoving;

    public bool isPreparingAttack;

    public bool isRetreating;

    public bool isLockedTarget;

    public bool isStunned;

    public bool isWaiting;

    public bool isDead;

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();

        enemyCharacterController = GetComponent<CharacterController>();

        enemyManager = GetComponentInParent<EnemyManager>();

        //for both the player combat and the player stats there will only be one instance of them so we can use the FindAnyObjectByType
        playerCombat = FindAnyObjectByType<CombatController>();

        playerStats = FindAnyObjectByType<PlayerStats>();

        //we do the enemy direction function to set his direction
        EnemyDirection();
    }

    //in here we check if the enemy isnt preparing an attack or retreating and then we randomize if he goes left or right
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
        
        //we set the ismoving to true, because the enemy is moving
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            //in case the counter particle is activated, we deactivate the arrow particle so that it doesnt collid with each other
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

        Vector3 dir = (playerCombat.transform.position - transform.position).normalized; //the direction is always to the player's

        Vector3 pDir = Quaternion.AngleAxis(90, Vector3.up) * dir; //Vector perpendicular to direction

        Vector3 movedir = Vector3.zero;

        Vector3 finalDirection = Vector3.zero;

        if (direction == Vector3.forward) //if the direction is forward (towards the player) then it's final direction is towards him
        {
            finalDirection = dir;
        }

        if (direction == Vector3.right || direction == Vector3.left) //if the direction is left or right, he's strafing, so we use the perpendicular direction
        {
            finalDirection = (pDir * direction.normalized.x);
        }

        if (direction == -Vector3.forward) //if its negative the forward direction, hes retreating
        {
            finalDirection = -transform.forward;
        }


        if (direction == Vector3.right || direction == Vector3.left) //in case the direction is left or right we make the enemy slower
        {
            moveSpeed /= 1.5f;
        }

        movedir += finalDirection * moveSpeed * Time.deltaTime;

        enemyCharacterController.Move(movedir);

        if (!isPreparingAttack) //if he's not preparing an attack, the next code isnt played
        {
            return;
        }

        if (Vector3.Distance(transform.position, playerCombat.transform.position) < 2) //we check if the enemy is close to the player, if he is we then
        {
            //activate the counter
            Counter(true);

            //deactivate the ismoving
            isMoving = false;

            //if the player isnt countering, we attack
            if (!playerCombat.isCountering)
            {
                Attack();
            }
        }
        else
        {
            isMoving = true;
        }
    }

    //we have the Counter Function where in case it's active we set the enemy attacking to this and we activate the counter particle, if not we deactivate it
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

    //the attack function, where we move the enemy towards the player and we set the airpunch trigger where we start the animation
    private void Attack()
    {
        transform.DOMove(transform.position + (transform.forward / 1), .5f);

        enemyAnimator.SetTrigger("AirPunch");
    }

    //in this function the player takes damage
    public void TakeDamage(float damage)
    {
        //we set the counter to false
        Counter(false);

        //if the player wont die we start the gotpunch courotine
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

            //getting a random number between these values
            int random = Random.Range(634, 1024);

            //adding fulgurite to these stats
            playerStats.AddFulgurite(random);

            NoLongerSelected();

            //the enmey is now dead
            isDead = true;

            if (playerCombat.currentTarget == this)
            {
                playerCombat.currentTarget = null;
            }

            //we play the death animation and then we set a timer to destroy the object
            enemyAnimator.Play("Death");

            enemyManager.EventAllEnemiesDead();

            StartCoroutine(DeathTimer());
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);

        yield break;
    }

    //if no longet selected, we then set the arrow particle to false (function so that the player can access it)
    public void NoLongerSelected()
    {
        selectedArrowParticle.SetActive(false);
    }

    //when the enemy gets punch we start the AI again to set a new attacker, and set a new direction
    IEnumerator GotPunch()
    {
        isPreparingAttack = false;

        isMoving = false;

        enemyAnimator.SetTrigger("Hit");

        yield return new WaitForSeconds(1.2f);

        Retreating();

        yield break;
    }

    //if the enemy is attackable we return true and set the arrow particle to true
    public bool IsAttackable()
    {
        if (currentHealth > 0)
        {
            selectedArrowParticle.SetActive(true);

            return true;
        }

        return false;
    }

    //the event that plays when the enemy makes contact with the player when attacking
    public void AttackEvent()
    {
        Counter(false);

        float random = Random.Range(20,40);

        if (Vector3.Distance(transform.position, playerCombat.transform.position) < 2)
        {
            playerStats.TakeDamage(random);
        }

        isPreparingAttack = false;
    }

    //event that plays when the enemy finishes the attacking animation
    public void AttackEndEvent()
    {
        Retreating();
    }

    //function that plays when preparing the attack
    public void PreparingAttack()
    {
        isPreparingAttack = true;

        EnemyDirection();
    }

    //function that plays when the enemy is retreating
    public void Retreating()
    {
        isRetreating = true;

        StartCoroutine(StopRetreating());

        EnemyDirection();
    }

    //coroutine to stop the AI from retreating and then starting the AI again
    IEnumerator StopRetreating()
    {
        yield return new WaitForSeconds(1.5f);

        isRetreating = false;

        EnemyDirection();

        enemyManager.StartingAI();

        yield break;
    }
}
