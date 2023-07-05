using System.Collections;
using UnityEngine;

/// <summary>
/// ���� 1: ���� ����
/// ���� 2: �޽�
/// </summary>
public class Golem : Boss
{
    IEnumerator healCoroutine;

    protected override void Awake()
    {
        enemyData = GameManager.Resource.Load<EnemyData>("Boss/Golem");
        healCoroutine = HealRoutine();
        base.Awake();
    }

    public override void ChangeToClose()
    {
        animator.SetBool("Heal", false);
        StopCoroutine(healCoroutine);
        StartAttack();
    }

    public override void ChangeToFar()
    {
        StartCoroutine(healCoroutine);
        StopAttack();
    }

    protected override IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (attack)
            {
                animator.SetTrigger("Attack");
                GameManager.Resource.Instantiate<ParticleSystem>("Particle/Explosion", attackTransform.position, Quaternion.identity, true);

                Collider[] colliders = Physics.OverlapSphere(attackTransform.position, enemyData.floatdatas[1]);
                for(int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Player"))
                    {
                        IHitable hittable = colliders[i].GetComponent<IHitable>();
                        hittable?.Hit(damage, 0f);
                    }
                }
                yield return new WaitForSeconds(enemyData.AttackSpeed);
                animator.SetBool("SerialAttack", !animator.GetBool("SerialAttack"));
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator HealRoutine()
    {
        animator.SetBool("Heal", true);
        while (true)
        {
            HP += enemyData.floatdatas[0] * Time.deltaTime;
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        if (onGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackTransform.position, enemyData.floatdatas[1]);
        }
    }
}
