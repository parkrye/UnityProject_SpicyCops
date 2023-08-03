using UnityEngine;

public class InGameUI_ActionIcon : SceneUI
{
    [SerializeField] KeyCode actionKey;

    [SerializeField] float coolTime, nowCoolTime;
    [SerializeField] bool isUsed;

    public override void Initialize()
    {

    }

    void Update()
    {
        UseAction();
        CoolTimeStateCheck();
    }

    void UseAction()
    {
        if (!isUsed)
        {
            if (Input.GetKeyDown(actionKey))
            {
                isUsed = true;
                images["ActionCoolImage"].enabled = true;
            }
        }
    }

    void CoolTimeStateCheck()
    {
        if(isUsed)
        {
            nowCoolTime += Time.deltaTime;
            if(nowCoolTime >= coolTime)
            {
                isUsed = false;
                nowCoolTime = 0f;
                images["ActionCoolImage"].enabled = false;
                texts["ActionCoolText"].text = "";
            }
            else
            {
                texts["ActionCoolText"].text = ((int)nowCoolTime).ToString();
            }
        }
    }
}
