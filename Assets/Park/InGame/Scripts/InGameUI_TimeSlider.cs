using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI_TimeSlider : SceneUI
{
    [SerializeField] InGameUIController inGameUIController;

    [SerializeField] float timeValue;
    [SerializeField] float maxTime;

    public override void Initialize()
    {
        maxTime = inGameUIController.inGameManager.TotalTime;
        timeValue = maxTime;
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
