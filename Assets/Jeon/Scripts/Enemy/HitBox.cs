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
        if (other.gameObject.tag == "Player" /*other.GetComponent("�������̽�")*/)
        {
            enemy.anim.SetTrigger("InArea");
            // �÷��̾ �״� �Լ� ȣ�� other.GetComponent("�������̽�");
        }
    }
    private void Attack()
    {

    }
}
