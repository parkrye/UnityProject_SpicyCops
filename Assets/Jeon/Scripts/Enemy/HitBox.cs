using Jeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private PoliceEnemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<PoliceEnemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" /*other.GetComponent("인터페이스")*/)
        {
            enemy.anim.SetTrigger("InArea");
            // 플레이어가 죽는 함수 호출 other.GetComponent("인터페이스");
        }
    }
    private void Attack()
    {

    }
}
