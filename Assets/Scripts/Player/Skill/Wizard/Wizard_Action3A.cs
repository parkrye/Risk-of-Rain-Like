using System.Collections;
using UnityEngine;

/// <summary>
/// 전방으로 텔레포트
/// </summary>
[CreateAssetMenu(fileName = "Wizard_Action3A", menuName = "Data/Skill/Wizard/Action3A")]
public class Wizard_Action3A : Skill, IEnumeratable
{
    public float teleportDistance, teleportCharge;
    public bool nowCharge;
    [SerializeField] ParticleSystem magicEffect;

    public override bool Active(bool isPressed, params float[] param)
    {
        if (nowCharge)
            nowCharge = false;

        if (isPressed)
        {
            hero.playerDataModel.animator.SetTrigger(actionKeys[actionNum]);
            hero.powerupSource.Play();

            return true;
        }
        return false;
    }

    public IEnumerator Enumerator()
    {
        teleportCharge = 0f;
        nowCharge = true;
        while (teleportCharge < 1f && nowCharge)
        {
            yield return new WaitForSeconds(0.01f * hero.playerDataModel.ReverseTimeScale);
            teleportCharge += 0.01f;
        }
        Teleport();
    }

    void Teleport()
    {
        RaycastHit hit;
        if (Physics.Raycast(hero.playerDataModel.playerTransform.position + Vector3.up, hero.playerDataModel.playerAction.lookFromTransform.forward.normalized, out hit, teleportDistance * teleportCharge, LayerMask.GetMask("Ground")))
        {
            hero.playerDataModel.playerTransform.position = hit.point;
        }
        else
        {
            hero.playerDataModel.playerTransform.position += hero.playerDataModel.playerAction.lookFromTransform.forward * teleportDistance * teleportCharge;
        }
        CoolCheck = false;
        hero.playerDataModel.animator.SetTrigger("Teleport");
        GameManager.Resource.Instantiate(magicEffect, hero.playerDataModel.playerTransform.position, Quaternion.identity, hero.playerDataModel.playerTransform, true);
    }
}
