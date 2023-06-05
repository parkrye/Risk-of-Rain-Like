public class Archer : Hero
{
    public override bool Jump(bool isPressed)
    {
        return isPressed;
    }

    public override bool Action1(bool isPressed, float coolTime)
    {
        if (coolChecks[0] && isPressed)
        {
            Animator.SetTrigger("Action1");

            coolChecks[0] = false;
            StartCoroutine(CoolTime(0, coolTime));
            return true;
        }
        return false;
    }

    public override bool Action2(bool isPressed, float coolTime)
    {
        if (coolChecks[1] && isPressed)
        {
            Animator.SetTrigger("Action2");

            coolChecks[1] = false;
            StartCoroutine(CoolTime(1, coolTime));
            return true;
        }
        return false;
    }

    public override bool Action3(bool isPressed, float coolTime)
    {
        if (coolChecks[2] && isPressed)
        {
            Animator.SetTrigger("Action3");

            coolChecks[2] = false;
            StartCoroutine(CoolTime(2, coolTime));
            return true;
        }
        return false;
    }

    public override bool Action4(bool isPressed, float coolTime)
    {
        if (coolChecks[3] && isPressed)
        {
            Animator.SetTrigger("Action4");

            coolChecks[3] = false;
            StartCoroutine(CoolTime(3, coolTime));
            return true;
        }
        return false;
    }
}
