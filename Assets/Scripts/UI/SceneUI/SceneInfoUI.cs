using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfoUI : SceneUI
{
    public enum ObjectState { Search, Keep, ComeBack, Fight, Win }

    public override void Initialize()
    {
        SettingDifficulty();
        GameManager.Data.TimeClock.AddListener(UpdateTime);
        GameManager.Data.Player.OnCoinEvent.AddListener(UpdateCoinText);
    }

    /// <summary>
    /// �ʱ� ���̵� ��ġ
    /// </summary>
    void SettingDifficulty()
    {
        switch(GameManager.Data.Difficulty)
        {
            case 1:
                images["DifficultyImage"].sprite = GameManager.Resource.Load<Icon>("Icon/EasyModeIcon").sprite;
                break;
            case 2:
                images["DifficultyImage"].sprite = GameManager.Resource.Load<Icon>("Icon/NormalModeIcon").sprite;
                break;
            case 3:
                images["DifficultyImage"].sprite = GameManager.Resource.Load<Icon>("Icon/HardModeIcon").sprite;
                break;
        }
    }

    /// <summary>
    /// �ð� �帧
    /// </summary>
    public void UpdateTime()
    {
        int seconds = (int)GameManager.Data.Time;
        string minText = seconds / 60 < 10 ? "0" + (seconds / 60).ToString() : (seconds / 60).ToString();
        string secText = seconds % 60 < 10 ? "0" + (seconds % 60).ToString() : (seconds % 60).ToString();
        texts["TimeText"].text = $"{minText}:{secText}";
    }

    /// <summary>
    /// ��ǥ ���� �̺�Ʈ
    /// </summary>
    public void UpdateObjective(ObjectState objectState)
    {
        switch (objectState)
        {
            case ObjectState.Search:
                texts["ObjectiveText"].text = "Objective\nFind Zone";
                break;
            case ObjectState.Keep:
                texts["ObjectiveText"].text = "Objective\nKeep Zone";
                break;
            case ObjectState.ComeBack:
                texts["ObjectiveText"].text = "Objective\nBack to Zone";
                break;
            case ObjectState.Fight:
                texts["ObjectiveText"].text = "Objective\nKill Boss";
                break;
            case ObjectState.Win:
                texts["ObjectiveText"].text = "Objective\nComplete";
                break;
        }
    }

    public void UpdateCoinText(int coin)
    {
        texts["CoinText"].text = coin.ToString();
    }
}
