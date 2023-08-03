using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class BallBase : MonoBehaviourPun
{
    [SerializeField] protected float throwPower = 10;
    [SerializeField] protected float speed = 1.5f;
    [SerializeField] protected float overlapAreaRange = 3;
    [SerializeField] protected LayerMask mask;

    protected bool isEnded;
    protected Player player;
    protected float curTime;
    Rigidbody rb;

    protected void Start()
    {
        if(photonView.IsMine)
            StartCoroutine(rrr());
    }
    protected IEnumerator rrr()
    {
        Vector3 targetPos = (transform.position + (transform.forward * throwPower));
        Vector3 startPos = transform.position;
        Vector3 upPos = startPos + new Vector3(0, 3, 0);
        
        float xSpeed = 0;
        float ySpeed = 0;
        float zSpeed = 0;
        isEnded = false;
        while (!isEnded)
        {
            xSpeed = Mathf.Lerp(startPos.x, targetPos.x, curTime);
            zSpeed = Mathf.Lerp(startPos.z, targetPos.z, curTime);
            ySpeed = Mathf.Lerp(Mathf.Lerp(startPos.y, upPos.y, curTime), Mathf.Lerp(upPos.y, 0, curTime), curTime);
            transform.position = new Vector3(xSpeed, ySpeed, zSpeed);
            curTime += Time.deltaTime * speed;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        curTime = 0;
        PhotonNetwork.Destroy(gameObject);
    }
    private void Shot()
    {
        rb.AddForce(transform.forward * throwPower + Vector3.up * 2, ForceMode.Impulse);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 3 && collision.gameObject.layer != 6)
            return;
        isEnded = true;      
        // 만약 자기 자신 혹은 트리거용 콜라이더가 아니라면
        photonView.RPC("RequestExplosion", RpcTarget.MasterClient, transform.position, transform.rotation);
    }

    [PunRPC]
    protected void RequestExplosion(Vector3 pos, Quaternion rot, PhotonMessageInfo info)
    {
        float sentTime = (float)info.SentServerTime;
        photonView.RPC("ResultExplosion", RpcTarget.AllViaServer, pos, rot, sentTime);
    }
    [PunRPC]
    protected virtual void ResultExplosion(Vector3 pos, Quaternion rot, float sentTime)
    {
        float lag = (float)(PhotonNetwork.Time - sentTime);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }
}
