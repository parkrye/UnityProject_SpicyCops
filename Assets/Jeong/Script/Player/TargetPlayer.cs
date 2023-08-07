using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPuller puller = other.GetComponent<PlayerPuller>();
            if (puller != null)
            {
                puller.SetPullTarget(gameObject); // PlayerPuller의 새 메서드 호출하여 PullTarget 설정
            }
        }

        if (other.CompareTag("Player"))
        {
            PlayerPusher pusher = other.GetComponent<PlayerPusher>();
            if (pusher != null)
            {
                pusher.SetPullTarget(gameObject); // PlayerPuller의 새 메서드 호출하여 PullTarget 설정
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPuller puller = other.GetComponent<PlayerPuller>();
            if (puller != null)
            {
                puller.ClearPullTarget(); // PlayerPuller의 새 메서드 호출하여 PullTarget 해제
            }
        }

        if (other.CompareTag("Player"))
        {
            PlayerPusher pusher = other.GetComponent<PlayerPusher>();
            if (pusher != null)
            {
                pusher.ClearPullTarget(); // PlayerPuller의 새 메서드 호출하여 PullTarget 설정
            }
        }
    }

}
