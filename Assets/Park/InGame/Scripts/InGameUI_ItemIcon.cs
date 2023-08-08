using UnityEngine;
using UnityEngine.UI;

public class InGameUI_ItemIcon : MonoBehaviour
{
    [SerializeField] Image itemImage;

    public void SettingIconImage(Sprite sprite = null)
    {
        itemImage.sprite = sprite;
    }
}
