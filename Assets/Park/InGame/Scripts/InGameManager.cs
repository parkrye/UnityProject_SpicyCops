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

    // �÷��̾� ID:��׷ΰ� ��ųʸ�
    // ���ʹ� ��ũ��Ʈ?

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
        // ���� �۾� ��� ����
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
        // �ڱ� ĳ���� ����
        // PhotonNetwork.Instantiate("Player", position, rotation, 0);
        // �÷��̾� ������ �� ��ũ��Ʈ�� ����������
        // �� ��ũ��Ʈ�� �÷��̾� ��� ����
        // UI�� �÷��̾� ���� ����

        // ���� �۾�
        if (PhotonNetwork.IsMasterClient)
        {
            // ���ʹ� ����
            // ���ʹ� ������ �� ��ũ��Ʈ ����������
            StartCoroutine(ClockWorking());
        }
    }

    private void DebugGameStart()
    {
        // �ڱ� ĳ���� ����
        // PhotonNetwork.Instantiate("Player", position, rotation, 0);
        // �÷��̾� ������ �� ��ũ��Ʈ�� ����������
        // �� ��ũ��Ʈ�� �÷��̾� ��� ����

        // ���� �۾�
        if (PhotonNetwork.IsMasterClient)
        {
            // ���ʹ� ����
            // ���ʹ� ������ �� ��ũ��Ʈ ����������
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