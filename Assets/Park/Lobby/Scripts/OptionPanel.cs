using System.Collections;
using UnityEngine;

public class OptionPanel : SceneUI
{
    [SerializeField] float masterVolume, bgmVolume, sfxVolume;

    void OnEnable()
    {
        sliders["MasterVolumeSlider"].value = GameManager.Audio.MasterVolume;
        sliders["BGMVolumeSlider"].value = GameManager.Audio.BGMVolume;
        sliders["SFXVolumeSlider"].value = GameManager.Audio.SFXVolume;
    }

    public void OnMasterVolumeSliderChanged()
    {
        masterVolume = sliders["MasterVolumeSlider"].value;
    }

    public void OnBGMVolumeSliderChanged()
    {
        bgmVolume = sliders["BGMVolumeSlider"].value;
    }

    public void OnSFXolumeSliderChanged()
    {
        sfxVolume = sliders["SFXVolumeSlider"].value;
    }

    public void OnCancelButtonClicked()
    {
        sliders["MasterVolumeSlider"].value = GameManager.Audio.MasterVolume;
        sliders["BGMVolumeSlider"].value = GameManager.Audio.BGMVolume;
        sliders["SFXVolumeSlider"].value = GameManager.Audio.SFXVolume;
        
        gameObject.SetActive(false);
    }

    public void OnConfirmButtonClicked()
    {
        GameManager.Audio.MasterVolume = masterVolume;
        GameManager.Audio.BGMVolume = bgmVolume;
        GameManager.Audio.SFXVolume = sfxVolume;

        gameObject.SetActive(false);
    }
}
