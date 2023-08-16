using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    // Interface로 상호작용 대상에 대해 Interact를 실행한다.
    public void Interact(PlayerInteraction interaction); 
}
