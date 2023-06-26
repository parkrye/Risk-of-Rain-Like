using System.Collections;
using UnityEngine;

/// <summary>
/// ��ȭ
/// </summary>
[CreateAssetMenu(fileName = "Warrior_Action4C", menuName = "Data/Skill/Warrior/Action4C")]
public class Warrior_Action4C : Skill, IEnumeratable
{
    public float skillTime;

    public override bool Active(bool isPressed)
    {
        if (isPressed)
        {
            hero.playerDataModel.animator.SetTrigger(actionKeys[actionNum]);

            CoolCheck = false;

            return true;
        }
        return false;
    }

    public IEnumerator enumerator()
    {
        hero.playerDataModel.Buff(0, modifier);
        hero.playerDataModel.Buff(1, modifier);
        hero.playerDataModel.Buff(2, modifier);
        hero.playerDataModel.Buff(3, 1 / modifier);
        yield return new WaitForSeconds(skillTime);
        hero.playerDataModel.Buff(0, 1 / modifier);
        hero.playerDataModel.Buff(1, 1 / modifier);
        hero.playerDataModel.Buff(2, 1 / modifier);
        hero.playerDataModel.Buff(3, modifier);
    }
}