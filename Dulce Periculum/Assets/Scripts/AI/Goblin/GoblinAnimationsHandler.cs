using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnimationsHandler : MonoBehaviour
{
    private GoblinFight fight;
    private Axe         axe;

    void Start()
    {
        fight = GetComponent<GoblinFight>();
        axe   = fight.AXE.GetComponent<Axe>();
    }

    void Update()
    {
        
    }

    public void FootL()
    {

    }

    public void FootR()
    {

    }

    public void AxeSwingSoundHandler()
    {
        axe.AxeSwingSound();
    }

    public void AxeEnableHandler()
    {
        axe.AxeEnable();
    }

    public void AxeDisableHandler()
    {
        axe.AxeDisable();
    }
}
