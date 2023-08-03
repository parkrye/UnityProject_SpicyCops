using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI_TimeSlider : MonoBehaviour
{
    [SerializeField] float timeValue;
    [SerializeField] float maxTime;

    Slider timeSlider;
    [SerializeField] TMP_Text timeText;

    void Awake()
    {
        timeSlider = GetComponent<Slider>();
        timeSlider.maxValue = maxTime;
    }

    [ContextMenu ("TimeClock")]
    public void TimeClock()
    {
        timeValue += 1f;
        timeSlider.value = timeValue;

        if(maxTime - timeValue <= 30f)
        {
            timeText.text = (maxTime - timeValue).ToString();
        }
    }
}
