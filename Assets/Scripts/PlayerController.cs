using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController playerCC;
    [SerializeField] Transform mainCam;
    [SerializeField] float speed = 6f;
    [SerializeField] float turnSmoothTime = .1f;
    float turnSmoothVelocity;
    Vector3 direction;
    float targetAngle;
    float angle;
    Vector3 moveDir;
    int layerMask;
    void Start()
    {
        //temporary, for build tests
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCC = GetComponent<CharacterController>();
        layerMask = 1 << 6;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && direction.magnitude == 0)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + new Vector3(0,1,0), transform.TransformDirection(Vector3.forward * 2), out hit, Mathf.Infinity, layerMask))
            {
                NPC character = hit.collider.GetComponent<NPC>();

                if (character != null)
                {
                    character.StartDialogue();
                }
            }
        }

        Movement();
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
