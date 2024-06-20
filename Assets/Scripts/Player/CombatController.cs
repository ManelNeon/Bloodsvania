using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [Header("References to other Scripts")]
    PlayerController playerController;

    Animator playerAnimator;

    public EnemyController currentTarget;

    public EnemyBoss bossTarget; 

    public EnemyController enemyAttacking;

    [Header("Enemy's Layer Mask")]
    [SerializeField] LayerMask enemyLayerMask;

    [Header("States")]
    [HideInInspector] public bool canAttack;

    [HideInInspector] public bool isCountering;

    bool isAttacking;

    [HideInInspector] public bool isOnRage;

    [HideInInspector] public bool isFighting;

    [Header("References")]
    [SerializeField] ParticleSystem healParticles;

    [SerializeField] GameObject rageVignette;

    [Header("Cameras")]
    [SerializeField] GameObject combatCamera;

    [SerializeField] GameObject normalCamera;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        playerAnimator = GetComponentInChildren<Animator>();

        canAttack = true;

        StartCoroutine(RageMode());

        StartCoroutine(RageGain());
    }

    IEnumerator RageGain()
    {
        while (true)
        {
            if (!isFighting && !isOnRage)
            {
                yield return new WaitForSeconds(.1F);

                if (GameManager.Instance.currentRage != GameManager.Instance.bloodValue)
                {
                    GameManager.Instance.currentRage += .5f;

                    GameManager.Instance.ChangingRageUI();
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    public void EnableFighting()
    {
        isFighting = true;

        combatCamera.GetComponent<CinemachineFreeLook>().m_YAxis = normalCamera.GetComponent<CinemachineFreeLook>().m_YAxis;

        combatCamera.GetComponent<CinemachineFreeLook>().m_XAxis = normalCamera.GetComponent<CinemachineFreeLook>().m_XAxis;

        combatCamera.SetActive(true);

        normalCamera.SetActive(false);
    }

    public void DisableFighting()
    {
        isFighting = false;

        normalCamera.GetComponent<CinemachineFreeLook>().m_YAxis = combatCamera.GetComponent<CinemachineFreeLook>().m_YAxis;

        normalCamera.GetComponent<CinemachineFreeLook>().m_XAxis = combatCamera.GetComponent<CinemachineFreeLook>().m_XAxis;

        normalCamera.SetActive(true);

        combatCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isControlable)
        {
            //if the player left clicks and if he has a current target
            if (Input.GetMouseButtonDown(0) && canAttack && isFighting)
            {
                if (currentTarget != null)
                {
                    //we rotate the player towards the enemy he's currently attacking and then we move towards him
                    transform.DOLookAt(currentTarget.transform.position, .2f);
                    transform.DOMove(TargetOffset(currentTarget.transform), .8f);
                }
                else if (bossTarget != null)
                {
                    //we rotate the player towards the enemy he's currently attacking and then we move towards him
                    transform.DOLookAt(bossTarget.transform.position, .2f);
                    transform.DOMove(TargetOffset(bossTarget.transform), .8f);
                }
                else
                {
                    return;
                }

                isAttacking = true;

                playerController.enabled = false;

                //randomizing between the two animations
                int number = Random.Range(1, 3);

                GetComponent<CharacterController>().enabled = false;

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
            if (Input.GetMouseButton(1) && enemyAttacking && isFighting)
            {
                transform.DOLookAt(new Vector3(enemyAttacking.transform.position.x, transform.position.y, enemyAttacking.transform.position.z), .2f);
                transform.DOMove(TargetOffset(enemyAttacking.transform), .8f);

                playerController.enabled = false;

                playerAnimator.Play("Counter");

                //he is countering now
                isCountering = true;

                Attack();

                canAttack = false;
            }

            if (Input.GetKeyDown(KeyCode.Q) && isFighting) 
            {
                if (!isOnRage)
                {
                    if (GameManager.Instance.currentRage >= 50)
                    {
                        rageVignette.SetActive(true);

                        GameManager.Instance.dmgValue *= 2;

                        playerController.speed *= 2;

                        playerAnimator.speed *= 2;

                        isOnRage = true;
                    }
                }
                else
                {
                    rageVignette.SetActive(false);

                    GameManager.Instance.dmgValue /= 2;

                    playerController.speed /= 2;

                    playerAnimator.speed /= 2;

                    isOnRage = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.E) && GameManager.Instance.currentRage >= 1)
            {
                //Heal
                if (GameManager.Instance.currentHP != GameManager.Instance.hpValue)
                {
                    GameManager.Instance.HealFunction();

                    healParticles.Play();
                }
            }
        }

        EnemyRaycast();
    }

    IEnumerator RageMode()
    {
        while (true)
        {
            if (isOnRage)
            {
                if (GameManager.Instance.currentHP > 10 && GameManager.Instance.currentRage > 0)
                {
                    yield return new WaitForSeconds(.1f);

                    GameManager.Instance.currentHP -= 2 / (GameManager.Instance.compositionValue * 0.03f);

                    GameManager.Instance.currentRage -= .5f;

                    GameManager.Instance.ChangingHPUI();

                    GameManager.Instance.ChangingRageUI();
                }
                else
                {
                    rageVignette.SetActive(false);

                    GameManager.Instance.dmgValue /= 2;

                    playerController.speed /= 2;

                    playerAnimator.speed /= 2;

                    isOnRage = false;
                }
                
            }
            else
            {
                yield return null;
            }
        }
    }

    //event that plays when the attack ends, if he's countering, hes nolonger countering, he can attack and he can move again
    public void AttackEnd()
    {
        if (isCountering)
        {
            isCountering = false;
        }

        GetComponent<CharacterController>().enabled = true;

        canAttack = true;

        isAttacking = false;

        playerController.enabled = true;
    }

    //event that plays when player makes contact, except in counter because we want the enemy to stop moving immeaditly on button press
    public void Attack()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.punchSound);

        GameManager.Instance.GainRage(Random.Range(10, 20));

        if (isCountering)
        {
            enemyAttacking.TakeDamage(GameManager.Instance.dmgValue * .05f);
            return;
        }

        currentTarget.TakeDamage(GameManager.Instance.dmgValue * .05f);
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
            if (hit.collider.GetComponent<EnemyController>() != null && hit.collider.GetComponent<EnemyController>().IsAttackable())
            {
                currentTarget = hit.collider.transform.GetComponent<EnemyController>();
            }
            else if (hit.collider.GetComponent<EnemyBoss>().IsAttackable())
            {
                bossTarget = hit.collider.GetComponent<EnemyBoss>();
            }
            else if (!isAttacking)
            {
                if (currentTarget != null)
                {
                    currentTarget.NoLongerSelected();
                }

                currentTarget = null;
                bossTarget = null;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        //we play the punch sfx
        AudioManager.Instance.PlaySFX(AudioManager.Instance.punchedSound);

        float damageCalculated = damage / (GameManager.Instance.compositionValue * 0.03f);

        GameManager.Instance.GainRage(Random.Range(10,20));

        if (GameManager.Instance.currentHP - damageCalculated > 0)
        {
            playerAnimator.Play("Punched");

            StartCoroutine(GotHit());

            GameManager.Instance.currentHP -= damageCalculated;

            GameManager.Instance.ChangingHPUI();

        }
        else
        {
            GameManager.Instance.currentHP = 0;

            GameManager.Instance.ChangingHPUI();

            playerAnimator.Play("Death");

            playerController.enabled = false;

            StartCoroutine(Death());
        }
    }

    //the GotHit sequence
    IEnumerator GotHit()
    {
        canAttack = false;

        playerController.enabled = false;

        yield return new WaitForSeconds(1);

        canAttack = true;

        playerController.enabled = true;
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(2);

        GameManager.Instance.MainMenuSequenceFunction();

        yield break;
    }
}
