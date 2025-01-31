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
using Photon.Pun.UtilityScripts;

public class InGameManager : MonoBehaviourPunCallbacks
{
    #region Variables and Properties
    [SerializeField] float nowTime, totalTime;
    public float NowTime { get { return nowTime; } set { nowTime = value; } }
    public float TotalTime { get { return totalTime; } set { totalTime = value; } }
    [SerializeField] int readyPlayerCount;

    [SerializeField] SafeArea safeArea;
    [SerializeField] InGameCamera inGameCamera;

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

    [SerializeField] Stack<(int, int)> rankStack;
    public Stack<(int, int)> RankStack { get { return rankStack; } }

    [SerializeField] ItemManager itemManager;
    public ItemManager ItemManager { get {  return itemManager; } }
    [SerializeField] bool started = false;
    [SerializeField] bool isPlaying;
    public bool IsPlaying { get { return isPlaying; } }
    public bool Started { get { return started; } }
    [SerializeField] AudioSource startAudio, endAudio, bgm;

    UnityEvent<Dictionary<int, float>> playerAggroEvent;
    UnityEvent<Dictionary<int, bool>> playerAliveEvent;
    UnityEvent<(int, int)> playerDeadEvent;
    #endregion

    void Awake()
    {
        playerAggroEvent = new UnityEvent<Dictionary<int, float>>();
        playerAliveEvent = new UnityEvent<Dictionary<int, bool>>();
        playerAggroDictionary = new Dictionary<int, float>();
        playerAliveDictionary = new Dictionary<int, bool>();
        playerIDDictonary = new Dictionary<int, int>();
        rankStack = new Stack<(int, int)> ();
        playerDeadEvent = new UnityEvent<(int, int)> ();
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
        //Debug.Log($"Disconnected : {cause}");
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnLeftRoom()
    {
        //Debug.Log("Left Room");
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // 방장 작업 대신 수행
        if (newMasterClient.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {

        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey(GameData.PLAYER_LOAD))
        {
            //Debug.Log($"{PlayerLoadCount() == PhotonNetwork.PlayerList.Length}");
            if (PlayerLoadCount() == PhotonNetwork.PlayerList.Length)
            {
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.CurrentRoom.SetLoadTime((int)PhotonNetwork.Time);
            }
            else
            {
                //Debug.Log($"Wait players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}");
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

    [PunRPC]
    void GameStart()
    {
        // 캐릭터 생성
        // UI에 플레이어 정보 저장
        // 넘버링
        
        GameObject player = PhotonNetwork.Instantiate("Player", startPositions[PhotonNetwork.LocalPlayer.GetPlayerNumber()].position, Quaternion.identity, 0);
        player.GetComponent<PlayerPusher>().PushCoolEvent.AddListener(inGameUIController.push.UseAction);
        player.GetComponent<PlayerPuller>().PullCoolEvent.AddListener(inGameUIController.pull.UseAction);
        int avatarNum = 0, colorNum = 0;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(GameData.PLAYER_AVATAR, out object avatarValue))
        {
            avatarNum = (int) avatarValue;
        }
        if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(GameData.PLAYER_COLOR, out object colorValue))
        {
            colorNum = (int) colorValue;
        }

        photonView.RPC("RequestAddPlayer", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.ActorNumber, player.GetComponent<PhotonView>().ViewID, avatarNum, colorNum);
        inGameUIController.SetPlayerPhotonView(player.GetComponent<PhotonView>());
        playerCamera.Follow = player.transform;
        playerCamera.LookAt = player.transform;

        StartCoroutine(GameSetting());

        // 방장 작업
        if (PhotonNetwork.IsMasterClient)
        {
            // 에너미가 이 스크립트 참조
        }
    }

    private void DebugGameStart()
    {
        GameStart();
    } 

    IEnumerator GameSetting()
    {
        //Debug.Log("Wait Game Setting");
        yield return new WaitUntil(() => { return readyPlayerCount == PhotonNetwork.PlayerList.Length; });
        //Debug.Log("All Ready");

        itemManager.Init();
        enemy.Seting();
        if (PhotonNetwork.IsMasterClient)
            safeArea.GameStartSetting();
        started = true;
        isPlaying = true;
        StartCoroutine(TimerRoutine());
        startAudio.Play();
        //Debug.Log("Done Game Setting");
    }
    #endregion

    #region Adding Player
    [PunRPC]
    void RequestAddPlayer(int playerActorNumber, int photonViewID, int avatarNum, int colorNum)
    {
        //Debug.Log($"{photonView.Owner} request add Player");
        ResultAddPlayer(playerActorNumber, photonViewID, avatarNum, colorNum);
    }

    public void ResultAddPlayer(int playerActorNumber, int photonViewID, int avatarNum, int colorNum)
    {
        PhotonView player = PhotonView.Find(photonViewID);
        //Debug.Log(player.gameObject.name + playerActorNumber);

        playerAggroDictionary.Add(photonViewID, 0f);
        playerAliveDictionary.Add(photonViewID, true);
        playerIDDictonary.Add(playerActorNumber, photonViewID);
        inGameUIController.AddOtherPlayerPhotonView(player);

        InGameAvatarManager playerAvatarManager = player.GetComponent<InGameAvatarManager>();
        playerAvatarManager.Initialize(avatarNum, colorNum);
        // 플레이어가 this 참조
        ModifyPlayerAggro(photonViewID, 0f);

        player.GetComponent<PlayerMover>().Initialize();
        //Debug.Log($"{photonView.Owner} request create Player");
        if (playerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            photonView.RPC("RequestCreatedPlayer", RpcTarget.AllBufferedViaServer);
        }
    }
    [PunRPC]
    void RequestCreatedPlayer(PhotonMessageInfo info)
    {
        readyPlayerCount++;
        //Debug.Log(readyPlayerCount);
    }

    #endregion

    #region Aggro Manager
    public void ModifyPlayerAggro(int targetPlayerPhotonViewID, float modifyValue)
    {
        photonView.RPC("RequestModifyPlayerAggro", RpcTarget.AllViaServer, targetPlayerPhotonViewID, modifyValue);
    }

    [PunRPC]
    void RequestModifyPlayerAggro(int targetPlayerPhotonViewID, float modifyValue)
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

    public void AddPlayerAggroEventListener(UnityAction<Dictionary<int, float>> lister)
    {
        playerAggroEvent.AddListener(lister);
    }

    public void RemovePlayerAggroEventListener(UnityAction<Dictionary<int, float>> lister)
    {
        playerAggroEvent?.RemoveListener(lister);
    }
    #endregion

    #region Rank(Dead) Manager
    public void PlayerDead(int targetPlayerPhotonViewID)
    {
        //Debug.Log($"Requset Player Dead {targetPlayerPhotonViewID}");
        photonView.RPC("RequestPlayerDead", RpcTarget.AllViaServer, targetPlayerPhotonViewID);
    }

    [PunRPC]
    void RequestPlayerDead(int targetPlayerPhotonViewID)
    {
        ResultPlayerDead(targetPlayerPhotonViewID);
    }

    void ResultPlayerDead(int targetPlayerPhotonViewID)
    {
        //Debug.Log($"Result Player Dead {playerAliveDictionary[targetPlayerPhotonViewID]}");
        if (!started)
            return;
        if (!playerAliveDictionary[targetPlayerPhotonViewID])
            return;

        foreach (KeyValuePair<int, int> pair in PlayerIDDictionary)
        {
            if (pair.Value == targetPlayerPhotonViewID && pair.Key == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                inGameCamera.ChangeCameraMode(false);
                break;
            }
        }

        //Debug.LogError($"{PhotonNetwork.LocalPlayer.ActorNumber} Result Player Dead {targetPlayerPhotonViewID}");
        playerAliveDictionary[targetPlayerPhotonViewID] = false;
        rankStack.Push((readyPlayerCount - rankStack.Count, targetPlayerPhotonViewID));
        //Debug.LogError($"{PhotonNetwork.LocalPlayer.ActorNumber} Rank Stack {rankStack.Peek()}");
        playerAliveEvent?.Invoke(playerAliveDictionary);
        playerDeadEvent?.Invoke(rankStack.Peek());

        //Debug.Log($"Rank Stack {rankStack.Count}, {readyPlayerCount - 1}");
        if (rankStack.Count >= readyPlayerCount - 1)
        {
            GameEnd();
        }
    }

    public void AddPlayerAliveEventListener(UnityAction<Dictionary<int, bool>> lister)
    {
        playerAliveEvent.AddListener(lister);
    }

    public void RemovePlayerAliveEventListener(UnityAction<Dictionary<int, bool>> lister)
    {
        playerAliveEvent?.RemoveListener(lister);
    }

    public void AddPlayerDeadEventListener(UnityAction<(int, int)> lister)
    {
        playerDeadEvent.AddListener(lister);
    }

    public void RemovePlayerDeadEventListener(UnityAction<(int, int)> lister)
    {
        playerDeadEvent?.RemoveListener(lister);
    }
    #endregion

    #region End Game Manager
    public void GameEnd()
    {
        if (started && IsPlaying)
        {
            isPlaying = false;
            safeArea.GameEnd();

            foreach (KeyValuePair<int, bool> pair in playerAliveDictionary)
            {
                if (pair.Value)
                {
                    rankStack.Push((1, pair.Key));
                }
            }
            //Debug.Log($"End Game {rankStack.Count}");
            bgm.Stop();
            endAudio.Play();
            inGameUIController.EndGameUI();
        }
    }
    #endregion

    #region timer
    IEnumerator TimerRoutine()
    {
        yield return new WaitUntil(() => { return started; });
        while (isPlaying && NowTime <= TotalTime)
        {
            NowTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        GameEnd();
    }
    #endregion

    #region Item
    public void SetItemUI(int itemNum)
    {
        inGameUIController.SettingItemIcon(itemNum);
    }

    public void DrawEffect(int usingPlayerActorNumber)
    {
        photonView.RPC("RequestDrawEffect", RpcTarget.AllViaServer, usingPlayerActorNumber);
    }

    [PunRPC]
    void RequestDrawEffect(int usingPlayerActorNumber)
    {
        ResultDrawEffect(usingPlayerActorNumber);
    }

    void ResultDrawEffect(int usingPlayerActorNumber)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == usingPlayerActorNumber)
            return;
        inGameUIController.DrawEffectOnUI();
    }
    #endregion
}