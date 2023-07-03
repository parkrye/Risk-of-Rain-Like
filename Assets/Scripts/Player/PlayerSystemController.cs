using System.Collections;
using UnityEngine;

public class PlayerSystemController : MonoBehaviour, IHitable, IDamagePublisher
{
    PlayerDataModel playerDataModel;

    void Awake()
    {
        playerDataModel = GetComponent<PlayerDataModel>();
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
            foreach (var hero in playerDataModel.heroList)
                hero.gameObject.SetActive(false);
            playerDataModel.heroNum = num;
            playerDataModel.heroList[num].gameObject.SetActive(true);
            playerDataModel.hero = playerDataModel.heroList[num];
            playerDataModel.animator = playerDataModel.hero.animator;
            playerDataModel.hero.playerDataModel = playerDataModel;
            playerDataModel.animator.SetInteger("Hero", num);
            playerDataModel.playerAction.AttackTransform = playerDataModel.hero.attackTransform;
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

    public void Hit(float damage, float Time)
    {
        if (!playerDataModel.dodgeDamage)
        {
            StartCoroutine(HitRoutine(damage, Time));
        }
    }

    public IEnumerator HitRoutine(float damage, float time)
    {
        float nowTime = 0f;
        do
        {
            float _damage = DamageOccurrence(damage);
            playerDataModel.NOWHP -= _damage;
            GameManager.Data.Records["Hit"] += _damage;
            nowTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        } while (nowTime < time);
    }

    public void LevelUp()
    {
        playerDataModel.MAXHP *= 1.1f;
        playerDataModel.NOWHP = playerDataModel.MAXHP;
        playerDataModel.AttackDamage *= 1.1f;

        ParticleSystem effect = GameManager.Resource.Instantiate(GameManager.Resource.Load<ParticleSystem>("Particle/LevelUp"), playerDataModel.playerTransform.position, Quaternion.identity, true);
        GameManager.Resource.Destroy(effect.gameObject, 0.2f);
    }

    public void Die()
    {
        StartCoroutine(DieRoutine());
    }

    public void AddDamageSubscriber(IDamageSubscriber _subscriber)
    {
        playerDataModel.damageSubscribers.Add(_subscriber);
    }

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

    public float DamageOccurrence(float _damage)
    {
        float damage = _damage;
        for (int i = 0; i < playerDataModel.damageSubscribers.Count; i++)
        {
            damage = playerDataModel.damageSubscribers[i].ModifiyDamage(damage);
        }
        return damage;
    }

    IEnumerator DieRoutine()
    {
        GameManager.Data.RecordTime = false;
        GameManager.UI.ShowPopupUI<PopUpUI>("UI/RecordUI");

        bool achive = false;

        yield return null;
        if (GameManager.Data.Records["Stage"] > GameManager.Data.Achievement["StageCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"�������� ��� ����!\n{GameManager.Data.Achievement["StageCount"]} => {(int)GameManager.Data.Records["Stage"]}");
            GameManager.Data.SetAchievement("StageCount", (int)GameManager.Data.Records["Stage"]);
            achive = true;
        }

        yield return null;
        if (GameManager.Data.Records["Time"] > GameManager.Data.Achievement["TimeCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"�����ð� ��� ����!\n{GameManager.Data.Achievement["TimeCount"]} => {(int)GameManager.Data.Records["Time"]}");
            GameManager.Data.SetAchievement("TimeCount", (int)GameManager.Data.Records["Time"]);
            achive = true;
        }

        yield return null;
        if (GameManager.Data.Records["Kill"] > GameManager.Data.Achievement["KillCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"óġ �� ��� ����!\n{GameManager.Data.Achievement["KillCount"]} => {(int)GameManager.Data.Records["Kill"]}");
            GameManager.Data.SetAchievement("KillCount", (int)GameManager.Data.Records["Kill"]);
            achive = true;
        }

        yield return null;
        if (GameManager.Data.Records["Damage"] > GameManager.Data.Achievement["DamageCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"���� ���ط� ��� ����!\n{GameManager.Data.Achievement["DamageCount"]} => {(int)GameManager.Data.Records["Damage"]}");
            GameManager.Data.SetAchievement("DamageCount", (int)GameManager.Data.Records["Damage"]);
            achive = true;
        }

        yield return null;
        if (GameManager.Data.Records["Hit"] > GameManager.Data.Achievement["HitCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"���� ���ط� ��� ����!\n{GameManager.Data.Achievement["HitCount"]} => {GameManager.Data.Records["Hit"]}");
            GameManager.Data.SetAchievement("HitCount", (int)GameManager.Data.Records["Hit"]);
            achive = true;
        }

        yield return null;
        if (GameManager.Data.Records["Heal"] > GameManager.Data.Achievement["HealCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"ȸ���� ��� ����!\n{GameManager.Data.Achievement["HealCount"]} => {(int)GameManager.Data.Records["Heal"]}");
            GameManager.Data.SetAchievement("HealCount", (int)GameManager.Data.Records["Heal"]);
            achive = true;
        }

        yield return null;
        if (GameManager.Data.Records["Money"] > GameManager.Data.Achievement["MoneyCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"ȹ�� ��ȭ ��� ����!\n{GameManager.Data.Achievement["MoneyCount"]} => {(int)GameManager.Data.Records["Money"]}");
            GameManager.Data.SetAchievement("MoneyCount", (int)GameManager.Data.Records["Money"]);
            achive = true;
        }

        yield return null;
        if (GameManager.Data.Records["Cost"] > GameManager.Data.Achievement["CostCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"��� ��ȭ ��� ����!\n{GameManager.Data.Achievement["CostCount"]} => {(int)GameManager.Data.Records["Cost"]}");
            GameManager.Data.SetAchievement("CostCount", (int)GameManager.Data.Records["Cost"]);
            achive = true;
        }

        yield return null;
        if (GameManager.Data.Player.LEVEL > GameManager.Data.Achievement["LevelCount"])
        {
            NotifyUI notifyUI = GameManager.UI.ShowPopupUI<NotifyUI>("UI/NotifyUI");
            notifyUI.SetText($"���� ��� ����!\n{GameManager.Data.Achievement["LevelCount"]} => {GameManager.Data.Player.LEVEL}");
            GameManager.Data.SetAchievement("LevelCount", GameManager.Data.Player.LEVEL);
            achive = true;
        }

        yield return null;
        if (achive)
            GameManager.Data.SaveAchiveMent();
    }
}
