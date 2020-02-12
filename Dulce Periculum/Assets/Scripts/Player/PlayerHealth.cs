using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CreatureHealth
{
    void Start()
    {
        Settings s    = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
        maxHealth     = s.PLAYER_HEALTH;
        currentHealth = maxHealth;
    }

    private void Die()
    {

    }
}
