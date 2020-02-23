using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetUper : MonoBehaviour
{
    public PlayerData          playerData;
    public GoblinSquadData[]   goblinSquadsData;
    public VillagerSquadData[] villagerSquadData;

    void Awake()
    {
        GameObject playerInstance;
        playerInstance                   = Instantiate(playerData.prefab, playerData.startPoint.transform.position, Quaternion.identity);
        playerInstance.transform.forward = playerData.forward;
        Destroy(playerData.startPoint);

        for (int i = 0; i < goblinSquadsData.Length; i++)
        {
            GameObject squad;

            squad = new GameObject("Goblin squad " + (i + 1).ToString());
            squad.AddComponent<GoblinsManager>();
            squad.GetComponent<GoblinsManager>().Initialize(goblinSquadsData[i]);
        }

        for (int i = 0; i < villagerSquadData.Length; i++)
        {
            GameObject squad;

            squad = new GameObject("Villager squad " + (i + 1).ToString());
            squad.AddComponent<VillagersManager>();
            squad.GetComponent<VillagersManager>().Initialize(villagerSquadData[i]);
        }

        Destroy(gameObject);
    }
}
