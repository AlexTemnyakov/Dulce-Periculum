using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUtils : MonoBehaviour
{
    private GameObject       player;
    private List<GameObject> village;

    void Start()
    {
        GameObject[] tmp;

        player  = GameObject.FindGameObjectWithTag("Player");
        tmp     = GameObject.FindGameObjectsWithTag("Village");
        village = new List<GameObject>(tmp);

        if (!player)
        {
            Debug.LogError("There is no object with the tag Player");
        }
    }

    void Update()
    {
        StartCoroutine(CheckVillage());
    }

    public float DistanceToPlayer(Vector3 position) { return Vector3.Distance(position, player.transform.position); }
    public bool  IsPlayerVisible(Vector3 position, Vector3 forward, float visibility, float visionAngle)
    {
        RaycastHit hit;
        Vector3    playerDirection = player.transform.position - position;

        //Debug.DrawRay(position, playerDirection * visibility + Vector3.up * playerHeightOffset, Color.black);
        if (Physics.Raycast(position, playerDirection * visibility + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET, out hit, visibility))
        {
            if (hit.transform.gameObject == player && Vector3.Angle(playerDirection, forward) <= visionAngle)
                return true;
        }

        return false;
    }

    public bool IsPlayerAtAttackDistance(Vector3 position, float attackDistance)
    {
        if (DistanceToPlayer(position) < attackDistance)
            return true;
        else
            return false;
    }

    public GameObject GetHouseInVillage()
    {
        if (village == null || village.Count == 0)
            return null;

        return village[Random.Range(0, village.Count)];
    }

    private IEnumerator CheckVillage()
    {
        for (int i = village.Count - 1; i >= 0; i--)
        {
            if (!village[i] || !village[i].activeInHierarchy)
            {
                village.RemoveAt(i);
            }

            yield return null;
        }
    }
}
