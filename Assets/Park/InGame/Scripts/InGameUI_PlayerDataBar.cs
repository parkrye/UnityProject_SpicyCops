using UnityEngine;

public class InGameUI_PlayerDataBar : SceneUI
{
    [SerializeField] GameObject deadImage;
    [SerializeField] float aggroValue;

    public override void Initialize()
    {
        base.Initialize();
        sliders["PlayerAggroBar"].maxValue = GameData.MAX_AGGRO;
    }

    public void ModifyAggroUI(float value)
    {
        aggroValue = value;
        if (aggroValue > GameData.MAX_AGGRO)
            aggroValue = GameData.MAX_AGGRO;
        if (aggroValue < 0f)
            aggroValue = 0f;
        sliders["PlayerAggroBar"].value = aggroValue;
    }

    public void CheckAlive(bool alive)
    {
        deadImage.SetActive(!alive);
    }
}
