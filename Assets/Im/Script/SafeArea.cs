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
        if(gameManager.PlayerAliveDictionary[v.ViewID])
            outAreaPlayer.Add(v.ViewID);
        // 플레이어인지 체크하고
        // 나간 플레이어는 나간플레이어 목록에 추가하고
        // 나간 플레이어를 서버에 전송
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

    public void GameEnd()
    {
        StopAllCoroutines();
    }
}
