using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] GameObject prevPanel;
    [SerializeField] TMP_Text avatarNameText;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] Button buyButton;
    [SerializeField] TMP_Text costText;

    [SerializeField] Camera avatarCamera;
    [SerializeField] int avatarNum;
    [SerializeField] GameObject avatarRoot;
    [SerializeField] Transform[] avatarList;

    void Awake()
    {
        SkinnedMeshRenderer[] tmpList = avatarRoot.GetComponentsInChildren<SkinnedMeshRenderer>();
        avatarList = new Transform[tmpList.Length];
        for (int i = 0; i < tmpList.Length; i++)
        {
            avatarList[i] = tmpList[i].transform.parent.transform;
        }
    }

    void OnEnable()
    {
        avatarNum = 0;
        GameData.userData.coin += 1000;
        moneyText.text = GameData.userData.coin.ToString();
        OnModifyAvatarChanged();
    }

    private void OnDisable()
    {
        avatarNum = 0;
        OnModifyAvatarChanged();
    }

    public void OnBuyButtonClicked()
    {
        if (GameData.userData.coin < GameData.AVATA_RPRICE)
            return;
        GameData.userData.coin -= GameData.AVATA_RPRICE;
        GameData.userData.avaters[GameData.AVATAR[avatarNum]] = true;
        moneyText.text = GameData.userData.coin.ToString();
        OnModifyAvatarChanged();
    }

    public void OnCancelButtonClicked()
    {
        gameObject.SetActive(false);
        prevPanel.gameObject.SetActive(true);
    }

    public void OnLeftButtonClicked()
    {
        avatarNum--;
        if(avatarNum < 0)
        {
            avatarNum = avatarList.Length - 1;
        }
        OnModifyAvatarChanged();
    }

    public void OnRightButtonClicked()
    {
        avatarNum++;
        if(avatarNum > avatarList.Length - 1)
        {
            avatarNum = 0;
        }
        OnModifyAvatarChanged();
    }

    void OnModifyAvatarChanged()
    {
        avatarCamera.transform.position = new Vector3(avatarNum * 5f, avatarCamera.transform.position.y, avatarCamera.transform.position.z);
        avatarNameText.text = GameData.AVATAR[avatarNum];
        if (GameData.userData.avaters[GameData.AVATAR[avatarNum]])
        {
            buyButton.interactable = false;
            costText.text = "-";
        }
        else
        {
            buyButton.interactable = true;
            costText.text = $"{GameData.AVATA_RPRICE} $";
        }
    }
}
