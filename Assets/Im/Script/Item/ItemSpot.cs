using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [SerializeField] float regenCoolTime;
    bool isActive;

    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾� ������Ʈ üũ ��
        // �÷��̾� ��ȣ�ۿ� �̺�Ʈ�� �����ʷ� �߰�
        // player.CommuEvent.AddListener(() => {Communicate();});
    }
    private void OnCollisionExit(Collision collision)
    {
        // �÷��̾� ������Ʈ üũ ��
        // �÷��̾� ��ȣ�ۿ� �̺�Ʈ �����ʿ��� ����
    }
    private void Communicate()
    {
        if (isActive) 
        {
            // ������ ������ ����
            // �� �� ��Ÿ�� ����
            StartCoroutine(RegenItem());
        }

    }
    IEnumerator RegenItem()
    {
        float t = 0;
        isActive = false;
        while(t < regenCoolTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        isActive = true;
    }
}
