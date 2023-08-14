using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameCamera : MonoBehaviour
{
    [SerializeField] InGameManager inGameManager;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    [SerializeField] int targetIndex;
    [SerializeField] bool isAlive;

    void Awake()
    {
        isAlive = true;
    }

    public void ChangeCameraMode(bool value)
    {
        isAlive = value;
    }

    void OnMoveCamera(InputValue inputValue)
    {
        if (!isAlive)
        {
            //Debug.Log(inputValue.Get<float>());
            if(inputValue.Get<float>() > 0)
            {
                targetIndex++;
                if(targetIndex > inGameManager.PlayerIDDictionary.Count)
                    targetIndex = 0;
            }
            else
            {
                targetIndex--;
                if(targetIndex < 0)
                targetIndex = inGameManager.PlayerIDDictionary.Count;
            }

            int index = 0;
            foreach(var kvp in inGameManager.PlayerIDDictionary)
            {
                if(index == targetIndex)
                {
                    virtualCamera.Follow = PhotonView.Find(inGameManager.PlayerIDDictionary[kvp.Key]).transform;
                    virtualCamera.LookAt = PhotonView.Find(inGameManager.PlayerIDDictionary[kvp.Key]).transform;
                    break;
                }
                index++;
            }
        }
    }
}
