using UnityEngine;

public class InGameUI_RadialAggro : SceneUI
{
    [SerializeField] float aggroValue;

    public override void Initialize()
    {
        base.Initialize();
        sliders["PlayerAggroBar"].maxValue = GameData.MAX_AGGRO;
    }

    public void ModifyAggro(float value)
    {
        aggroValue = value;
        if (aggroValue > GameData.MAX_AGGRO)
            aggroValue = GameData.MAX_AGGRO;
        if (aggroValue < 0f)
            aggroValue = 0f;
        sliders["PlayerAggroBar"].value = aggroValue;
    }
}
