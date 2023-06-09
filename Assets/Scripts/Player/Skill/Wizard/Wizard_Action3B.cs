using UnityEngine;

/// <summary>
/// �ǵ�
/// </summary>
[CreateAssetMenu(fileName = "Wizard_Action3B", menuName = "Data/Skill/Wizard/Action3B")]
public class Wizard_Action3B : Skill, IDamageSubscriber
{
    public float shieldPoint;
    bool summonMagicFlat;
    [SerializeField] ParticleSystem magicFlatPRefab;
    ParticleSystem magicFlat;

    public override bool Active(bool isPressed, params float[] param)
    {
        if (isPressed)
        {
            hero.playerDataModel.animator.SetTrigger(actionKeys[actionNum]);
            hero.powerupSource.Play();

            if (!summonMagicFlat)
                magicFlat = GameManager.Resource.Instantiate(magicFlatPRefab, hero.transform.position, Quaternion.identity, hero.transform, true);
            summonMagicFlat = true;
            shieldPoint = param[0];
            hero.playerDataModel.playerSystem.AddDamageSubscriber(this);
            CoolCheck = false;
            return true;
        }
        return false;
    }

    public float ModifiyDamage(float _damage)
    {
        if(_damage > shieldPoint)
        {
            hero.playerDataModel.playerSystem.RemoveDamageSubscriber(this);
            GameManager.Resource.Destroy(magicFlat);
            summonMagicFlat = false;
            return _damage - shieldPoint;
        }
        else
        {
            shieldPoint -= _damage;
            return 0f;
        }
    }
}
