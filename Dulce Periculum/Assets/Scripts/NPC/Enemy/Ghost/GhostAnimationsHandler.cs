using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimationsHandler : MonoBehaviour
{
    private GameManager gameManager;
    private GhostFight  fight;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        fight       = GetComponent<GhostFight>();
    }

    void Update()
    {

    }

    public void Spell()
    {
        GameObject instance;

        instance = Instantiate(fight.Fireball, transform.position + Vector3.up + transform.forward, Quaternion.identity);

        instance.GetComponent<Magic>().Initialize(gameManager.Player.transform.position + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET - transform.position);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
