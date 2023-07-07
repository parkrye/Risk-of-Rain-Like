using System.Collections;
using UnityEngine;

/// <summary>
/// ��ƽ��
/// </summary>
[CreateAssetMenu(fileName = "Archer_Action1B", menuName = "Data/Skill/Archer/Action1B")]
public class Archer_Action1B : Skill, ICriticable, IEnumeratable
{
    [SerializeField] float chargeSpeed, charge; // ���� �ӵ�, ������
    [SerializeField] Arrow arrow;               // ȭ��

    public override bool Active(bool isPressed, params float[] param)
    {
        if (isPressed)  // 
        {
            charge = 0f;

            return true;
        }
        else
        {
            hero.playerDataModel.animator.SetTrigger(actionKeys[actionNum]);
            hero.attackSource.Play();

            Arrow arrowAttack = GameManager.Resource.Instantiate(arrow, true);
            arrowAttack.transform.position = hero.playerDataModel.playerAction.AttackTransform.position;
            arrowAttack.Shot(hero.playerDataModel.playerAction.lookAtTransform.position, param[0] * modifier * charge);

            CoolCheck = false;
            return true;
        }
    }

    public IEnumerator Enumerator()
    {
        while (CoolCheck)
        {
            charge += Time.deltaTime * chargeSpeed;
            if (charge > 2f)
                break;
            yield return null;
        }
    }
}

