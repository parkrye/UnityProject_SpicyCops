using Photon.Pun;
using UnityEngine;
public class InConnectPanel : MonoBehaviour
{
	[SerializeField] GameObject optionPanel;
    [SerializeField] GameObject lobbyPanel_Rooms;

    void OnEnable()
    {
        optionPanel.SetActive(false);
    }

    public void OnLobbyButtonClicked()
    {
        gameObject.SetActive(false);
        lobbyPanel_Rooms.SetActive(true);
    }

    public void OnOptionButtonClicked()
    {
        optionPanel.SetActive(true);
    }

    public void OnLogoutButtonClicked()
	{
		PhotonNetwork.Disconnect();
	}

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
