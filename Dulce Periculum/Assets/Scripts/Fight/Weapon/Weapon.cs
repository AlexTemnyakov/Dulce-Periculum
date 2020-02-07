using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public    int  DAMAGE;
    [HideInInspector]
    public    bool hit                = true;

    protected bool waitingForCooldown = false;

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
