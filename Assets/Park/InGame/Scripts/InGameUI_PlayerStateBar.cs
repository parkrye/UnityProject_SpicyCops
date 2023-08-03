using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI_PlayerStateBar : MonoBehaviour
{
    [SerializeField] Animator uiAnimator;

    void Update()
    {
        ShowAndHideBar();
    }

    void ShowAndHideBar()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            uiAnimator.SetTrigger("OnTab");
        }
    }
}
