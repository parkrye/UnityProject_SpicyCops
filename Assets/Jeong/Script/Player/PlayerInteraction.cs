using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // Player ªÛ»£¿€øÎ
    [SerializeField] bool debug;

    [SerializeField] Transform point;
    [SerializeField] float range;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        
        Collider[] colliders = Physics.OverlapSphere(point.position, range);
        foreach (Collider collider in colliders)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();
            interactable?.Interact();
            anim.SetTrigger("IsPicked");
        }
        Debug.Log("Player Interact");
        
    }

    private void OnInteract(InputValue value)
    {
        Interact();
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(point.position, range);
    }

    // Item »πµÊ

}
