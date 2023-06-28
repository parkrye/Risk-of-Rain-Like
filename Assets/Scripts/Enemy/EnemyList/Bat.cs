using System.Collections;
using UnityEngine;

public class Bat : Enemy, IMazable
{
    protected override void Awake()
    {
        enemyData = GameManager.Resource.Load<EnemyData>("Enemy/Bat");
        base.Awake();
    }

    protected override IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (attack && !isStunned)
            {
                animator.SetTrigger("Attack");
                GameObject enemyBolt = GameManager.Resource.Instantiate(GameManager.Resource.Load<GameObject>("EnemyAttack/EnemyBolt"), transform.position + Vector3.up, Quaternion.identity, true);
                enemyBolt.GetComponent<EnemyBolt>().Shot(GameManager.Data.Player.transform.position, damage);
                yield return new WaitForSeconds(enemyData.AttackSpeed);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void Stuned(float time)
    {
        if (!isStunned)
        {
            StartCoroutine(StunRoutine(time));
        }
    }

    public void Slowed(float time, float modifier)
    {
        if (!isSlowed)
        {
            StartCoroutine(SlowRoutine(time, modifier));
        }
    }
    public void KnockBack(float distance, Transform backFrom)
    {
        StartCoroutine(KnockBackRoutine(distance, backFrom));
    }

    public IEnumerator StunRoutine(float time)
    {
        isStunned = true;
        animator.SetBool("Stun", isStunned);
        StopAttack();
        StopCoroutine(AttackRoutine());
        yield return new WaitForSeconds(time);
        isStunned = false;
        animator.SetBool("Stun", isStunned);
        StartAttack();
        StartCoroutine(AttackRoutine());
    }

    public IEnumerator SlowRoutine(float time, float modifier)
    {
        isSlowed = true;
        float prevMoveSpeed = enemyData.MoveSpeed;
        float prevAttackSpeed = enemyData.AttackSpeed;
        enemyData.MoveSpeed *= modifier;
        enemyData.AttackSpeed *= modifier;
        yield return new WaitForSeconds(time);
        enemyData.MoveSpeed = prevMoveSpeed;
        enemyData.AttackSpeed = prevAttackSpeed;
        isSlowed = false;
    }

    public IEnumerator KnockBackRoutine(float distance, Transform backFrom)
    {
        float now = 0f;
        while (now < distance)
        {
            transform.Translate(backFrom.forward * Time.deltaTime);
            now += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
