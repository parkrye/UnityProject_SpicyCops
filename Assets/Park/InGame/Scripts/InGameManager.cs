using Cinemachine;
using Jeon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class InGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] float nowTime, totalTime;
    public float NowTime { get { return nowTime; } set {  nowTime = value; } }
    public float TotalTime { get { return totalTime; } set { totalTime = value; } }

    [SerializeField] SafeArea safeArea;

    [SerializeField] Dictionary<int, float> playerDictionary;   // <view id, aggro>
    public Dictionary<int, float> PlayerDictionary { get {  return playerDictionary; } }

    [SerializeField] Enemy enemy;

    [SerializeField] List<Transform> startPositions;    // initial start positions
    [SerializeField] int startNum;

    [SerializeField] InGameUIController inGameUIController;

    [SerializeField] CinemachineVirtualCamera playerCamera;

    UnityEvent<float> timeEvent;
    UnityEvent<Dictionary<int, float>> playerAggroEvent;

    void Start()
    {
        playerDictionary = new Dictionary<int, float>();
        timeEvent = new UnityEvent<float>();
        playerAggroEvent = new UnityEvent<Dictionary<int, float>>();
        inGameUIController.Initialize();

        // Normal game mode
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LocalPlayer.SetLoad(true);
        }
        // Debug game mode
        else
        {
            PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #region Connection
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions() { IsVisible = false };
        PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(DebugGameSetupDelay());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected : {cause}");
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // ���� �۾� ��� ����
        if (newMasterClient.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            StartCoroutine(TimerRoutine());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey(GameData.PLAYER_LOAD))
        {
            Debug.Log($"{PlayerLoadCount() == PhotonNetwork.PlayerList.Length}");
            if (PlayerLoadCount() == PhotonNetwork.PlayerList.Length)
            {
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.CurrentRoom.SetLoadTime((int)PhotonNetwork.Time);
            }
            else
            {
                Debug.Log($"Wait players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}");
            }
        }
    }

    public override void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(GameData.LOAD_TIME))
        {
            StartCoroutine(GameStartTimer());
        }
    }

    IEnumerator DebugGameSetupDelay()
    {
        yield return new WaitForSeconds(1f);
        DebugGameStart();
    }

    int PlayerLoadCount()
    {
        int loadCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad())
                loadCount++;
        }
        return loadCount;
    }
    #endregion

    #region Start Game
    IEnumerator GameStartTimer()
    {
        yield return new WaitForEndOfFrame();
        GameStart();
    }

    void GameStart()
    {
        // ĳ���� ����
        // UI�� �÷��̾� ���� ����
        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
        photonView.RPC("RequestAddPlayer", RpcTarget.AllBufferedViaServer, player.GetComponent<PhotonView>().ViewID);
        inGameUIController.SetPlayerPhotonView(player.GetComponent<PhotonView>());
        playerCamera.Follow = player.transform;

        // ���� �۾�
        if (PhotonNetwork.IsMasterClient)
        {
            // ���ʹ̰� �� ��ũ��Ʈ ����
            StartCoroutine(TimerRoutine());
            safeArea.GameStartSetting();
        }
    }

    private void DebugGameStart()
    {
        // ĳ���� ����
        // UI�� �÷��̾� ���� ����
        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
        photonView.RPC("RequestAddPlayer", RpcTarget.AllBufferedViaServer, player);
        inGameUIController.SetPlayerPhotonView(player.GetComponent<PhotonView>());
        playerCamera.Follow = player.transform;

        // ���� �۾�
        if (PhotonNetwork.IsMasterClient)
        {
            // ���ʹ̰� this ����
            StartCoroutine(TimerRoutine());
            safeArea.GameStartSetting();
        }
    } 
    #endregion

    #region Timer
    IEnumerator TimerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            photonView.RPC("RequestTimer", RpcTarget.AllViaServer, 0.1f);
        }
    }

    [PunRPC]
    void RequestTimer(float time, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        ResultTimer(time + lag);
    }

    public void ResultTimer(float time)
    {
        NowTime += time;
        timeEvent?.Invoke(NowTime);
    }

    public void AddTimeEventListenr(UnityAction<float> lister)
    {
        timeEvent.AddListener(lister);
    }

    public void RemoveTimeEventListener(UnityAction<float> lister)
    {
        timeEvent?.RemoveListener(lister);
    }
    #endregion

    #region Adding Player
    [PunRPC]
    void RequestAddPlayer(int photonViewID, PhotonMessageInfo info)
    {
        ResultAddPlayer(photonViewID);
    }

    public void ResultAddPlayer(int photonViewID)
    {
        GameObject player = PhotonView.Find(photonViewID).gameObject;
        playerDictionary.Add(player.GetComponent<PhotonView>().ViewID, 0f);
        player.transform.position = startPositions[startNum++].position;
        inGameUIController.AddOtherPlayerPhotonView(player.GetComponent<PhotonView>());
        // �÷��̾ this ����
    }
    #endregion

    #region Aggro Manager
    public void ModifyPlayerAggro(int targetPlayerPhotonViewID, float modifyValue)
    {
        photonView.RPC("RequestModifyPlayerAggro", RpcTarget.AllViaServer, targetPlayerPhotonViewID, modifyValue);
    }

    [PunRPC]
    void RequestModifyPlayerAggro(int targetPlayerPhotonViewID, float modifyValue, PhotonMessageInfo info)
    {
        float sum = playerDictionary[targetPlayerPhotonViewID] + modifyValue;
        if (sum < 0f)
            sum = 0f;
        if (sum > GameData.MAX_AGGRO)
            sum = GameData.MAX_AGGRO;
        ResultModifyPlayerAggro(targetPlayerPhotonViewID, sum);
    }

    void ResultModifyPlayerAggro(int targetPlayerPhotonViewID, float modifyValue)
    {
        playerDictionary[targetPlayerPhotonViewID] = modifyValue;
        playerAggroEvent?.Invoke(playerDictionary);
    }

    public void AddPlayerAggroEventListenr(UnityAction<Dictionary<int, float>> lister)
    {
        playerAggroEvent.AddListener(lister);
    }

    public void RemovePlayerAggroEventListenr(UnityAction<Dictionary<int, float>> lister)
    {
        playerAggroEvent?.RemoveListener(lister);
    } 
    #endregion
}