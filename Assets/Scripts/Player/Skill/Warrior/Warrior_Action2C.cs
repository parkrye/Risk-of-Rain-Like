using System.Collections;
using UnityEngine;

/// <summary>
/// ������
/// </summary>
[CreateAssetMenu(fileName = "Warrior_Action2C", menuName = "Data/Skill/Warrior/Action2C")]
public class Warrior_Action2C : Skill, IEnumeratable, ICriticable
{
    public float knockbackDistance;
    float damage;

    public override bool Active(bool isPressed, params float[] param)
    {
        if (isPressed)
        {
            damage = param[0];
            hero.playerDataModel.animator.SetTrigger(actionKeys[actionNum]);

            CoolCheck = false;

            return true;
        }
        return false;
    }

    public IEnumerator enumerator()
    {
        yield return new WaitForSeconds(0.08f / hero.playerDataModel.TimeScale);

        ParticleSystem effect = GameManager.Resource.Instantiate(GameManager.Resource.Load<ParticleSystem>("Particle/Dirt"), hero.playerDataModel.playerAction.AttackTransform.position, Quaternion.identity, true);
        effect.transform.LookAt(effect.transform.position + hero.playerDataModel.playerTransform.forward);
        GameManager.Resource.Destroy(effect.gameObject, 1f);
        Collider[] colliders = Physics.OverlapSphere(hero.playerDataModel.playerAction.closeAttackTransform.position, hero.playerDataModel.playerAction.closeAttackRange);
        foreach (Collider collider in colliders)
        {
            if (!collider.CompareTag("Player"))
            {
                IHitable hittable = collider.GetComponent<IHitable>();
                hittable?.Hit(damage * modifier, 0f);

                IMezable mazable = collider.GetComponent<IMezable>();
                mazable?.KnockBack(knockbackDistance, hero.playerDataModel.playerTransform);
            }
        }
    }
}