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
        // ��������� InGameManager�޾ƿ��� PlayerAliveDictionary false��
    }

    [PunRPC]
    protected void DieDead()
    {
        animator.SetBool("IsDied", true);
    }
}
