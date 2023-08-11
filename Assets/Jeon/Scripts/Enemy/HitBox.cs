using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitBox : MonoBehaviourPun
{
    [SerializeField] private PoliceEnemy enemy;
    [SerializeField] InGameManager inGameManager;

    private void Awake()
    {
        enemy = GetComponentInParent<PoliceEnemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + other.gameObject.layer);
        if (other.gameObject.layer == 3)
        {
            enemy.AddPlayer(other.GetComponent<PhotonView>().ViewID);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            enemy.RemovePlayer(other.GetComponent<PhotonView>().ViewID);
        }

    }
    
}