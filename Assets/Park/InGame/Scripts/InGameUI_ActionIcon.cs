using System.Collections;
using UnityEngine;

public class InGameUI_ActionIcon : SceneUI
{
    [SerializeField] float nowCoolTime;
    [SerializeField] int actionNum;

    public void UseAction()
    {
        images["ActionCoolImage"].enabled = true;
        StartCoroutine(CoolTimeStateCheck());
    }

    IEnumerator CoolTimeStateCheck()
    {
        nowCoolTime = 0f;
        float coolTime = 0f;
        if (actionNum == 0)
            coolTime = GameData.PushCoolTime;
        else
            coolTime = GameData.PullCoolTime;

        while (nowCoolTime <= coolTime)
        {
            texts["ActionCoolText"].text = ((int)(coolTime - nowCoolTime) + 1).ToString();
            nowCoolTime += Time.deltaTime;
            yield return null;
        }
        nowCoolTime = 0f;
        images["ActionCoolImage"].enabled = false;
        texts["ActionCoolText"].text = "";
    }
}
