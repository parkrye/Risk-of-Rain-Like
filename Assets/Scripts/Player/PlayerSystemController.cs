using System.Collections;
using UnityEngine;

/// <summary>
/// �÷��̾�� ���õ� ���� �۾��� ���� ��ũ��Ʈ
/// </summary>
public class PlayerSystemController : MonoBehaviour, IHitable, IDamagePublisher
{
    PlayerDataModel playerDataModel;    // �÷��̾� ������ ��
    ParticleSystem levelupParticle;     // ������ ȿ��

    void Awake()
    {
        playerDataModel = GetComponent<PlayerDataModel>();
        levelupParticle = GameManager.Resource.Load<ParticleSystem>("Particle/LevelUp");
    }

    /// <summary>
    /// ĳ���� ����
    /// </summary>
    /// <param name="num"></param>
    /// <returns>���� ���� ����</returns>
    public bool SelectHero(int num)
    {
        if (num >= 0 && num < playerDataModel.heroList.Count)
        {
            for(int i = 0; i < playerDataModel.heroList.Count; i++)
                playerDataModel.heroList[i].gameObject.SetActive(false);    // ��� ���� ��Ȱ��ȭ
            playerDataModel.heroNum = num;                                  // ���� ��ȣ ���
            playerDataModel.heroList[num].gameObject.SetActive(true);       // ���õ� ���� Ȱ��ȭ
            playerDataModel.hero = playerDataModel.heroList[num];           // ���õ� ���� ����
            playerDataModel.animator = playerDataModel.hero.animator;       // ���õ� ������ �ִϸ����� ����
            playerDataModel.hero.playerDataModel = playerDataModel;         // ���õ� ������ ������ �� ����
            playerDataModel.playerAction.AttackTransform = playerDataModel.hero.attackTransform;
                                                                            // ���õ� ������ ���� ������ ����
            return true;
        }
        return false;
    }

    /// <summary>
    /// ĳ���� �ı�
    /// </summary>
    public void DestroyCharacter()
    {
        GameManager.Resource.Destroy(gameObject);
    }

    /// <summary>
    /// ���� ���� ��, ��ҽ� ���� ���� ���� �Է�
    /// 0: �̵��ӵ�, 1: ��������, 2: ���ݷ�, 3: ġ��Ÿ Ȯ��, 4: ġ��Ÿ ����
    /// </summary>
    public void Buff(int num, float value)
    {
        playerDataModel.buffModifier[num] *= value;
    }

    /// <summary>
    /// �ǰ� �޼ҵ�
    /// </summary>
    /// <param name="damage">�����</param>
    /// <param name="Time">���� �ð�</param>
    public void Hit(float damage, float Time = 0f)
    {
        if (!playerDataModel.dodgeDamage)
            StartCoroutine(HitRoutine(damage, Time));
    }

