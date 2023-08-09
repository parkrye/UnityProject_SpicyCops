using UnityEngine;

public class InGameUI_RadialAggro : SceneUI
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public void ModifyAggro(float value)
    {
        images["AggroImage"].fillAmount = value / GameData.MAX_AGGRO;
    }
}
