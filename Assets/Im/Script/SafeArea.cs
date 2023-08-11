using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviourPun
{
    float maxScale = 300;
    float minScale = 55;
    float time = 120;
    float rate;
    float scale;

    public List<int> outAreaPlayer;
    public InGameManager gameManager;

    private void Awake()
    {
        outAreaPlayer = new List<int>();
    }
        
    public void GameStartSetting()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        rate = 0;
        scale = maxScale;
        outAreaPlayer = new List<int>();
        AreaEnable();
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerMover mover = other.GetComponent<PlayerMover>();
        if (mover == null)
            return;
        PhotonView v = other.GetComponent<PhotonView>();
        if (v == null)
            return;
        if (outAreaPlayer.Contains(v.ViewID))
            return;
        outAreaPlayer.Add(v.ViewID);
        // �÷��̾����� üũ�ϰ�
        // ���� �÷��̾�� �����÷��̾� ��Ͽ� �߰��ϰ�
        // ���� �÷��̾ ������ ����
    }
    private void OnTriggerEnter(Collider other)
    {
        PhotonView v = other.GetComponent<PhotonView>();
        if (v == null)
            return;
        if (outAreaPlayer.Contains(v.ViewID))
        {
            outAreaPlayer.Remove(v.ViewID);
        }
    }

    public void AreaEnable()
    {
        StartCoroutine(GrowLess());
        StartCoroutine(OutCheck());
    }
    IEnumerator GrowLess()
    {
        float rate = 0;
        float scale = maxScale;
        while (rate <= 1)
        {
            scale = Mathf.Lerp(maxScale, minScale, rate);
            transform.localScale = new Vector3(scale, 8, scale);
            rate += Time.deltaTime / time;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("End");
    }
    IEnumerator OutCheck()
    {
        while (true) 
        {
            foreach (int i in outAreaPlayer)
            {
                gameManager.ModifyPlayerAggro(i, 1);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
