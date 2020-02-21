using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFight : Fight
{
    public GameObject   AXE;

    private const
            int         HIT_TYPES_COUNT = 2;

    private Animator    animator;
    private GoblinVoice voice;
    private int         hitNum;

    void Start()
    {
        animator = GetComponent<Animator>();
        voice    = GetComponent<GoblinVoice>();
        hitNum   = 0;
    }

    public void Attack()
    {
        if (!AXE.GetComponent<Weapon>().IsWaitingForCooldown())
        {
            animator.SetInteger("Hit", hitNum);
            animator.SetTrigger("Attack");

            hitNum = (hitNum + 1) % HIT_TYPES_COUNT;

            AXE.GetComponent<Weapon>().WaitForCooldown(COOLDOWN_TIME);

            voice.MakeAttackSound();
        }
    }
}
