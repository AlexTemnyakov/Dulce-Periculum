using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFight : Fight
{
    public GameObject FIREBALL_1;

    private Animator animator;
    private bool     waitingForCooldown = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (!IsWaitingForCooldown())
        {
            //animator.SetInteger("Hit", hitNum);
            animator.SetTrigger("Attack");

            //hitNum = (hitNum + 1) % HIT_TYPES_COUNT;

            //AXE.GetComponent<Weapon>().WaitForCooldown(COOLDOWN_TIME);

            //voice.MakeAttackSound();
            StartCoroutine(WaitForCooldown(COOLDOWN_TIME));
        }
    }

    public IEnumerator WaitForCooldown(float time)
    {
        waitingForCooldown = true;

        yield return new WaitForSeconds(time);

        waitingForCooldown = false;
    }

    public bool IsWaitingForCooldown()
    {
        return waitingForCooldown;
    }
}
