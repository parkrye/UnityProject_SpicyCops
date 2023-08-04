using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviourPun
{
    [SerializeField] float maxScale = 250;
    [SerializeField] float minScale = 55;
    [SerializeField] float time = 120;

    public List<Player> inAreaPlayer;
    public List<Player> outAreaPlayer;

    public void GameStartSetting()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        inAreaPlayer = new List<Player>();
        outAreaPlayer = new List<Player>();
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            inAreaPlayer.Add(player);
        }
        AreaEnable();
    }
    private void OnCollisionExit(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            outAreaPlayer.Add(player);
            inAreaPlayer.Remove(player);
        }
        // �÷��̾����� üũ�ϰ�
        // ���� �÷��̾�� �����÷��̾� ��Ͽ� �߰��ϰ�
        // ���� �÷��̾ ������ ����
    }
    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            inAreaPlayer.Add(player);
            outAreaPlayer.Remove(player);
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
