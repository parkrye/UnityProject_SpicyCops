using UnityEngine;
using UnityEngine.UI;

public class InGameUI_PlayerAggroBar : MonoBehaviour
{
    [SerializeField] Slider aggroSlider;

    [SerializeField] float aggroValue;

    [ContextMenu("AggroUp")]
    void AggroUp()
    {
        aggroValue += 0.1f;
        if (aggroValue > 1f)
            aggroValue = 1f;

        aggroSlider.value = aggroValue;
    }

    [ContextMenu("AggroDown")]
    void AggroDown()
    {
        aggroValue -= 0.1f;
        if (aggroValue < 0f)
            aggroValue = 0f;

        aggroSlider.value = aggroValue;
    }
}
