using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider, bgmVolumeSlider, sfxVolumeSlider;

    void OnEnable()
    {
        // �������� ����� ������
        // �����̴� ���� ����� ������
    }

    public void OnMasterVolumeSliderChanged()
    {
        // ����� ���� ����������
    }

    public void OnBGMVolumeSliderChanged()
    {
        // ����� ���� ����������
    }

    public void OnSFXolumeSliderChanged()
    {
        // ����� ���� ����������
    }

    public void OnCancelButtonClicked()
    {
        // �������� ����� ������ �����ֱ�
        gameObject.SetActive(false);
    }

    public void OnConfirmButtonClicked()
    {
        // ����� ���� ����������
        gameObject.SetActive(false);
    }
}
