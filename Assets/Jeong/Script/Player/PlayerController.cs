using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField] private float curAggro;
    [SerializeField] private float maxAggro;

    private enum PlayerState { Idle, Attack, Hit, Slow, Strun, Dead, Size } // 대기, 공격, 피격, 둔화, 기절, 잡힘

    private PlayerState curState;

    private void Start()
    {
        curState = PlayerState.Idle;
    }

    private void Update()
    {
        switch (curState)
        {
            case PlayerState.Idle:
                DoIdle();
                break;

            case PlayerState.Attack:
                DoAttack();
                break;

            case PlayerState.Hit:
                DoHit();
                break;

            case PlayerState.Slow:
                DoSlow();
                break;

            case PlayerState.Strun:
                DoSturn();
                break;

            case PlayerState.Dead:
                DoDead();
                break;
        }
    }

    private void AggroGauge()
    {
        // Player 어그로 수치 관리
        // 인게임매니저를 통해 어그로수치를 서버에 발신요청한다.
        // 서버에서 수락하면 어그로수치를 인게임매니저를 통해 다른 Player와 Enemy에게 어그로 수치를 발신한다.
        // 인게임 매니저를 통해 다른Player의 어그로수치를 받아온다.
    }

    private void DoIdle()
    {
        // 대기상태, 다른 상태 진입
        // Attack, Hit,  Sturn, Dead 전환

        // 공격 아이템 습득시 DoAttack 상태전환
        // 피격 당할시 DoHit 상태전환
        // Enemy에게 잡힐시 Dead 상태전환
    }


    private void DoAttack()
    {
        // 공격상태
        // Idle 전환
        // 아이템 사용

        // 공격 아이템 사용 후 DoIdle 상태전환
        // Enemy에게 잡힐시 Dead 상태전환
    }

    private void DoHit()
    {
        // 피격상태, 다른 상태 진입
        // Slow, Sturn 전환 

        // 둔화 아이템 피격시 Slow 상태전환
        // 기절 아이템 피격시 Strun 상태전환
        // Enemy에게 잡힐시 Dead 상태전환
    }

    private void DoSlow()
    {
        // 아이템 피격 둔화
        // Idle, Dead 전환

        // 둔화 후 일정시간 이후 Idle 상태전환
        // Enemy에게 잡힐시 Dead 상태전환
    }

    private void DoSturn()
    {
        // 아이템 피격 기절
        // Idle, Dead 전환

        // 기절 후 일정시간 이후 Idle 상태전환
        // Enemy에게 잡힐시 Dead 상태전환
    }

    private void DoDead()
    {
        // Enemy에게 잡힘

        // Enmey가 Player에게 닿으면 DoCaught함수를 술래가 호출한다.

        // 죽는 애니메이션 재생
        // 어그로 수치 0으로 변경
        // 입력된 데이터를 0으로 초기화 후 입력 차단
    }
}
