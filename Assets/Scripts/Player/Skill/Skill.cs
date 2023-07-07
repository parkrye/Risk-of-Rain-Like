using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��ų �߻� ��ũ��Ʈ. ScriptableObject
/// </summary>
public abstract class Skill : ScriptableObject
{
    public Hero hero;                   // �� ��ų�� ������ ����
    public string SkillName;            // ��ų�� �̸�
    public string SkillDesc;            // ��ų�� ����
    public Sprite SkillIcon;            // ��ų�� ������

    public float coolTime, modifier;    // ��ų ��Ÿ��, ��ų ����ġ
    bool coolCheck;                     // ��Ÿ�� üũ
    protected string[] actionKeys = {"Action1A", "Action2A", "Action3A", "Action4A",
                                    "Action1B", "Action2B", "Action3B", "Action4B",
                                    "Action1C", "Action2C", "Action3C", "Action4C"};
                                        // �ִϸ��̼� Ʈ���� Ű
    [SerializeField] protected int actionNum;
                                        // �ִϸ��̼� Ʈ���� Ű ��ȣ

    /// <summary>
    /// ��Ÿ�� ������Ƽ
    /// </summary>
    public bool CoolCheck
    {
        get { return coolCheck; }           // ��Ÿ�� ��ȯ
        set
        {
            coolCheck = value;              // ��Ÿ�� üũ
            CoolEvent?.Invoke(CoolCheck);   // ��Ÿ�� �̺�Ʈ �ߵ�
        }
    }
    public UnityEvent<bool> CoolEvent;      // ��Ÿ�� �̺�Ʈ

    private void OnEnable()
    {
        CoolCheck = true;
    }

    /// <summary>
    /// ��ų ��� �޼ҵ�
    /// </summary>
    /// <param name="isPressed">��ư Ŭ�� ����</param>
    /// <param name="param">�߰������� ���Ǵ� ������</param>
    /// <returns>��ų ���� ����</returns>
    public abstract bool Active(bool isPressed, params float[] param);

    /// <summary>
    /// ��Ÿ�� ���� ������
    /// </summary>
    /// <param name="coolModifier">��Ÿ�� ������</param>
    /// <returns></returns>
    public IEnumerator CoolTime(float coolModifier)
    {
        while (CoolCheck)       // ��Ÿ���� false�� �ɶ����� �ϴ� ���
            yield return null;
        yield return new WaitForSeconds(coolTime * coolModifier * hero.playerDataModel.ReverseTimeScale);
                                // ��ų ��Ÿ�� * ��Ÿ�� ������ ( * �ð� ����� ��� 2��� ���� ���� ���ƾ��ϹǷ� ����)
        CoolCheck = true;       // ��Ÿ�� true
    }
}
