using System.Text;

public class SceneInfoUI : SceneUI
{
    public override void Initialize()
    {
        SettingDifficulty();

        GameManager.Data.TimeClock.RemoveAllListeners();
        GameManager.Data.Player.OnCoinEvent.RemoveAllListeners();
        GameManager.Data.TimeClock.AddListener(UpdateTime);
        GameManager.Data.Player.OnCoinEvent.AddListener(UpdateCoinText);
    }

    /// <summary>
    /// 초기 난이도 배치
    /// </summary>
    void SettingDifficulty()
    {
        switch(GameManager.Data.NowRecords["Difficulty"])
        {
            case 1f:
                images["DifficultyImage"].sprite = GameManager.Resource.Load<Icon>("Icon/EasyModeIcon").sprite;
                break;
            case 2f:
                images["DifficultyImage"].sprite = GameManager.Resource.Load<Icon>("Icon/NormalModeIcon").sprite;
                break;
            case 3f:
                images["DifficultyImage"].sprite = GameManager.Resource.Load<Icon>("Icon/HardModeIcon").sprite;
                break;
        }
    }

    /// <summary>
    /// 시간 흐름
    /// </summary>
    public void UpdateTime()
    {
        StringBuilder time = new(), sec = new();
        if (GameManager.Data.Time[0] < 10)
            time.Append("0");
        time.Append(GameManager.Data.Time[0].ToString());
        time.Append(":");
        if (GameManager.Data.Time[1] < 10)
            time.Append("0");
        time.Append(GameManager.Data.Time[1].ToString());
        texts["TimeText"].text = time.ToString();
    }

    /// <summary>
    /// 목표 관련 이벤트
    /// </summary>
    public void UpdateObjective(LevelScene.LevelState objectState)
    {
        switch (objectState)
        {
            case LevelScene.LevelState.Search:
                texts["ObjectiveText"].text = "목표\n성소를 찾아라";
                break;
            case LevelScene.LevelState.Keep:
                texts["ObjectiveText"].text = "목표\n성소를 지켜라";
                break;
            case LevelScene.LevelState.ComeBack:
                texts["ObjectiveText"].text = "목표\n성소로 돌아가라";
                break;
            case LevelScene.LevelState.Fight:
                texts["ObjectiveText"].text = "목표\n보스를 잡아라";
                break;
            case LevelScene.LevelState.Win:
                texts["ObjectiveText"].text = "목표\n완료";
                break;
        }
    }

    public void UpdateCharge(int percentage)
    {
        texts["ObjectiveText"].text = $"목표\n성소를 지켜라\n({percentage}%)";
    }

    public void UpdateCoinText(int coin)
    {
        texts["CoinText"].text = coin.ToString();
    }
}
