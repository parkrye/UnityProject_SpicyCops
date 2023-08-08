using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class BallBase : MonoBehaviourPun
{
    [SerializeField] protected float throwPower = 5;
    [SerializeField] protected float speed = 1.5f;
    [SerializeField] protected float overlapAreaRange = 3;

    public InGameManager gameManager;

    protected bool isEnded;
    protected int viewId;
    protected float curTime;
    protected Rigidbody rb;
    protected Collider col;
    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        gameManager = GameObject.Find("InGameManager").GetComponent<InGameManager>();
        rb.mass = 0.4f;
    }
    protected void Start()
    {
        /*
        if(photonView.IsMine)
            StartCoroutine(rrr());
        */
        Shot();
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
            ySpeed = Mathf.Lerp(Mathf.Lerp(startPos.y, upPos.y, curTime), Mathf.Lerp(upPos.y, 0.5f, curTime), curTime);
            transform.position = new Vector3(xSpeed, ySpeed, zSpeed);
            curTime += Time.deltaTime * speed;
            yield return null;
        }
        curTime = 0;
        yield return new WaitForSeconds(3);
        PhotonNetwork.Destroy(gameObject);
    }
    private void Shot()
    {
        rb.AddForce((transform.forward * throwPower) + (Vector3.up * throwPower), ForceMode.Impulse);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == 0)
            rb.AddForce(transform.forward * -0.5f * throwPower, ForceMode.Impulse);
        if (collision.gameObject.layer != 3 && collision.gameObject.layer != 6)
            return;
        PhotonView view = collision.gameObject.GetComponent<PhotonView>();
        if (view != null && view.ViewID == viewId)
            return;
        Debug.Log("Fall");
        isEnded = true;
        rb.velocity = Vector3.zero;
        col.enabled = false;
        rb.useGravity = false;
        if(photonView.IsMine)
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

    public void SetPlayer(int viewId)
    {
        this.viewId = viewId;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, overlapAreaRange);
    }
}
