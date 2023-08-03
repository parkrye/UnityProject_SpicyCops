using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI_ActionIcon : MonoBehaviour
{
    [SerializeField] Image coolTimeImage;
    [SerializeField] TMP_Text coolTimeText;
    [SerializeField] KeyCode actionKey;

    [SerializeField] float coolTime, nowCoolTime;
    [SerializeField] bool isUsed;

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
                coolTimeImage.enabled = true;
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
                coolTimeImage.enabled = false;
                coolTimeText.text = "";
            }
            else
            {
                coolTimeText.text = ((int)nowCoolTime).ToString();
            }
        }
    }
}
