using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    private bool stealed = false;

    public bool Stealed
    {
        get
        {
            return stealed;
        }
        set
        {
            stealed = value;
        }
    }
}
