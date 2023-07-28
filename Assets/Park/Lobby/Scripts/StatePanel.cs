using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class StatePanel : MonoBehaviour
{
	public static StatePanel Instance { get; private set; }

	[SerializeField] RectTransform content;
	[SerializeField] TMP_Text textPrefab;
	[SerializeField] GameObject panel;
	[SerializeField] bool view;

	Photon.Realtime.ClientState state;

	void Awake()
	{
		Instance = this;
		view = false;
        panel.SetActive(view);
        StartCoroutine(StateRoutine());
        StartCoroutine(ViewRoutine());
	}

	IEnumerator StateRoutine()
	{
		while (true)
		{
			yield return null;

            if (state == PhotonNetwork.NetworkClientState)
                continue;

            state = PhotonNetwork.NetworkClientState;

            TMP_Text instance = Instantiate(textPrefab, content);
            instance.text = string.Format("[Photon NetworkState] {0} : {1}", System.DateTime.Now.ToString("HH:mm:ss.ff"), state.ToString());
            Debug.Log(string.Format("[Photon NetworkState] {0}", state.ToString()));
        }
	}

	IEnumerator ViewRoutine()
	{
		while (true)
		{
			yield return null;

            if (Input.GetKeyDown(KeyCode.F12))
			{
				view = !view;
                panel.SetActive(view);
            }
        }
	}

	public void AddMessage(string message)
	{
		TMP_Text instance = Instantiate(textPrefab, content);
		instance.text = string.Format("[Photon] {0} : {1}", System.DateTime.Now.ToString("HH:mm:ss.ff"), message);
		Debug.Log(string.Format("[Photon] {0}", message));
	}
}
