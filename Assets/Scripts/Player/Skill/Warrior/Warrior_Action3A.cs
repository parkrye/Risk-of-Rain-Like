using UnityEngine;

/// <summary>
/// ������
/// </summary>
[CreateAssetMenu(fileName = "Warrior_Action3A", menuName = "Data/Skill/Warrior/Action3A")]
public class Warrior_Action3A : Skill
{
    public override bool Active(bool isPressed)
    {
        if (isPressed)
        {
            hero.playerDataModel.animator.SetTrigger(actionKeys[2]);

            Vector3 dashVec = hero.playerDataModel.playerMovement.moveDir;
            if (dashVec.magnitude == 0f)
                dashVec.z = 1f;

            CoolCheck = false;

            return true;
        }
        return false;
    }
}
