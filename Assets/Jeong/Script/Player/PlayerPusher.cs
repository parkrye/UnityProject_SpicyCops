using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPusher : MonoBehaviourPun
{
    [SerializeField] private bool debug;
    [SerializeField] private float pushForce; // 미는 힘
    [SerializeField] private float pushRange; // 밀 수 있는 범위

    // ******************* 아래는 서버에서 받아올 항목*********************
    [SerializeField] private float pushDuration; // 미는 시간
    [SerializeField] private float pushCooltime; // 밀기 쿨타임

    private float pushingStartTime; // 밀기 시작 시간
    private bool canPush = true; // 밀기 가능한지 여부
    // ******************************************************************

    [SerializeField] public bool isPushing = false;

    private PlayerInput playerInput;
    private Animator anim;
    public UnityEvent PushCoolEvent;


    [SerializeField] private GameObject currentPullTarget;
    [SerializeField] private GameObject targetPlayer;

    private void Start()
    {
        anim = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();

        if (!photonView.IsMine)
            Destroy(playerInput);
    }
    private void OnPush(InputValue value)
    {
        
        // z키를 누르면 
        if (value.isPressed && canPush)
        {
            if (currentPullTarget != null)
            {
                isPushing = true;
                canPush = false;
                pushingStartTime = Time.time;
                FindTargetPlayer();

                PushTarget();
                StartCoroutine(PushCooldown());
            }
        }
            
    }

    private void PushTarget()
    {
        // 타겟을 찾으면 민다.
        if (isPushing && targetPlayer != null)
        {
            Push(targetPlayer);
        }
    }


    private IEnumerator PushCooldown()
    {
        PushCoolEvent?.Invoke();
        yield return new WaitForSeconds(GameData.PushCoolTime);
        canPush = true; // 쿨타임 종료 후 다시 잡을 수 있도록 설정
        currentPullTarget = null; // 쿨타임이 끝나면 PullTarget 초기화
    }

    [PunRPC]
    private void PushAnim()
    {
        //Debug.LogError($"{PhotonNetwork.LocalPlayer.ActorNumber} : Push Anim Played");
        anim.SetTrigger("IsPushed");
    }

    private void FindTargetPlayer() // 잡아당길 Player 탐색
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pushRange);

        // 가장 가까운 Player와의 거리를 최대값으로 초기화하고, Player의 GameObject를 null로 초기화한다.
        float closestDistance = float.MaxValue;
        GameObject closestPlayer = null;

        // 범위안의 Collider를 모두 확인하는 작업
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject.CompareTag("Player"))
            {
                CapsuleCollider capsuleCollider = collider.gameObject.GetComponent<CapsuleCollider>();
                if (capsuleCollider != null)
                {
                    // 현재 Player와 다른 Player 사이의 거리 계산
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    // 거리가 더 가까운 Player를 찾으면
                    if (distance < closestDistance)
                    {
                        // 최대값으로 초기화했던 가장 가까운거리를 새로찾은 거리로 갱신하고
                        closestDistance = distance;
                        // 마찬가지로 null이 들어있던 Player의 Gameobject도 새로 갱신한다.
                        closestPlayer = collider.gameObject;
                    }
                }
            }
        }
        // 가장 가까운 Player을 탐색하고 targetPlayer 변수에 갱신한다.
        targetPlayer = closestPlayer;
    }

    // 밀기
    public void Push(GameObject player)
    {
        //Debug.LogError($"{PhotonNetwork.LocalPlayer.ActorNumber} : Push Anim");
        photonView.RPC("PushAnim", RpcTarget.AllViaServer);
        Vector3 directionToTarget = (player.transform.position - transform.position).normalized * pushForce;
        PlayerMover mover = player.GetComponent<PlayerMover>();
        mover.photonView.RPC("mePushing", RpcTarget.AllViaServer, directionToTarget);
    }

    public void SetPullTarget(GameObject target)
    {
        currentPullTarget = target;
    }

    public void ClearPullTarget()
    {
        currentPullTarget = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pushRange);
    }
}
