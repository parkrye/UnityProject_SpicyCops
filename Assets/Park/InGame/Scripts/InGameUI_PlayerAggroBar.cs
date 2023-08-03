using UnityEngine;

public class InGameUI_PlayerAggroBar : SceneUI
{
    [SerializeField] float aggroValue;

    public override void Initialize()
    {

    }

    [ContextMenu("AggroUp")]
    void AggroUp()
    {
        aggroValue += 0.1f;
        if (aggroValue > 1f)
            aggroValue = 1f;

        sliders["PlayerAggroBar"].value = aggroValue;
    }

    [ContextMenu("AggroDown")]
    void AggroDown()
    {
        aggroValue -= 0.1f;
        if (aggroValue < 0f)
            aggroValue = 0f;

        sliders["PlayerAggroBar"].value = aggroValue;
    }
}
