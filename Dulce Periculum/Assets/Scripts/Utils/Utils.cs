using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
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
