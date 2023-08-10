using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // Player 상호작용
    [SerializeField] bool debug;

    [SerializeField] Transform point;
    [SerializeField] float range;

    private bool canInteract = false;

    private Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    // 상호작용 오브젝트 탐색
    public void FindInteract()
    {
        
    }

    // 상호작용
    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(point.position, range);
        foreach (Collider collider in colliders)
        {
            anim.SetBool("IsPicked", true);
            IInteractable interactable = collider.GetComponent<IInteractable>();
            interactable?.Interact(this);
            anim.SetBool("IsPicked", false);
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

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(point.position, range);
    }

    // Item 획득

}
