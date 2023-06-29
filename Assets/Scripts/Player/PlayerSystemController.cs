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

    public void Die()
    {
        Debug.Log("you died");
        GameManager.Data.RecordTime = false;
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
}
