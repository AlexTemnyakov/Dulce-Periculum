using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : Fight
{
    public  float      HideSwordTime;
    public  GameObject Sword;

    private const
            int        HIT_TYPES_COUNT  = 4;

    private GameManager gameManager;
    private Animator    animator;
    private float       deltaTime;
    private int         hitNum;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        animator    = GetComponent<Animator>();
        deltaTime   = 0;
        hitNum      = 0;
    }

    void Update()
    {
        if (!Sword.GetComponent<Weapon>().IsWaitingForCooldown())
        {
            if (gameManager.CustomInput.GetKeyDown(KeyCode.Mouse0))
            {
                if (!animator.GetBool("Attacking State"))
                {
                    deltaTime = 0;
                    animator.SetBool("Attacking State", true);
                }
                else
                {
                    animator.SetTrigger("Attack");
                    animator.SetInteger("Hit", hitNum);

                    hitNum = (hitNum + 1) % HIT_TYPES_COUNT;

                    Sword.GetComponent<Weapon>().WaitForCooldown(COOLDOWN_TIME);
                }
            }
            else
            {
                deltaTime += Time.deltaTime;

                if (deltaTime > HideSwordTime)
                {
                    deltaTime = 0;
                    animator.SetBool("Attacking State", false);
                }
            }
        }
    }
}
