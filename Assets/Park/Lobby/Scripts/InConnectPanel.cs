using Photon.Pun;
using UnityEngine;
public class InConnectPanel : MonoBehaviour
{
	[SerializeField] GameObject optionPanel;

    void OnEnable()
    {
        optionPanel.SetActive(false);
    }

    public void OnLobbyButtonClicked()
	{
		PhotonNetwork.JoinLobby();
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
