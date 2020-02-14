using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static float PLAYER_HEIGHT_OFFSET = 2.0f;

    public static float GetHeight(Vector3 point)
    {
        RaycastHit hit;

        if (Physics.Raycast(point, Vector3.down, out hit, 100))
        {
            return Vector3.Distance(point, hit.point);
        }
        else
        {
            return int.MaxValue;
        }
    }

    public static IEnumerator WaitFor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
