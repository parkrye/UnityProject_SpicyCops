using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviourPun
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

        
        if (interactables.Count < 1)
            return;
        foreach(IInteractable interactable in interactables)
        {
            interactable?.Interact(this);
        }
        photonView.RPC("PickAnim",RpcTarget.AllViaServer);
        interactables.Clear();
    }
    [PunRPC]
    public void PickAnim()
    {
        anim.SetTrigger("IsPicked");
       
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
