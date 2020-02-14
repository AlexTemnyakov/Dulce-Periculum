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
        Interactable interactable;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET,
                                                            INTERACTION_DISTANCE - transform.forward.magnitude);
            foreach (Collider c in hitColliders)
            {
                if (c.gameObject.CompareTag("Interactable"))
                {
                    interactable = c.gameObject.GetComponent<Interactable>();
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
