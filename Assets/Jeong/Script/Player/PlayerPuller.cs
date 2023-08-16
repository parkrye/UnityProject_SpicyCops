using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPuller : MonoBehaviourPun
{
    [SerializeField] private bool debug; // Gizmo확인용

    [SerializeField] private float pullForce = 1f; // 잡아당기는 힘
    [SerializeField] private float pullRange; // 잡아당기는 범위

    // ******************* 아래는 서버에서 받아올 항목*********************
    [SerializeField] private float pullDuration = 2f; // 잡기 지속 시간
    [SerializeField] private float pullCooltime = 5f; // 잡기 쿨타임

    private float pullingStartTime; // 잡기 시작 시간
    private bool canPull = true; // 잡기 가능한지 여부
                                 // ******************************************************************
    private IEnumerator pullCooltimeCoroutine;

    [SerializeField] public bool isPulling = false;

    [SerializeField] private GameObject currentPullTarget;
    [SerializeField] private GameObject targetPlayer;
    public UnityEvent PullCoolEvent;


    private PlayerInput playerInput;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();

        if (!photonView.IsMine)
            Destroy(playerInput);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        if (isPulling)
        {
            // 현재 시간이 지속시간을 초과하면 잡기 상태 해제
            if (Time.time - pullingStartTime > pullDuration)
            {
                isPulling = false;
                photonView.RPC("ChangePullState", RpcTarget.AllViaServer, false);
                targetPlayer = null;
            }

            else
            {
                PullTarget();
            }
        }
    }

    private void OnPull(InputValue value)
    {
        if (!photonView.IsMine)
            return;
         //Debug.LogError($" isPressed : {value.isPressed}, canPull : {canPull}");
        if (value.isPressed && canPull)
        {

            if (currentPullTarget != null)
            {
                isPulling = true;
                pullingStartTime = Time.time;
                FindTargetPlayer();
                photonView.RPC("ChangePullState", RpcTarget.AllViaServer, true);
                canPull = false;
            }
        }
        else
        {
            PullingEnd();

            // PullCooldown 코루틴 시작
        }
    }

    [PunRPC]
    public void ChangePullState(bool state)
    {
        anim.SetBool("IsPulled", state);

    }

    public void PullingEnd()
    {
        //Debug.LogError("PullingEnd");
        currentPullTarget?.GetComponent<PlayerMover>()?.photonView.RPC("mePullingFinish", RpcTarget.AllViaServer);
        isPulling = false;
        photonView.RPC("ChangePullState", RpcTarget.AllViaServer, false);
        targetPlayer = null;
        StartCoroutine(PullCooldown());
        currentPullTarget = null; // 쿨타임이 끝나면 PullTarget 초기화
    }

    private IEnumerator PullCooldown()
    {
        PullCoolEvent?.Invoke();
        yield return new WaitForSeconds(pullCooltime);
        canPull = true; // 쿨타임 종료 후 다시 잡을 수 있도록 설정
    }

    private void PullTarget()
    {
        //Debug.LogError($"PullTarget {isPulling}, {targetPlayer}");
        // 타겟을 찾으면 잡아당긴다.
        if (isPulling && targetPlayer != null)
        {
            Pull(targetPlayer);
        }
    }

    private void FindTargetPlayer() // 잡아당길 Player 탐색
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRange);

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

    // 잡아당기기 
    private void Pull(GameObject player)
    {
        //Debug.LogError("Pull 호출");
        PlayerMover mover = player.GetComponent<PlayerMover>();
        mover.RequestPullStart(photonView.ViewID);

        // 잡아당기는 Player만 바라보도록 회전시킨다.
        transform.LookAt(player.transform.position, Vector3.up);
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pullRange);
    }
}