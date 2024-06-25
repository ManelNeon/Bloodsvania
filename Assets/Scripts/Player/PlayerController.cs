using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController playerCC;

    [Header("Main Camera Transform")]
    [SerializeField] Transform mainCam;

    [Header("Movement Variables")]
    public float speed = 6f;

    [SerializeField] float turnSmoothTime = .1f;

    [Header("Player Animator")]
    [SerializeField] Animator playerAnimator;

    [Header("NPC's Layer Mask")]
    [SerializeField] LayerMask npcLayerMask;

    [HideInInspector] public Vector3 direction;

    [Header("Roll Variables")]
    [SerializeField] float dashSpeed;

    [SerializeField] float dashTime;

    [HideInInspector] public bool isDashing;

    [Header("Rotation and Movement Hidden Variables")]
    float turnSmoothVelocity;
        
    float targetAngle;

    float angle;

    Vector3 moveDir;

    [Header("Raycast")]
    RaycastHit hit;

    [Header("Jump Variables")]
    [SerializeField] float gravityMultiplier = 3.0f;

    [SerializeField] float jumpHeight = 3f;

    [SerializeField] Transform groundCheck;

    [SerializeField] float groundDistance = 0.4f;

    [SerializeField] LayerMask groundMask;

    bool isGrounded;

    Vector3 velocity;

    float gravity = -9.81f;


    void Start()
    {
        //temporary, for build tests
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        playerCC = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        NPCRaycast();

        //setting the blend tree to the direction maginute, to see if the player is moving and animating them
        playerAnimator.SetFloat("Blend", direction.magnitude);

        //if the direction magnitude is different than 0 we play the footsteps sound
        if (direction.magnitude != 0)
        {
            //SFXManager.Instance.PlayFootstep();
        }

        if (Input.GetKeyDown(KeyManager.Instance.dashKey) && !isDashing && direction.magnitude != 0 && isGrounded && GameManager.Instance.canUseAbilities)
        {
            isDashing = true;

            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            playerAnimator.Play("Roll");

            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        direction = Vector3.zero;

        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            playerCC.Move(moveDir * dashSpeed * Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(.5f);

        isDashing = false;
    }

    void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime * gravityMultiplier;

        if (Input.GetKeyDown(KeyManager.Instance.jumpKey) && isGrounded && GameManager.Instance.isControlable && !isDashing)
        {
            playerAnimator.Play("Salto");

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (playerCC.enabled)
        {
            playerCC.Move(velocity * Time.deltaTime);
        }
    }

    //putting the movement on the late update
    private void LateUpdate()
    {
        ApplyGravity();

        Movement();
    }

    //The raycast for the npc function
    void NPCRaycast()
    {
        //the player can only talk if he is not moving
        if (Input.GetKeyDown(KeyManager.Instance.interactKey) && direction.magnitude == 0 && !GetComponent<CombatController>().isFighting)
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), Camera.main.transform.forward, out hit, 2, npcLayerMask))
            {
                NPC character = hit.collider.GetComponent<NPC>();

                if (character != null)
                {
                    character.StartDialogue();
                }
            }
        }
    }

    //function for the movement
    void Movement()
    {
        //the game manager has to let the player control
        if (GameManager.Instance.isControlable && !isDashing && playerCC.enabled)
        {
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (direction.magnitude >= 0.1f)
            {
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                playerCC.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }  
    }
}
