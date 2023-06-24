using UnityEngine;

/// <summary>
/// ���� ź�� �߻�
/// </summary>
[CreateAssetMenu(fileName = "Wizard_Action1A", menuName = "Data/Skill/Wizard/Action1A")]
public class Wizard_Action1A : Skill
{
    public override bool Active(bool isPressed)
    {
        if (isPressed)
        {
            hero.playerDataModel.animator.SetTrigger(actionKeys[actionNum]);

            GameObject energyBolt = GameManager.Resource.Instantiate(GameManager.Resource.Load<GameObject>("Attack/EnergyBolt"), true);
            energyBolt.transform.position = hero.playerDataModel.playerAction.AttackTransform.position;
            energyBolt.GetComponent<Bolt>().Shot(hero.playerDataModel.playerAction.lookAtTransform.position, hero.playerDataModel.AttackDamage * modifier);

            CoolCheck = false;

            return true;
        }
        return false;
    }
}
