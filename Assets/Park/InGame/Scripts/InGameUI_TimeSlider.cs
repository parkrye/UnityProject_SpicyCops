using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI_TimeSlider : SceneUI
{
    [SerializeField] InGameUIController inGameUIController;

    [SerializeField] float timeValue;

    public override void Initialize()
    {
        sliders["TimeSlider"].maxValue = inGameUIController.inGameManager.TotalTime;
        inGameUIController.inGameManager.AddTimeEventListener(TimeClock);
    }

    public void TimeClock(float time)
    {
        if (timeValue > inGameUIController.inGameManager.TotalTime)
            return;

        timeValue = inGameUIController.inGameManager.TotalTime - time;
        sliders["TimeSlider"].value = timeValue;

        if(timeValue <= 30f)
        {
            texts["TimeText"].text = ((int)timeValue).ToString();
        }
    }
}
