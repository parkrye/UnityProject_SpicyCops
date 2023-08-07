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
    #region Variables and Properties
    [SerializeField] float nowTime, totalTime;
    public float NowTime { get { return nowTime; } set { nowTime = value; } }
    public float TotalTime { get { return totalTime; } set { totalTime = value; } }
    [SerializeField] int readyPlayerCount;

    [SerializeField] SafeArea safeArea;

    [SerializeField] Dictionary<int, float> playerAggroDictionary;   // <view id, aggro>
    [SerializeField] Dictionary<int, bool> playerAliveDictionary;   // <view id, alve>
    [SerializeField] Dictionary<int, int> playerIDDictonary;    // <actor number, view id>
    public Dictionary<int, float> PlayerAggroDictionary { get { return playerAggroDictionary; } }
    public Dictionary<int, bool> PlayerAliveDictionary { get { return playerAliveDictionary; } }
    public Dictionary<int, int> PlayerIDDictionary { get { return playerIDDictonary; } }

    [SerializeField] Enemy enemy;

    [SerializeField] List<Transform> startPositions;    // initial start positions
    [SerializeField] int startNum;

    [SerializeField] InGameUIController inGameUIController;

    [SerializeField] CinemachineVirtualCamera playerCamera;

    UnityEvent<float> timeEvent;
    UnityEvent<Dictionary<int, float>> playerAggroEvent;
    UnityEvent<Dictionary<int, bool>> playerAliveEvent;
    #endregion

    void Start()
    {
        timeEvent = new UnityEvent<float>();
        playerAggroEvent = new UnityEvent<Dictionary<int, float>>();
        playerAliveEvent = new UnityEvent<Dictionary<int, bool>>();
        playerAggroDictionary = new Dictionary<int, float>();
        playerAliveDictionary = new Dictionary<int, bool>();
        playerIDDictonary = new Dictionary<int, int>();
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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //int leftPlayerActorNum = otherPlayer.ActorNumber;
        //GameObject leftPlayerCharacter = PhotonView.Find(playerActorNumberViewIDDictonary[leftPlayerActorNum]).gameObject;

        //playerViewIDAggroValueDictionary.Remove(leftPlayerCharacter.GetComponent<PhotonView>().ViewID);
        //playerActorNumberViewIDDictonary.Remove(leftPlayerActorNum);
        //PhotonNetwork.Destroy(leftPlayerCharacter);
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
        // 방장 작업 대신 수행
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
        // 캐릭터 생성
        // UI에 플레이어 정보 저장
        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
        photonView.RPC("RequestAddPlayer", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.ActorNumber, player.GetComponent<PhotonView>().ViewID, GameData.CurrentAvatarNum, GameData.CurrentColorNum);
        inGameUIController.SetPlayerPhotonView(player.GetComponent<PhotonView>());
        playerCamera.Follow = player.transform;

        // 방장 작업
        if (PhotonNetwork.IsMasterClient)
        {
            // 에너미가 이 스크립트 참조
            StartCoroutine(TimerRoutine());
            safeArea.GameStartSetting();
            StartCoroutine(GameSetting());
        }
    }

    private void DebugGameStart()
    {
        // 캐릭터 생성
        // UI에 플레이어 정보 저장
        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
        photonView.RPC("RequestAddPlayer", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.ActorNumber, player.GetComponent<PhotonView>().ViewID, GameData.CurrentAvatarNum, GameData.CurrentColorNum);
        inGameUIController.SetPlayerPhotonView(player.GetComponent<PhotonView>());
        playerCamera.Follow = player.transform;

        // 방장 작업
        if (PhotonNetwork.IsMasterClient)
        {
            // 에너미가 this 참조
            StartCoroutine(TimerRoutine());
            safeArea.GameStartSetting();
            StartCoroutine(GameSetting());
        }
    } 

    IEnumerator GameSetting()
    {
        while(readyPlayerCount < PhotonNetwork.PlayerList.Length)
        {
            yield return new WaitForSeconds(0.1f);
        }
        enemy.Seting();

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
    void RequestAddPlayer(int playerActorNumber, int photonViewID, int avatarNum, int colorNum, PhotonMessageInfo info)
    {
        ResultAddPlayer(playerActorNumber, photonViewID, avatarNum, colorNum);
    }

    public void ResultAddPlayer(int playerActorNumber, int photonViewID, int avatarNum, int colorNum)
    {
        PhotonView player = PhotonView.Find(photonViewID);
        playerAggroDictionary.Add(photonViewID, 0f);
        playerAliveDictionary.Add(photonViewID, true);
        playerIDDictonary.Add(playerActorNumber, photonViewID);
        player.transform.position = startPositions[startNum++].position;
        inGameUIController.AddOtherPlayerPhotonView(player);
        InGameAvatarManager playerAvatarManager = player.GetComponent<InGameAvatarManager>();
        /*playerAvatarManager.Initialize(avatarNum, colorNum);*/ // 스킨관련떄문에 주석처리함
        // 플레이어가 this 참조
        ModifyPlayerAggro(photonViewID, 0f);
        photonView.RPC("RequestCreatedPlayer", RpcTarget.MasterClient);

    }
    [PunRPC]
    void RequestCreatedPlayer(PhotonMessageInfo info)
    {
        readyPlayerCount++;
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
        if (playerAliveDictionary[targetPlayerPhotonViewID])
        {
            float sum = playerAggroDictionary[targetPlayerPhotonViewID] + modifyValue;
            if (sum < 0f)
                sum = 0f;
            if (sum > GameData.MAX_AGGRO)
                sum = GameData.MAX_AGGRO;
            ResultModifyPlayerAggro(targetPlayerPhotonViewID, sum);
        }
    }

    void ResultModifyPlayerAggro(int targetPlayerPhotonViewID, float modifyValue)
    {
        playerAggroDictionary[targetPlayerPhotonViewID] = modifyValue;
        playerAggroEvent?.Invoke(playerAggroDictionary);
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

    #region Alive Manager
    public void ModifyPlayerAlive(int targetPlayerPhotonViewID, bool isAlive)
    {
        photonView.RPC("RequestModifyPlayerAlive", RpcTarget.AllViaServer, targetPlayerPhotonViewID, isAlive);
    }

    [PunRPC]
    void RequestModifyPlayerAlive(int targetPlayerPhotonViewID, bool isAlive, PhotonMessageInfo info)
    {
        ResultModifyPlayerAlive(targetPlayerPhotonViewID, isAlive);
    }

    void ResultModifyPlayerAlive(int targetPlayerPhotonViewID, bool isAlive)
    {
        playerAliveDictionary[targetPlayerPhotonViewID] = isAlive;
        playerAliveEvent?.Invoke(playerAliveDictionary);
    }

    public void AddPlayerAliveEventListenr(UnityAction<Dictionary<int, bool>> lister)
    {
        playerAliveEvent.AddListener(lister);
    }

    public void RemovePlayerAliveEventListenr(UnityAction<Dictionary<int, bool>> lister)
    {
        playerAliveEvent?.RemoveListener(lister);
    }
    #endregion
}