using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [SerializeField] float regenCoolTime;
    bool isActive;

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어 컴포넌트 체크 후
        // 플레이어 상호작용 이벤트에 리스너로 추가
        // player.CommuEvent.AddListener(() => {Communicate();});
    }
    private void OnCollisionExit(Collision collision)
    {
        // 플레이어 컴포넌트 체크 후
        // 플레이어 상호작용 이벤트 리스너에서 제거
    }
    private void Communicate()
    {
        if (isActive) 
        {
            // 랜덤한 아이템 지급
            // 그 후 쿨타임 적용
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
