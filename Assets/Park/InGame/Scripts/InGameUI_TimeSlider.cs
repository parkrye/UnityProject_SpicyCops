using UnityEngine;

public class InGameUI_TimeSlider : SceneUI
{
    [SerializeField] float timeValue;
    [SerializeField] float maxTime;

    public override void Initialize()
    {
        sliders["TimeSlider"].maxValue = maxTime;
    }

    [ContextMenu ("TimeClock")]
    public void TimeClock()
    {
        if (timeValue > maxTime)
            return;

        timeValue -= 1f;
        sliders["TimeSlider"].value = timeValue;

        if(timeValue <= 30f)
        {
            texts["TimeText"].text = timeValue.ToString();
        }
    }
}
