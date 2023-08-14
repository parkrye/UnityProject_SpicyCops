using UnityEngine;

public class OptionPanel : SceneUI
{
    [SerializeField] float prevMasterVolume, prevBgmVolume, prevSfxVolume;
    [SerializeField] AudioSource sfxTestSound;

    void OnEnable()
    {
        prevMasterVolume = GameManager.Audio.MasterVolume * 0.025f;
        prevBgmVolume = GameManager.Audio.BGMVolume * 0.025f;
        prevSfxVolume = GameManager.Audio.SFXVolume * 0.025f;
    }

    public void OnMasterVolumeSliderChanged()
    {
        GameManager.Audio.MasterVolume = sliders["MasterVolumeSlider"].value;
    }

    public void OnBGMVolumeSliderChanged()
    {
        GameManager.Audio.BGMVolume = sliders["BGMVolumeSlider"].value;
    }

    public void OnSFXolumeSliderChanged()
    {
        GameManager.Audio.SFXVolume = sliders["SFXVolumeSlider"].value;
        sfxTestSound.Play();
    }

    public void OnCancelButtonClicked()
    {
        GameManager.Audio.MasterVolume = prevMasterVolume;
        GameManager.Audio.BGMVolume = prevBgmVolume;
        GameManager.Audio.SFXVolume = prevSfxVolume;

        sliders["MasterVolumeSlider"].value = prevMasterVolume;
        sliders["BGMVolumeSlider"].value = prevBgmVolume;
        sliders["SFXVolumeSlider"].value = prevSfxVolume;

        gameObject.SetActive(false);
    }

    public void OnConfirmButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
