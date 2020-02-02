using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static float PLAYER_HEIGHT_OFFSET = 1.0f;

    public static int   BUILDINGS_LAYER = 10;

    public static IEnumerator WaitForAnimation(Animation animation)
    {
        while (animation.isPlaying)
            yield return null;

        /*do
        {
            yield return null;
        } while (animation.isPlaying);*/
    }
}
