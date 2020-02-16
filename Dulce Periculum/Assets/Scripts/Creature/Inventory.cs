using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public  GameObject       inventory;

    private List<GameObject> items = new List<GameObject>();

    public void Add(GameObject o)
    {
        o.transform.SetParent(inventory.transform);
        o.SetActive(false);
        items.Add(o);
    }

    public IEnumerator DropAll()
    {
        if (items.Count <= 0)
            yield break;

        int angle = 360 / items.Count;

        inventory.transform.parent = null;

        for (int i = items.Count - 1; i >= 0; i--)
        {
            Vector3 position, shift;

            shift      = Quaternion.Euler(0, angle * i, 0) * new Vector3(1, 0, 1) * 2;
            position   = transform.position + shift;
            position.y = Utils.GetTerrainHeight(position.x, position.z);

            items[i].transform.parent   = null;
            items[i].transform.position = position;
            items[i].SetActive(true);

            items.RemoveAt(i);

            yield return null;
        }

        Destroy(inventory);
    }
}
