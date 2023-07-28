using TMPro;
using UnityEngine;

public class EnterPrivateRoomPanel : MonoBehaviour
{
    [SerializeField] public TMP_InputField passwordInputField;
    [SerializeField] RoomEntry roomEntry;

    public void Initialize(RoomEntry _roomEntry)
    {
        roomEntry = _roomEntry;
    }

    public void OnCancelButtonClicked()
    {
        passwordInputField.text = "";
        gameObject.SetActive(false);
    }

    public void OnConfirmButtonClicked()
    {
        if (roomEntry.EnterPrivateRoom())
        {
            gameObject.SetActive(false);
        }
        else
        {
            passwordInputField.text = "";
        }
    }
}
