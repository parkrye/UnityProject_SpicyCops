using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // Player ªÛ»£¿€øÎ
    [SerializeField] bool debug;
    private bool canInteract = true; 
    [SerializeField] float range;

    [SerializeField] private Transform point;
   
    private Animator anim;

    private void Awake()
    {
        anim = transform.gameObject.GetComponent<Animator>();
    }



    // ªÛ»£¿€øÎ
   /* public void Interact()
    {
        if (canInteract)
        {
            Collider[] colliders = Physics.OverlapSphere(point.position, range);
            foreach (Collider collider in colliders)
            {
                IInteractable interactable = collider.GetComponent<IInteractable>();
                interactable?.Interact(this);
                anim.SetBool("IsPicked", true);

            }
            Debug.Log("Player Interact");
            anim.SetBool("IsPicked", false);
        }

    }*/
    public void Interact()
    {
        StartCoroutine(InteractDelay());
        Collider[] colliders = Physics.OverlapSphere(point.position, range);
        List<IInteractable> interactables = new List<IInteractable>();
        foreach (Collider collider in colliders)
        {
            IInteractable inter = collider.GetComponent<IInteractable>();
            if(inter != null)
                interactables.Add(inter);
        }

        Debug.Log($"{interactables.Count}");
        
        if (interactables.Count < 1)
            return;
        foreach(IInteractable interactable in interactables)
        {
            Debug.Log($"");
            
            interactable?.Interact(this);
        }
        StartCoroutine(PickAnim());
        interactables.Clear();
    }
    IEnumerator PickAnim()
    {
        anim.SetBool("IsPicked", true);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("IsPicked", false);
    }

    private void OnInteract(InputValue value)
    {
       if(!canInteract) 
            return;
        Interact();
    }

    IEnumerator InteractDelay()
    {
        canInteract = false;
        yield return new WaitForSeconds(1f);
        canInteract = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(point.position, range);
    }

    // Item »πµÊ

}
