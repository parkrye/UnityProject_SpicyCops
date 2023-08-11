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
    float oneTick;

    public List<int> outAreaPlayer;
    public InGameManager gameManager;

    private void Awake()
    {
        outAreaPlayer = new List<int>();
        oneTick = 1 / 1200;
    }

    private void GrowLess(float f)
    {
        if (rate > 1)
            return;
        scale = Mathf.Lerp(maxScale, minScale, rate);
        transform.localScale = new Vector3(scale, 8, scale);
        rate += oneTick;
        
    }
    private void OutCheck(float f)
    {
        foreach (int i in outAreaPlayer)
        {
            gameManager.ModifyPlayerAggro(i, 0.5f);
        }
    }
        
    public void GameStartSetting()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        rate = 0;
        scale = maxScale;
        outAreaPlayer = new List<int>();
        gameManager.AddTimeEventListener(GrowLess);
        gameManager.AddTimeEventListener(OutCheck);
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
}
