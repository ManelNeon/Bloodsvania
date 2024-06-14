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

    [Header("Rotation and Movement Hidden Variables")]
    float turnSmoothVelocity;
        
    float targetAngle;

    float angle;

    Vector3 moveDir;

    [Header("Raycast")]
    RaycastHit hit;

    void Start()
    {
        //temporary, for build tests
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        playerCC = GetComponent<CharacterController>();
    }

    void Update()
    {
        NPCRaycast();

        //setting the blend tree to the direction maginute, to see if the player is moving and animating them
        playerAnimator.SetFloat("Blend", direction.magnitude);

        //if the direction magnitude is different than 0 we play the footsteps sound
        if (direction.magnitude != 0)
        {
            //SFXManager.Instance.PlayFootstep();
        }
    }

    //putting the movement on the late update
    private void LateUpdate()
    {
        Movement();
    }

    //The raycast for the npc function
    void NPCRaycast()
    {
        //the player can only talk if he is not moving
        if (Input.GetKeyDown(KeyCode.F) && direction.magnitude == 0)
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
        if (GameManager.Instance.isControlable)
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
