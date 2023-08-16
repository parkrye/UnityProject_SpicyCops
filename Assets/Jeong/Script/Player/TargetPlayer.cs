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
                puller.SetPullTarget(other.gameObject); // PlayerPuller�� �� �޼��� ȣ���Ͽ� PullTarget ����
                StartCoroutine(DistanceCheckRoutine(other.transform, puller));
            }
        }

        if (other.CompareTag("Player"))
        {
            PlayerPusher pusher = GetComponent<PlayerPusher>();
            if (pusher != null)
            {
                pusher.SetPullTarget(other.gameObject); // PlayerPuller�� �� �޼��� ȣ���Ͽ� PullTarget ����
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
        //        puller.ClearPullTarget(); // PlayerPuller�� �� �޼��� ȣ���Ͽ� PullTarget ����
        //    }
        //}

        if (other.CompareTag("Player"))
        {
            PlayerPusher pusher = GetComponent<PlayerPusher>();
            if (pusher != null)
            {
                pusher.ClearPullTarget(); // PlayerPuller�� �� �޼��� ȣ���Ͽ� PullTarget ����
            }
        }
    }

}
