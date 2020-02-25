using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInput : MonoBehaviour
{
    private bool blocked = false;

    public bool GetKeyDown(KeyCode c)
    {
        if (blocked)
            return false;
        else
            return Input.GetKeyDown(c);
    }

    public bool GetKey(KeyCode c)
    {
        if (blocked)
            return false;
        else
            return Input.GetKey(c);
    }

    public float GetAxis(string axisName)
    {
        if (blocked)
            return 0;
        else
            return Input.GetAxis(axisName);
    }

    public float GetAxisRaw(string axisName)
    {
        if (blocked)
            return 0;
        else
            return Input.GetAxisRaw(axisName);
    }

    public void Block()
    {
        blocked = true;
    }

    public void Unblock()
    {
        blocked = false;
    }
}
