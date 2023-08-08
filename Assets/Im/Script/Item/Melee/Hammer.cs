using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Hammer", menuName = "Item/Melee/Hammer")]
public class Hammer : MeleeItem
{
    List<PlayerInput> inputs = new List<PlayerInput>();
    protected override void MeeleAttack(Vector3 pos, Quaternion rot, float lag, int viewId)
    {
        base.MeeleAttack(pos, rot, lag, viewId);
        foreach(Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<PhotonView>().ViewID == viewId)
                return;
            PlayerInput input = collider.gameObject.GetComponent<PlayerInput>();
            if (input == null)
                return;
            inputs.Add(input);
            // getcomponent 플레이어 검출
            // 자기자신은 리턴
            // 기절 부여(컨트롤러 일정시간 비활성화?) 혹은 이동속도 0
        }
    }
    public override IEnumerator Cor(int viewId)
    {
        yield return new WaitForEndOfFrame();
        foreach(PlayerInput input in inputs)
        {
            DisableInput(input);
        }
        yield return new WaitForSeconds(2);
        foreach (PlayerInput input in inputs)
        {
            EnableInput(input);
        }
        yield return new WaitForEndOfFrame();
        inputs.Clear();
    }
    private void EnableInput(PlayerInput input)
    {
        input.enabled = true;
    }
    private void DisableInput(PlayerInput input)
    {
        input.enabled = false;
    }
}
