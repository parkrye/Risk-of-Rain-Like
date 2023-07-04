using UnityEngine;

/// <summary>
/// ����ź �߻�
/// </summary>
[CreateAssetMenu(fileName = "Wizard_Action1A", menuName = "Data/Skill/Wizard/Action1A")]
public class Wizard_Action1A : Skill, ICriticable
{
    public override bool Active(bool isPressed, params float[] param)
    {
        if (isPressed)
        {
            hero.playerDataModel.animator.SetTrigger(actionKeys[actionNum]);
            hero.attackSource.Play();

            ParticleSystem effect = GameManager.Resource.Instantiate(GameManager.Resource.Load<ParticleSystem>("Particle/MagicEffect"), hero.playerDataModel.playerTransform.position, Quaternion.identity, hero.playerDataModel.playerTransform, true);

            GameObject energyBolt = GameManager.Resource.Instantiate(GameManager.Resource.Load<GameObject>("Attack/EnergyBolt"), true);
            energyBolt.transform.position = hero.playerDataModel.playerAction.AttackTransform.position;
            energyBolt.GetComponent<Bolt>().Shot(hero.playerDataModel.playerAction.lookAtTransform.position, param[0] * modifier);

            CoolCheck = false;

            return true;
        }
        return false;
    }
}
