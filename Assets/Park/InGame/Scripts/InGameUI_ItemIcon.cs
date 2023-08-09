using UnityEngine;
using UnityEngine.UI;

public class InGameUI_ItemIcon : MonoBehaviour
{
    [SerializeField] Image itemImage;

    public void SettingIconImage(Sprite sprite = null)
    {
        if (sprite != null)
        {
            itemImage.sprite = sprite;
            itemImage.color = new Color(1, 1, 1, 1);
        }
        else
            itemImage.color = new Color(0, 0, 0, 0);
    }
}
