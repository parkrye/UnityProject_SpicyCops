using UnityEngine;

public class InGameUI_PlayerStateBar : SceneUI
{
    [SerializeField] Animator uiAnimator;

    public override void Initialize()
    {

    }

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
