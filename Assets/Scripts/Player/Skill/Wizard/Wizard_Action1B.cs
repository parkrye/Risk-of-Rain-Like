using System.Collections;
using UnityEngine;

/// <summary>
/// ����ź�� 3�� �߻�
/// </summary>
[CreateAssetMenu(fileName = "Wizard_Action1B", menuName = "Data/Skill/Wizard/Action1B")]
public class Wizard_Action1B : Skill, ICriticable, IEnumeratable
{
    float damage;

    public override bool Active(bool isPressed, params float[] param)
    {
        if (isPressed)
        {
            hero.playerDataModel.animator.SetTrigger(actionKeys[actionNum]);
            damage = param[0] * modifier;
            CoolCheck = false;

            return true;
        }
        return false;
    }

    public IEnumerator enumerator()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject followBolt = GameManager.Resource.Instantiate(GameManager.Resource.Load<GameObject>("Attack/FollowEnergyBolt"), true);
            followBolt.transform.position = hero.playerDataModel.playerAction.AttackTransform.position;
            followBolt.GetComponent<FollowBolt>().Shot(hero.playerDataModel.playerAction.lookAtTransform.position, damage);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
