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
            // getcomponent �÷��̾� ����
            // �ڱ��ڽ��� ����
            // ���� �ο�(��Ʈ�ѷ� �����ð� ��Ȱ��ȭ?) Ȥ�� �̵��ӵ� 0
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
