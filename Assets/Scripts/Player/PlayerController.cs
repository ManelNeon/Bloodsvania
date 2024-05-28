using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController playerCC;

    [SerializeField] Transform mainCam;

    [SerializeField] float speed = 6f;

    [SerializeField] float turnSmoothTime = .1f;

    [SerializeField] Animator playerAnimator;

    float turnSmoothVelocity;

    Vector3 direction;

    float targetAngle;

    float angle;

    Vector3 moveDir;

    [SerializeField] LayerMask npcLayerMask;

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

        playerAnimator.SetFloat("Blend", direction.magnitude);
    }

    private void LateUpdate()
    {
        Movement();
    }

    void NPCRaycast()
    {
        if (Input.GetKeyDown(KeyCode.F) && direction.magnitude == 0)
        {
            Debug.Log("F Pressed");

            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.forward.normalized, out hit, 2, npcLayerMask))
            {
                Debug.Log("Hit Something");
                NPC character = hit.collider.GetComponent<NPC>();

                if (character != null)
                {
                    Debug.Log("Hit NPC");
                    character.StartDialogue();
                }
            }
        }
    }

    void Movement()
    {
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
