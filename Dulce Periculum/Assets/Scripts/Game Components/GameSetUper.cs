using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetUper : MonoBehaviour
{
    public PlayerData        playerData;
    public GoblinSquadData[] goblinSquadsData;

    public

    void Awake()
    {
        Instantiate(playerData.prefab, playerData.startPoint.transform.position, Quaternion.identity);
        Destroy(playerData.startPoint);

        for (int i = 0; i < goblinSquadsData.Length; i++)
        {
            GameObject squad;

            squad = new GameObject("Goblin squad " + (i + 1).ToString());
            squad.AddComponent<GoblinsManager>();
            squad.GetComponent<GoblinsManager>().Initialize(goblinSquadsData[i]);
        }
    }

    void Update()
    {
        
    }
}
