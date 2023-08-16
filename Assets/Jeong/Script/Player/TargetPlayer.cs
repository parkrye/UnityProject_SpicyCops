using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    [SerializeField] float maxDistance = 2;
    [SerializeField] bool targeting;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPuller puller = GetComponent<PlayerPuller>();
            if (puller != null)
            {
                targeting = true;
                puller.SetPullTarget(other.gameObject); // PlayerPuller의 새 메서드 호출하여 PullTarget 설정
                StartCoroutine(DistanceCheckRoutine(other.transform, puller));
            }
        }

        if (other.CompareTag("Player"))
        {
            PlayerPusher pusher = GetComponent<PlayerPusher>();
            if (pusher != null)
            {
                pusher.SetPullTarget(other.gameObject); // PlayerPuller의 새 메서드 호출하여 PullTarget 설정
            }
        }
    }

    IEnumerator DistanceCheckRoutine(Transform target, PlayerPuller puller)
    {
        while (targeting)
        {
            if (Vector3.Distance(transform.position, target.position) > maxDistance)
            {
                targeting = false;
                puller.PullingEnd();
                puller.ClearPullTarget();
            }
            yield return null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    PlayerPuller puller = other.GetComponent<PlayerPuller>();
        //    if (puller != null)
        //    {
        //        puller.ClearPullTarget(); // PlayerPuller의 새 메서드 호출하여 PullTarget 해제
        //    }
        //}

        if (other.CompareTag("Player"))
        {
            PlayerPusher pusher = GetComponent<PlayerPusher>();
            if (pusher != null)
            {
                pusher.ClearPullTarget(); // PlayerPuller의 새 메서드 호출하여 PullTarget 설정
            }
        }
    }

}
