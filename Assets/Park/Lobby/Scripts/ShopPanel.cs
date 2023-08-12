using UnityEngine;

public class ShopPanel : SceneUI
{
    [SerializeField] GameObject prevPanel;

    [SerializeField] Camera avatarCamera;
    [SerializeField] int avatarNum;
    [SerializeField] GameObject avatarRoot;
    [SerializeField] Transform[] avatarList;

    protected override void Awake()
    {
        base.Awake();
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
        texts["MoneyText"].text = GameData.userData.coin.ToString();
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
        texts["MoneyText"].text = GameData.userData.coin.ToString();
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
        texts["AvatarDescText"].text = GameData.AVATAR[avatarNum];
        if (GameData.userData.avaters[GameData.AVATAR[avatarNum]])
        {
            buttons["BuyButton"].interactable = false;
            texts["CostText"].text = "-";
            avatarList[avatarNum].localEulerAngles = Vector3.zero;
        }
        else
        {
            buttons["BuyButton"].interactable = true;
            texts["CostText"].text = $"{GameData.AVATA_RPRICE} $";
        }
    }
}
