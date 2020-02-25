using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsHandler : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerFight fight;
    private Sword       sword;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        fight       = GetComponent<PlayerFight>();
        sword       = fight.Sword.GetComponent<Sword>();
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

    public void SwitchFireOffHandler()
    {
        sword.SwitchFireOff();
    }

    public void SwitchFireOnHandler()
    {
        sword.SwitchFireOn();
    }

    public void BlockInput()
    {
        gameManager.CustomInput.Block();
    }

    public void UnblockInput()
    {
        gameManager.CustomInput.Unblock();
    }
}
