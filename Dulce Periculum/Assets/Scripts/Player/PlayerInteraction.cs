using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float INTERACTION_DISTANCE;

    void Start()
    {
        
    }

    void Update()
    {
        RaycastHit   hit;
        Interactable interactable;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Physics.Raycast(transform.position + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET, transform.forward, out hit, INTERACTION_DISTANCE))
            {
                if (hit.transform.gameObject.tag.Equals("Interactable"))
                {
                    interactable = hit.transform.gameObject.GetComponent<Interactable>();
                    if (interactable)
                    {
                        print("Interact");
                        interactable.Interact();
                    }
                }
            }
        }
    }
}
