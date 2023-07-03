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
    /// �ʱ� ���̵� ��ġ
    /// </summary>
    void SettingDifficulty()
    {
        switch(GameManager.Data.Records["Difficulty"])
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
    /// �ð� �帧
    /// </summary>
    public void UpdateTime()
    {
        int seconds = (int)GameManager.Data.Records["Time"];
        string minText = seconds / 60 < 10 ? "0" + (seconds / 60).ToString() : (seconds / 60).ToString();
        string secText = seconds % 60 < 10 ? "0" + (seconds % 60).ToString() : (seconds % 60).ToString();
        texts["TimeText"].text = $"{minText}:{secText}";
    }

    /// <summary>
    /// ��ǥ ���� �̺�Ʈ
    /// </summary>
    public void UpdateObjective(LevelScene.LevelState objectState)
    {
        switch (objectState)
        {
            case LevelScene.LevelState.Search:
                texts["ObjectiveText"].text = "��ǥ\n���Ҹ� ã�ƶ�";
                break;
            case LevelScene.LevelState.Keep:
                texts["ObjectiveText"].text = "��ǥ\n���Ҹ� ���Ѷ�";
                break;
            case LevelScene.LevelState.ComeBack:
                texts["ObjectiveText"].text = "��ǥ\n���ҷ� ���ư���";
                break;
            case LevelScene.LevelState.Fight:
                texts["ObjectiveText"].text = "��ǥ\n������ ��ƶ�";
                break;
            case LevelScene.LevelState.Win:
                texts["ObjectiveText"].text = "��ǥ\n�Ϸ�";
                break;
        }
    }

    public void UpdateCoinText(int coin)
    {
        texts["CoinText"].text = coin.ToString();
    }
}
