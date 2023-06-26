using System.Collections;
using UnityEngine;

/// <summary>
/// ���� ������
/// </summary>
[CreateAssetMenu(fileName = "Warrior_Action1B", menuName = "Data/Skill/Warrior/Action1B")]
public class Warrior_Action1B : Skill
{
    public override bool Active(bool isPressed)
    {
        if (isPressed)
        {
            hero.playerDataModel.animator.SetTrigger(actionKeys[actionNum]);

            GameObject slash = GameManager.Resource.Instantiate(GameManager.Resource.Load<GameObject>("Attack/Slash"), true);
            slash.transform.position = hero.playerDataModel.playerAction.AttackTransform.position;
            slash.GetComponent<Bolt>().Shot(hero.playerDataModel.playerAction.lookAtTransform.position, hero.playerDataModel.AttackDamage * modifier, 0.05f);

            CoolCheck = false;

            return true;
        }
        return false;
    }
}