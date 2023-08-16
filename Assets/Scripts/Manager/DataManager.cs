using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    // Player's Data Script

    // Player Dictionary
    // Enemy

    [SerializeField] float clock;
    public float Clock { get { return clock; } set { clock = value; } }

    [SerializeField] PhotonView photonView;

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("TimeClocking", RpcTarget.AllViaServer);
        }
        else
        {
            // µ¿±âÈ¸
            // Player's Data script => foreach => Master => copy
        }
    }

    [PunRPC]
    public void TimeClocking(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        Clock += Time.deltaTime + lag;
    }
}