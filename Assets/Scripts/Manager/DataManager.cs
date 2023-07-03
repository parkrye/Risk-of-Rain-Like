using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    bool recordTime;
    Dictionary<string, float> records;

    public UnityEvent TimeClock;
    public bool RecordTime { get { return recordTime; } set { recordTime = value; } }
    public Dictionary<string, float> Records { get { return records; } set { records = value; } }
    public Dictionary<string, int> Achievement { get; private set; }

    public PlayerDataModel Player { get; set; }

    void Update()
    {
        if (RecordTime)
        {
            Records["Time"] += Time.deltaTime;
            TimeClock?.Invoke();
        }
    }

    public void Initialize()
    {
        Cursor.lockState = CursorLockMode.None;
        Records = new Dictionary<string, float>
        {
            { "Stage", 1f },
            { "Time", 0f },
            { "Difficulty", 1f },
            { "Kill", 0f },
            { "Damage", 0f },
            { "Heal", 0f },
            { "Hit", 0f },
            { "Money", 0f },
            { "Cost", 0f },
        };

        Achievement = CSVRW.ReadCSV_Achievements();

        if (Player != null)
        {
            Player.playerSystem.DestroyCharacter();
            Player = null;
        }
    }

    /// <summary>
    /// Achivement�� Ư�� ���� �����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="index">Achivement�� Ű</param>
    /// <param name="value">Achivement�� ������ ��</param>
    public void SetAchievement(string index, int value)
    {
        Achievement[index] = value;
    }

    /// <summary>
    /// Achivement�� �����ϴ� �޼ҵ�
    /// </summary>
    public void SaveAchiveMent()
    {
        CSVRW.WriteCSV_Achievements(Achievement);
    }

    /// <summary>
    /// Achivement�� �ʱ�ȭ�ϴ� �޼ҵ�
    /// </summary>
    public void ResetAchivement()
    {
        Achievement["StageCount"] = 0;
        Achievement["TimeCount"] = 0;
        Achievement["KillCount"] = 0;
        Achievement["DamageCount"] = 0;
        Achievement["HitCount"] = 0;
        Achievement["HealCount"] = 0;
        Achievement["MoneyCount"] = 0;
        Achievement["CostCount"] = 0;
        Achievement["LevelCount"] = 0;
        CSVRW.WriteCSV_Achievements(Achievement);
    }
}
