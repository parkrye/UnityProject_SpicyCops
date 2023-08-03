using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class InGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] float clock;
    public float Clock { get { return clock; } set {  clock = value; } }

    // 플레이어 ID:어그로값 딕셔너리
    // 에너미 스크립트?

    void Start()
    {
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
            StartCoroutine(ClockWorking());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.GetReady(targetPlayer)))
        {
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
        if (propertiesThatChanged.ContainsKey(CustomProperty.GetLoadTime(PhotonNetwork.CurrentRoom)))
        {
            StartCoroutine(GameStartTimer());
        }
    }

    IEnumerator GameStartTimer()
    {
        double loadTime = PhotonNetwork.CurrentRoom.GetLoadTime();
        while (loadTime  > PhotonNetwork.Time)
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Game Start!");
        GameStart();
    }

    void GameStart()
    {
        // 자기 캐릭터 생성
        // PhotonNetwork.Instantiate("Player", position, rotation, 0);
        // 플레이어 생성시 이 스크립트를 가져가도록
        // 이 스크립트에 플레이어 목록 저장
        // UI에 플레이어 정보 저장

        // 방장 작업
        if (PhotonNetwork.IsMasterClient)
        {
            // 에너미 생성
            // 에너미 생성시 이 스크립트 가져가도록
            StartCoroutine(ClockWorking());
        }
    }

    private void DebugGameStart()
    {
        // 자기 캐릭터 생성
        // PhotonNetwork.Instantiate("Player", position, rotation, 0);
        // 플레이어 생성시 이 스크립트를 가져가도록
        // 이 스크립트에 플레이어 목록 저장

        // 방장 작업
        if (PhotonNetwork.IsMasterClient)
        {
            // 에너미 생성
            // 에너미 생성시 이 스크립트 가져가도록
            StartCoroutine(ClockWorking());
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

    IEnumerator ClockWorking()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Clock += 0.1f;
        }
    }
}