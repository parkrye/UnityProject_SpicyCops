using Cinemachine;
using Jeon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    void Start()
    {
        playerDictionary = new Dictionary<int, float>();
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
        // 방장 작업 대신 수행
        if (newMasterClient.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            StartCoroutine(ClockWorkRoutine());
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
        photonView.RPC("RequestAddPlayer", RpcTarget.AllBufferedViaServer, player.GetComponent<PhotonView>().ViewID);
        inGameUIController.SetPlayerPhotonView(player.GetComponent<PhotonView>());
        playerCamera.Follow = player.transform;

        // 방장 작업
        if (PhotonNetwork.IsMasterClient)
        {
            // 에너미가 이 스크립트 참조
            StartCoroutine(ClockWorkRoutine());
            safeArea.GameStartSetting();
        }
    }

    private void DebugGameStart()
    {
        // 캐릭터 생성
        // UI에 플레이어 정보 저장
        GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
        photonView.RPC("RequestAddPlayer", RpcTarget.AllBufferedViaServer, player);
        inGameUIController.SetPlayerPhotonView(player.GetComponent<PhotonView>());
        playerCamera.Follow = player.transform;

        // 방장 작업
        if (PhotonNetwork.IsMasterClient)
        {
            // 에너미가 this 참조
            StartCoroutine(ClockWorkRoutine());
            safeArea.GameStartSetting();
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

    IEnumerator ClockWorkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            photonView.RPC("RequestClockWork", RpcTarget.AllViaServer, 0.1f);
        }
    }

    // rpc
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
        // 플레이어가 this 참조
    }

    [PunRPC]
    void RequestClockWork(float time, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        ResultClockWork(time + lag);
    }

    public void ResultClockWork(float time)
    {
        NowTime += time;
    }
}