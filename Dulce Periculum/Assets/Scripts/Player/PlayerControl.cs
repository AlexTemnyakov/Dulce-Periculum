using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Range(0, 10f)]
    public  float               Sensitivity;
    public  float               ForwardSpeed;
    public  float               BackwardSpeed;
    public  float               SideSpeed;
    public  float               Acceleration;
    [Range(0, 0.3f)]
    public  float               RunRotSpeed;

    private const
            float               SPEED_DEVIATION = 0.99f;
    private const
            float               BRAKING_LERP    = 0.01f;
    private const
            float               WALKING_LERP    = 0.5f;
    private const
            float               RUNNING_LERP    = 0.05f;

    private GameManager         gameManager;
    private CharacterController controller;
    private Animator            animator;
    private PlayerFight         fight;
    private float               curSpeed;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        controller  = GetComponent<CharacterController>();
        animator    = GetComponent<Animator>();
        fight       = GetComponent<PlayerFight>();
        curSpeed    = 0;

        animator.applyRootMotion = false;
    }

    void Update()
    {
        if (gameManager.CustomInput.GetKeyDown(KeyCode.Mouse2))
        {
            animator.SetBool("Attacking State", false);
        }

        if (gameManager.CustomInput.GetKeyDown(KeyCode.Mouse1) && animator.GetBool("Attacking State") && curSpeed < ForwardSpeed)
        {
            animator.SetTrigger("Spell");
        }
        
        Rotate();
        Move();
        
    }

    private void Rotate()
    {
        if (curSpeed < ForwardSpeed)
            transform.Rotate(new Vector3(0, gameManager.CustomInput.GetAxis("Mouse X"), 0) * Time.deltaTime * Sensitivity);
        else
            transform.Rotate(new Vector3(0, gameManager.CustomInput.GetAxis("Mouse X"), 0) * Time.deltaTime * Sensitivity / 2);
    }

    private void Move()
    {
        Vector3 movement    = Vector3.zero;
        Vector3 moveForward = Vector3.zero;
        Vector3 moveSide    = Vector3.zero;
        float   accel       = 1;
        float   lerpTime    = 1f;
        int     forwardDir;
        int     rightDir;

        if (gameManager.CustomInput.GetKey(KeyCode.LeftShift))
        {
            accel = Acceleration;
            animator.SetBool("Accelerating", true);
        }
        else
            animator.SetBool("Accelerating", false);

        if (gameManager.CustomInput.GetAxisRaw("Vertical") > 0)
        {
            if (curSpeed > ForwardSpeed)
                lerpTime = accel > 1 ? RUNNING_LERP : BRAKING_LERP;
            else
                lerpTime = WALKING_LERP;

            curSpeed     = Mathf.Lerp(curSpeed, ForwardSpeed * accel, lerpTime) * SPEED_DEVIATION;
            moveForward  = transform.forward * curSpeed;
        }
        else if (gameManager.CustomInput.GetAxisRaw("Vertical") < 0)
        {
            lerpTime    = WALKING_LERP;  
            curSpeed    = Mathf.Lerp(curSpeed, -BackwardSpeed / 2, lerpTime) * SPEED_DEVIATION;
            moveForward = transform.forward * curSpeed;
        }
        else
        {
            lerpTime    = curSpeed > ForwardSpeed ? BRAKING_LERP : WALKING_LERP;
            curSpeed    = Mathf.Lerp(curSpeed, 0, lerpTime) * SPEED_DEVIATION;
            moveForward = transform.forward * curSpeed;
        }

        if (gameManager.CustomInput.GetAxisRaw("Horizontal") > 0)
        {
            if (curSpeed > ForwardSpeed)
            {
                transform.forward = Vector3.Lerp(transform.forward, Quaternion.Euler(0, 1, 0) * transform.forward, RunRotSpeed);
            }
            else
            {
                moveSide = transform.right * SideSpeed * SPEED_DEVIATION;
            }
        }
        else if (gameManager.CustomInput.GetAxisRaw("Horizontal") < 0)
        {
            if (curSpeed > ForwardSpeed)
            {
                transform.forward = Vector3.Lerp(transform.forward, Quaternion.Euler(0, -1, 0) * transform.forward, RunRotSpeed);
            }
            else
            {
                moveSide = -transform.right * SideSpeed * SPEED_DEVIATION;
            }
        }

        movement   = moveForward + moveSide + Physics.gravity;

        forwardDir = Vector3.Angle(transform.forward, moveForward) < 90 ? 1 : -1;
        rightDir   = Vector3.Angle(transform.right, moveSide) < 90 ? 1 : -1;

        controller.Move(movement * Time.deltaTime);

        animator.SetFloat("Straight Speed", forwardDir * Mathf.Abs(moveForward.magnitude));
        animator.SetFloat("Side Speed", rightDir * Mathf.Abs(moveSide.magnitude));
    }
}
