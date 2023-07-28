using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider, bgmVolumeSlider, sfxVolumeSlider;

    void OnEnable()
    {
        // 볼륨값을 오디오 값으로
        // 슬라이더 값을 오디오 값으로
    }

    public void OnMasterVolumeSliderChanged()
    {
        // 오디오 값을 볼륨값으로
    }

    public void OnBGMVolumeSliderChanged()
    {
        // 오디오 값을 볼륨값으로
    }

    public void OnSFXolumeSliderChanged()
    {
        // 오디오 값을 볼륨값으로
    }

    public void OnCancelButtonClicked()
    {
        // 볼륨값을 오디오 값으로 돌려주기
        gameObject.SetActive(false);
    }

    public void OnConfirmButtonClicked()
    {
        // 오디오 값을 볼륨값으로
        gameObject.SetActive(false);
    }
}
