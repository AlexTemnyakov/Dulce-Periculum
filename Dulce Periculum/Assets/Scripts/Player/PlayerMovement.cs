using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0, 10f)]
    public  float               SENSITIVITY;
    public  float               FORWARD_SPEED;
    public  float               BACKWARD_SPEED;
    public  float               SIDE_SPEED;
    public  float               ACCELERATION;
    [Range(0, 0.3f)]
    public  float               RUN_ROT_SPEED;

    private const
            float               SPEED_DEVIATION = 0.99f;
    private const
            float               BRAKING_LERP    = 0.01f;
    private const
            float               WALKING_LERP    = 0.5f;
    private const
            float               RUNNING_LERP    = 0.05f;

    private CharacterController controller;
    private Animator            animator;
    private float               curSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator   = GetComponent<Animator>();
        curSpeed   = 0;

        animator.applyRootMotion = false;
    }

    void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
        if (curSpeed < FORWARD_SPEED)
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * SENSITIVITY);
        else
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * SENSITIVITY / 2);
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            accel = ACCELERATION;
            animator.SetBool("Accelerating", true);
        }
        else
            animator.SetBool("Accelerating", false);

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (curSpeed > FORWARD_SPEED)
                lerpTime = accel > 1 ? RUNNING_LERP : BRAKING_LERP;
            else
                lerpTime = WALKING_LERP;

            curSpeed     = Mathf.Lerp(curSpeed, FORWARD_SPEED * accel, lerpTime) * SPEED_DEVIATION;
            moveForward  = transform.forward * curSpeed;
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            lerpTime    = WALKING_LERP;  
            curSpeed    = Mathf.Lerp(curSpeed, -BACKWARD_SPEED / 2, lerpTime) * SPEED_DEVIATION;
            moveForward = transform.forward * curSpeed;
        }
        else
        {
            lerpTime    = curSpeed > FORWARD_SPEED ? BRAKING_LERP : WALKING_LERP;
            curSpeed    = Mathf.Lerp(curSpeed, 0, lerpTime) * SPEED_DEVIATION;
            moveForward = transform.forward * curSpeed;
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (curSpeed > FORWARD_SPEED)
            {
                transform.forward = Vector3.Lerp(transform.forward, Quaternion.Euler(0, 1, 0) * transform.forward, RUN_ROT_SPEED);
            }
            else
            {
                moveSide = transform.right * SIDE_SPEED * SPEED_DEVIATION;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            if (curSpeed > FORWARD_SPEED)
            {
                transform.forward = Vector3.Lerp(transform.forward, Quaternion.Euler(0, -1, 0) * transform.forward, RUN_ROT_SPEED);
            }
            else
            {
                moveSide = -transform.right * SIDE_SPEED * SPEED_DEVIATION;
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
