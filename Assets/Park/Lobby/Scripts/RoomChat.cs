using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class RoomChat : MonoBehaviour
{
    [SerializeField] PhotonView photonView;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform content;
    [SerializeField] TMP_Text chatPrefab;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] bool nowChatting;

    [System.Serializable]
    public class ChatMessage
    {
        public string sender = "";
        public string message = "";
    }

    readonly List<ChatMessage> chatMessages = new();

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        nowChatting = false;
        inputField.onSubmit.AddListener(SubmitChat);

        StartCoroutine(URoutine());
    }

    void OnEnable()
    {
        ClearChat();
    }

    IEnumerator URoutine()
    {
        while (true)
        {
            yield return null;

            if (!nowChatting)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    nowChatting = true;
                    inputField.ActivateInputField();
                }
            }
            else if (nowChatting)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    nowChatting = false;
                    inputField.DeactivateInputField();
                }
            }

            //Show messages
            for (int i = 0; i < chatMessages.Count; i++)
            {
                TMP_Text message = Instantiate(chatPrefab, content);
                message.text = chatMessages[i].sender + ": " + chatMessages[i].message;
                scrollRect.normalizedPosition = new Vector2(0f, 0f);
            }
            chatMessages.Clear();
        }
    }

    void SubmitChat(string text)
    {
        if (text.Replace(" ", "") != "")
        {
            //Send message
            photonView.RPC("SendChat", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer, inputField.text);
            inputField.text = "";
            nowChatting = false;
        }
    }

    [PunRPC]
    void SendChat(Player sender, string message)
    {
        ChatMessage m = new()
        {
            sender = sender.NickName,
            message = message
        };

        chatMessages.Insert(0, m);
    }

    public void ClearChat()
    {
        TMP_Text[] chatList = content.GetComponentsInChildren<TMP_Text>();
        for(int i = 0; i < chatList.Length; i++)
        {
            Destroy(chatList[i].gameObject);
        }
    }
}