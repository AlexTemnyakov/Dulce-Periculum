using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : Fight
{
    public  float      HIDE_SWORD_TIME;
    public  GameObject SWORD;

    private const
            int        HIT_TYPES_COUNT  = 4;

    private Animator   animator;
    private float      deltaTime;
    private int        hitNum;

    void Start()
    {
        animator  = GetComponent<Animator>();
        deltaTime = 0;
        hitNum    = 0;
    }

    void Update()
    {
        if (!SWORD.GetComponent<Weapon>().IsWaitingForCooldown())
        {
            if (Input.GetMouseButtonDown(0))
            {
                deltaTime = 0;
                animator.SetBool("Attacking State", true);
                animator.SetTrigger("Attack");
                animator.SetInteger("Hit", hitNum);

                hitNum = (hitNum + 1) % HIT_TYPES_COUNT;

                SWORD.GetComponent<Weapon>().WaitForCooldown(COOLDOWN_TIME);
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
}
