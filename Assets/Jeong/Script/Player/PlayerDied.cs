using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDied : MonoBehaviourPun
{
    Animator animator;
    [SerializeField] InGameManager gameManager;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void DoDeath()
    {
        photonView.RPC("DieDead", RpcTarget.AllViaServer);
        // 경륜이한테 InGameManager받아오면 PlayerAliveDictionary false로
    }

    [PunRPC]
    protected void DieDead()
    {
        animator.SetBool("IsDied", true);
    }
}
