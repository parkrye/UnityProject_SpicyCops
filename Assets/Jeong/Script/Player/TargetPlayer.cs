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
                puller.SetPullTarget(gameObject); // PlayerPuller�� �� �޼��� ȣ���Ͽ� PullTarget ����
            }
        }

        if (other.CompareTag("Player"))
        {
            PlayerPusher pusher = other.GetComponent<PlayerPusher>();
            if (pusher != null)
            {
                pusher.SetPullTarget(gameObject); // PlayerPuller�� �� �޼��� ȣ���Ͽ� PullTarget ����
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
                puller.ClearPullTarget(); // PlayerPuller�� �� �޼��� ȣ���Ͽ� PullTarget ����
            }
        }

        if (other.CompareTag("Player"))
        {
            PlayerPusher pusher = other.GetComponent<PlayerPusher>();
            if (pusher != null)
            {
                pusher.ClearPullTarget(); // PlayerPuller�� �� �޼��� ȣ���Ͽ� PullTarget ����
            }
        }
    }

}
