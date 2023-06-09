using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : PopUpUI
{
    protected override void Awake()
    {
        base.Awake();

        sliders["VolumeSlider"].onValueChanged.AddListener(OnVolumeSlider);
        sliders["BGMSlider"].onValueChanged.AddListener(OnBGMSlider);
        sliders["SFXSlider"].onValueChanged.AddListener(OnSESlider);

        buttons["DoneButton"].onClick.AddListener(OnDoneButton);
        buttons["ResetButton"].onClick.AddListener(OnResetButton);

        ResetOptions();
    }

    void OnVolumeSlider(float volume)
    {
        GameManager.Audio.MasterVolume = volume;
    }

    void OnBGMSlider(float volume)
    {
        GameManager.Audio.BGMVolume = volume;
    }

    void OnSESlider(float volume)
    {
        GameManager.Audio.SFXVolume = volume;
    }

    void OnDoneButton()
    {
        YesNoPopUpUI yesNoPopUpUI = GameManager.UI.ShowPopupUI<YesNoPopUpUI>("UI/YesNoUI");
        yesNoPopUpUI.SetText(0, "이렇게 설정하시겠습니까?");
        yesNoPopUpUI.SetText(1, "예");
        yesNoPopUpUI.SetText(2, "아니오");
        yesNoPopUpUI.YesEvent.RemoveAllListeners();
        yesNoPopUpUI.NoEvent.RemoveAllListeners();
        yesNoPopUpUI.YesEvent.AddListener(SetOptions);
    }

    void OnResetButton()
    {
        YesNoPopUpUI yesNoPopUpUI = GameManager.UI.ShowPopupUI<YesNoPopUpUI>("UI/YesNoUI");
        yesNoPopUpUI.SetText(0, "정말 초기화하시겠습니까?");
        yesNoPopUpUI.SetText(1, "예");
        yesNoPopUpUI.SetText(2, "아니오");
        yesNoPopUpUI.YesEvent.RemoveAllListeners();
        yesNoPopUpUI.NoEvent.RemoveAllListeners();
        yesNoPopUpUI.YesEvent.AddListener(ResetOptions);
    }

    void SetOptions()
    {
        CloseUI();
    }

    void ResetOptions()
    {
        sliders["VolumeSlider"].value = 0.5f;
        sliders["BGMSlider"].value = 0.5f;
        sliders["SFXSlider"].value = 0.5f;
    }
}
