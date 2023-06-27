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
}
