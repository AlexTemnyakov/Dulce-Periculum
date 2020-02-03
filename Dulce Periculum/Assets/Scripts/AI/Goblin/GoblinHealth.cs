using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHealth : CreatureHealth
{
    void Update()
    {
        if (!IsAlive())
        {
            Animator a = GetComponent<Animator>();
            a.SetTrigger("Death");
        }
    }
}