    /// <summary>
    /// ����� �߻� ������
    /// </summary>
    /// <param name="damage">�����</param>
    /// <param name="time">���� �ð�</param>
    /// <returns>0.1�ʸ��� �߻�</returns>
    public IEnumerator HitRoutine(float damage, float time)
    {
        float nowTime = 0f;
        while (nowTime <= time)
        {
            float _damage = DamageOccurrence(damage);       // ����� �߻� �̺�Ʈ�� ���ҵ� ������� ���Ͽ�
            playerDataModel.NOWHP -= _damage;               // ���� ü���� ���̰�
            GameManager.Data.NowRecords["Hit"] += _damage;  // ���� �ǰݷ��� �����ϰ�
            nowTime += 0.1f;                                // �� �ð��� ����
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// ������ �޼ҵ�
    /// </summary>
    public void LevelUp()
    {
        playerDataModel.MAXHP *= 1.1f;                  // �ִ� ü���� 10% ����
        playerDataModel.NOWHP = playerDataModel.MAXHP;  // ���� ü���� �ִ� ü������
        playerDataModel.AttackDamage *= 1.1f;           // ���� �������� 10% ����

        GameManager.Resource.Instantiate(levelupParticle, playerDataModel.playerTransform.position, Quaternion.identity, transform, true);
                                                        // ������ ��ƼŬ �߻�
    }

    /// <summary>
    /// ��� �޼ҵ�
    /// </summary>
    public void Die()
    {
        StartCoroutine(DieRoutine());
    }

    /// <summary>
    /// ������� ���� ������ �߰� �޼ҵ�
    /// </summary>
    /// <param name="_subscriber">�߰��� ������</param>
    public void AddDamageSubscriber(IDamageSubscriber _subscriber)
    {
        playerDataModel.damageSubscribers.Add(_subscriber);
    }

    /// <summary>
    /// ������� ���� ������ ��� �޼ҵ�
    /// </summary>
    /// <param name="_subscriber">����� ������</param>
    public void RemoveDamageSubscriber(IDamageSubscriber _subscriber)
    {
        for (int i = playerDataModel.damageSubscribers.Count - 1; i >= 0; i--)
        {
            if (playerDataModel.damageSubscribers[i].Equals(_subscriber))
            {
                playerDataModel.damageSubscribers.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// ����� �߻��� ���� �����ڵ��� ȿ�� ����
    /// </summary>
    /// <param name="_damage">�߻��� �����</param>
    /// <returns>������ �����</returns>
    public float DamageOccurrence(float _damage)
    {
        float damage = _damage;
        for (int i = 0; i < playerDataModel.damageSubscribers.Count; i++)
        {
            damage = playerDataModel.damageSubscribers[i].ModifiyDamage(damage);    // �� �������� ����� ��ȯ �Լ��� �����Ͽ� ����� ��ȯ
        }
        return damage;
    }

    /// <summary>
    /// ����� ������ ���� �� UI ���� ������
    /// </summary>
    /// <returns></returns>
    IEnumerator DieRoutine()
    {
        GameManager.Data.RecordTime = false;                    // �ð� ��� ����
        GameManager.UI.ShowPopupUI<PopUpUI>("UI/RecordUI");     // ��� UI ����

        bool achive = false;

        // ���� �� ���, ������ ���� ���� �� UI ����

        // ��������
        yield return null;
        GameManager.Data.AddAchives("StageCount", (int)GameManager.Data.NowRecords["Stage"]);
        if (GameManager.Data.NowRecords["Stage"] > GameManager.Data.Records["StageCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"Ž�� ��� ����!\n{GameManager.Data.Records["StageCount"]} => {(int)GameManager.Data.NowRecords["Stage"]}");
            GameManager.Data.SetRecords("StageCount", (int)GameManager.Data.NowRecords["Stage"]);
            achive = true;
        }

        // �ð�
        yield return null;
        GameManager.Data.AddAchives("TimeCount", (int)GameManager.Data.NowRecords["Time"]);
        if (GameManager.Data.NowRecords["Time"] > GameManager.Data.Records["TimeCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"�����ð� ��� ����!\n{GameManager.Data.Records["TimeCount"]} => {(int)GameManager.Data.NowRecords["Time"]}");
            GameManager.Data.SetRecords("TimeCount", (int)GameManager.Data.NowRecords["Time"]);
            achive = true;
        }
        
        // óġ ��
        yield return null;
        GameManager.Data.AddAchives("KillCount", (int)GameManager.Data.NowRecords["Kill"]);
        if (GameManager.Data.NowRecords["Kill"] > GameManager.Data.Records["KillCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"óġ �� ��� ����!\n{GameManager.Data.Records["KillCount"]} => {(int)GameManager.Data.NowRecords["Kill"]}");
            GameManager.Data.SetRecords("KillCount", (int)GameManager.Data.NowRecords["Kill"]);
            achive = true;
        }

        // ���� ���ط�
        yield return null;
        GameManager.Data.AddAchives("DamageCount", (int)GameManager.Data.NowRecords["Damage"]);
        if (GameManager.Data.NowRecords["Damage"] > GameManager.Data.Records["DamageCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"���� ���ط� ��� ����!\n{GameManager.Data.Records["DamageCount"]} => {(int)GameManager.Data.NowRecords["Damage"]}");
            GameManager.Data.SetRecords("DamageCount", (int)GameManager.Data.NowRecords["Damage"]);
            achive = true;
        }

        // ���� ���ط�
        yield return null;
        GameManager.Data.AddAchives("HitCount", (int)GameManager.Data.NowRecords["Hit"]);
        if (GameManager.Data.NowRecords["Hit"] > GameManager.Data.Records["HitCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"���� ���ط� ��� ����!\n{GameManager.Data.Records["HitCount"]} => {GameManager.Data.NowRecords["Hit"]}");
            GameManager.Data.SetRecords("HitCount", (int)GameManager.Data.NowRecords["Hit"]);
            achive = true;
        }

        // ȸ����
        yield return null;
        GameManager.Data.AddAchives("HealCount", (int)GameManager.Data.NowRecords["Heal"]);
        if (GameManager.Data.NowRecords["Heal"] > GameManager.Data.Records["HealCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"ȸ���� ��� ����!\n{GameManager.Data.Records["HealCount"]} => {(int)GameManager.Data.NowRecords["Heal"]}");
            GameManager.Data.SetRecords("HealCount", (int)GameManager.Data.NowRecords["Heal"]);
            achive = true;
        }

        // ȹ�� ��ȭ
        yield return null;
        GameManager.Data.AddAchives("MoneyCount", (int)GameManager.Data.NowRecords["Money"]);
        if (GameManager.Data.NowRecords["Money"] > GameManager.Data.Records["MoneyCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"ȹ�� ��ȭ ��� ����!\n{GameManager.Data.Records["MoneyCount"]} => {(int)GameManager.Data.NowRecords["Money"]}");
            GameManager.Data.SetRecords("MoneyCount", (int)GameManager.Data.NowRecords["Money"]);
            achive = true;
        }

        // �Ҹ� ��ȭ
        yield return null;
        GameManager.Data.AddAchives("CostCount", (int)GameManager.Data.NowRecords["Cost"]);
        if (GameManager.Data.NowRecords["Cost"] > GameManager.Data.Records["CostCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"��� ��ȭ ��� ����!\n{GameManager.Data.Records["CostCount"]} => {(int)GameManager.Data.NowRecords["Cost"]}");
            GameManager.Data.SetRecords("CostCount", (int)GameManager.Data.NowRecords["Cost"]);
            achive = true;
        }

        // ����
        yield return null;
        GameManager.Data.AddAchives("LevelCount", (int)GameManager.Data.Player.LEVEL);
        if (GameManager.Data.Player.LEVEL > GameManager.Data.Records["LevelCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"���� ��� ����!\n{GameManager.Data.Records["LevelCount"]} => {GameManager.Data.Player.LEVEL}");
            GameManager.Data.SetRecords("LevelCount", GameManager.Data.Player.LEVEL);
            achive = true;
        }

        // ������ �����ϰ� ��� ���Ž� ��� ����
        yield return null;
        GameManager.Data.SaveAchivements();
        if (achive)
            GameManager.Data.SaveRecords();
    }
}
