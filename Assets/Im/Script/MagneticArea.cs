using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticArea : MonoBehaviour
{
    // List<Player> inAreaPlayer;
    // List<Player> outAreaPlayer;
    private void Awake()
    {
        // 모든 플레이어 리스트 서버에서 받아서 안에있는 플레이어 리스트에 추가
    }
    private void OnCollisionExit(Collision collision)
    {
        // 플레이어인지 체크하고
        // 나간 플레이어는 나간플레이어 목록에 추가하고
        // 나간 플레이어를 서버에 전송
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 위의 반대
    }
}
