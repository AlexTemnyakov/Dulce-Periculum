using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static float PLAYER_HEIGHT_OFFSET = 2.0f;

    public static float GetTerrainHeight(float x, float z)
    {
        Vector3 point = new Vector3(x, 0, z);

        return GetCurrentTerrain(point).SampleHeight(point);
    }

    public static float GetTerrainHeight(Vector3 point)
    {
        point = new Vector3(point.x, 0, point.z);

        return GetCurrentTerrain(point).SampleHeight(point);
    }

    public static IEnumerator WaitFor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private static Terrain GetCurrentTerrain(Vector3 position)
    {
        Terrain[] terrains = Terrain.activeTerrains;

        if (terrains.Length == 0)
            return null;

        // Measure the distance between the terrain center and the passed position.
        Vector3 center  = new Vector3(terrains[0].transform.position.x + terrains[0].terrainData.size.x / 2, 
                                      position.y, 
                                      terrains[0].transform.position.z + terrains[0].terrainData.size.z / 2);
        float   lowDist = (center - position).sqrMagnitude;
        int     terrainIndex = 0;

        for (int i = 1; i < terrains.Length; i++)
        {
            Terrain terrain;
            Vector3 terrainPos;
            float   dist;

            terrain    = terrains[i];
            terrainPos = terrain.GetPosition();
            center     = new Vector3(terrains[i].transform.position.x + terrains[i].terrainData.size.x / 2,
                                     position.y,
                                     terrains[i].transform.position.z + terrains[i].terrainData.size.z / 2);

            dist = (center - position).sqrMagnitude;
            if (dist < lowDist)
            {
                lowDist      = dist;
                terrainIndex = i;
            }
        }

        return terrains[terrainIndex];
    }
}
