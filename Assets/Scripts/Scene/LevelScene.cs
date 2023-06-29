using System.Collections;
using UnityEngine;

public class LevelScene : BaseScene
{
    [SerializeField] Transform startPosition;
    StartPositionGimic[] startPositions;
    [SerializeField] SummonPositionGimic summonPosition;
    [SerializeField] BossSummon bossSummonZone;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] ItemDropper itemDropper;
    [SerializeField] float spawnDelay, spawnDistance;
    [SerializeField] int enemyLimit;

    public enum LevelState { Search, Keep, ComeBack, Fight, Win }

    protected override IEnumerator LoadingRoutine()
    {
        // �÷��̾� ���� ����
        GameManager.Data.Player.controllable = false;
        yield return new WaitForEndOfFrame();

        // �÷��̾� ���� ��ġ ����
        startPositions = startPosition.GetComponentsInChildren<StartPositionGimic>();
        int spawnPosition = Random.Range(0, startPositions.Length);
        startPositions[spawnPosition].SetGimic();
        GameManager.Data.Player.transform.position = startPositions[spawnPosition].transform.position;
        Progress = 0.1f;

        // ������ ��ġ ����
        summonPosition.SetGimic();
        Progress = 0.2f;

        // UI ����
        GameManager.UI.CreateSceneCanvas();
        GameManager.UI.CreatePopupCanvas();
        SceneInfoUI infoUI = GameManager.UI.ShowSceneUI<SceneInfoUI>("UI/SceneInfoUI");
        infoUI.Initialize();
        GameManager.UI.ShowSceneUI<SceneUI>("UI/SceneItemUI").Initialize();
        GameManager.UI.ShowSceneUI<SceneUI>("UI/SceneKeyUI").Initialize();
        GameManager.UI.ShowSceneUI<SceneUI>("UI/SceneStatusUI").Initialize();
        infoUI.UpdateObjective(LevelState.Search);
        Progress = 0.4f;

        // ���ʹ� ���� ����
        if (spawnDelay == 0f)
            spawnDelay = 5f;
        if (spawnDistance == 0f)
            spawnDistance = 10f;
        enemySpawner.Initialize(spawnDelay, spawnDistance, enemyLimit);
        Progress = 0.6f;

        // �̺�Ʈ ����
        bossSummonZone.ObjectStateEvent.AddListener(infoUI.UpdateObjective);
        bossSummonZone.ObjectStateEvent.AddListener(enemySpawner.StopSpawn);
        bossSummonZone.ObjectStateEvent.AddListener(itemDropper.StopDrop);
        Progress = 0.8f;

        // �÷��̾� ����
        GetComponent<YLimiter>().Initialize();
        GameManager.Data.Player.controllable = true;
        GameManager.Data.RecordTime = true;
        Progress = 1f;
    }
}
