using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsHandler : MonoBehaviour
{
    private PlayerFight fight;
    private Sword       sword;

    void Start()
    {
        fight = GetComponent<PlayerFight>();
        sword = fight.SWORD.GetComponent<Sword>();
    }

    public void UnsheatheSwordHandler()
    {
        sword.UnsheatheSword();
    }

    public void SheatheSwordHandler()
    {
        sword.SheatheSword();
    }

    public void SwordSwingSoundHandler()
    {
        sword.SwordSwingSound();
    }

    public void SwordEnableHandler()
    {
        sword.SwordEnable();
    }

    public void SwordDisableHandler()
    {
        sword.SwordDisable();
    }
}
