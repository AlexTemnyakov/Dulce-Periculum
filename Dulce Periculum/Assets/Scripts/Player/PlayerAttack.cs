using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public  float      HIDE_SWORD_TIME;
    public  float      COOLDOWN_TIME;
    public  GameObject WEAPON;

    private const
            int        HIT_TYPES_COUNT  = 6;

    private Animator   animator;
    private float      deltaTime;
    private int        hitNum;
    private bool       waitingForCooldown;

    void Start()
    {
        animator           = GetComponent<Animator>();
        deltaTime          = 0;
        hitNum             = 0;
        waitingForCooldown = false;
    }

    void Update()
    {
        if (!waitingForCooldown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                deltaTime = 0;
                animator.SetBool("Attacking State", true);
                animator.SetTrigger("Attack");
                animator.SetInteger("Hit", hitNum);

                hitNum = (hitNum + 1) % HIT_TYPES_COUNT;
            }
            else
            {
                deltaTime += Time.deltaTime;

                if (deltaTime > HIDE_SWORD_TIME)
                {
                    deltaTime = 0;
                    animator.SetBool("Attacking State", false);
                }
            }
        }
    }

    public IEnumerator WaitForCooldown()
    {
        waitingForCooldown                        = true;
        WEAPON.GetComponent<Collider>().isTrigger = false;

        yield return new WaitForSeconds(COOLDOWN_TIME);

        waitingForCooldown                        = false;
        WEAPON.GetComponent<Collider>().isTrigger = true;
    }
}
