using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviourPun
{
    float maxScale = 300;
    float minScale = 55;
    float time = 120;

    public List<int> inAreaPlayer;
    public List<int> outAreaPlayer;
    public InGameManager gameManager;


    public void GameStartSetting()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        inAreaPlayer = new List<int>();
        outAreaPlayer = new List<int>();
        foreach(int viewID in gameManager.PlayerAggroDictionary.Keys)
        {
            inAreaPlayer.Add(viewID);
        }
        AreaEnable();
    }
    private void OnCollisionExit(Collision collision)
    {
        PhotonView collide = collision.gameObject.GetComponent<PhotonView>();
        if (inAreaPlayer.Contains(collide.ViewID))
        {
            outAreaPlayer.Add(collide.ViewID);
            inAreaPlayer.Remove(collide.ViewID);
        }
        // 플레이어인지 체크하고
        // 나간 플레이어는 나간플레이어 목록에 추가하고
        // 나간 플레이어를 서버에 전송
    }
    private void OnCollisionEnter(Collision collision)
    {
        PhotonView collide = collision.gameObject.GetComponent<PhotonView>();
        if (inAreaPlayer.Contains(collide.ViewID))
        {
            inAreaPlayer.Add(collide.ViewID);
            outAreaPlayer.Remove(collide.ViewID);
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
            transform.localScale = new Vector3(scale, 20, scale);
            rate += Time.deltaTime / time;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("End");
    }
}
