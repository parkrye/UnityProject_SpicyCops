using Photon.Pun;
using System.Collections;
using UnityEngine;

public class InGameUI_TimeSlider : SceneUI
{
    [SerializeField] InGameUIController inGameUIController;

    [SerializeField] float timeValue;
    [SerializeField] AudioSource timerTickSound;
    [SerializeField] bool tickStarted;

    public void Initialize(float serverTime)
    {
        base.Initialize();
        sliders["TimeSlider"].maxValue = inGameUIController.inGameManager.TotalTime;
        timeValue = inGameUIController.inGameManager.TotalTime - ((float)PhotonNetwork.Time - serverTime);
        //Debug.Log($"Timer : {timeValue}");
        tickStarted = false;
        StartCoroutine(TimeClockRoutine());
    }

    IEnumerator TimeClockRoutine()
    {
        while(timeValue > 0f)
        {
            timeValue -= 0.1f;
            sliders["TimeSlider"].value = timeValue;

            if (timeValue <= 30f)
            {
                if (!tickStarted)
                {
                    tickStarted = true;
                    StartCoroutine(TickRoutine());
                }
                texts["TimeText"].text = ((int)timeValue).ToString();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator TickRoutine()
    {
        while (timeValue > 0f)
        {
            timerTickSound.Play();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
