using UnityEngine;
using System.Collections;

/// <summary>
/// �ܹ� ���
/// </summary>
[CreateAssetMenu(fileName = "Archer_Action1A", menuName = "Data/Skill/Archer/Action1A")]
public class Archer_Action1A : Skill, ICriticable
{
    [SerializeField] Arrow arrow;   // ȭ��

    public override bool Active(bool isPressed, params float[] param)
    {
        if (isPressed)  // ��ư�� ���� ������
        {
            hero.playerDataModel.animator.SetTrigger(actionKeys[actionNum]);        // �ִϸ����� ����
            hero.attackSource.Play();                                               // �Ҹ� ���

            Arrow arrowAttack = GameManager.Resource.Instantiate(arrow, true);      // ȭ���� �����ϰ�
            arrowAttack.transform.position = hero.playerDataModel.playerAction.AttackTransform.position;
                                                                                    // ȭ���� ���� ��ġ�� �ű��
            arrowAttack.Shot(hero.playerDataModel.playerAction.lookAtTransform.position, param[0] * modifier);
                                                                                    // ȭ���� �߻�

            CoolCheck = false;  // ��Ÿ�� üũ

            return true;
        }
        return false;
    }
}

