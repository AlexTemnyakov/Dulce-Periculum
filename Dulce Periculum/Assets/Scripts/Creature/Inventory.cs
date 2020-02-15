using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<GameObject> items = new List<GameObject>();

    public void Add(GameObject o)
    {
        o.SetActive(false);
        items.Add(o);
    }

    public void DropAll()
    {
        int angle = 360 / items.Count;

        for (int i = items.Count; i >= 0; i--)
        {
            Vector3 position, shift;

            shift    = Quaternion.Euler(0, angle * i, 0) * new Vector3(1, 0, 1) * 2;
            position = transform.position + shift;
            position = position + Vector3.down * Utils.GetHeight(position);

            items[i].transform.position = position;
            items[i].SetActive(true);

            items.RemoveAt(i);
        }
    }
}
