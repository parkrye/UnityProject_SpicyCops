using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDied : MonoBehaviour
{
    Animator animator;
    [SerializeField] InGameManager gameManager;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void DoDeath()
    {
        animator.SetBool("IsDied", true);
        // 경륜이한테 InGameManager받아오면 PlayerAliveDictionary false로
    }
}
