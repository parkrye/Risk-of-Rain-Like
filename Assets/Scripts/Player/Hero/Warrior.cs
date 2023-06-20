using UnityEngine;

public class Warrior : Hero
{
    override protected void Awake()
    {
        base.Awake();

        heroName = "Warrior";
    }

    public override bool Jump(bool isPressed)
    {
        if (nowCharge)
            nowCharge = false;

        if (isPressed)
        {
            StartCoroutine(JumpCharger());
        }

        return isPressed;
    }

    protected override void ChargeJump()
    {
        playerDataModel.playerMovement.dirModifier += Vector3.up * playerDataModel.jumpPower * 1.2f * jumpCharge * 0.01f;
        playerDataModel.jumpCount++;
        animator.SetTrigger("JumpH");
        animator.SetTrigger("JumpL");
    }
}
