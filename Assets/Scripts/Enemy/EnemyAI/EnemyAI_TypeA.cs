using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공중 몬스터 AI
/// 접근 => ( 우회 접근 ) => 공격
/// </summary>
public class EnemyAI_TypeA : EnemyAI
{
    enum AI_State { Approach, Bypass, Attack, Dumb }
    [SerializeField] AI_State state;
    Stack<Vector3> bypass;
    Vector3 prevPlayerPosition;
    [SerializeField] bool moveSide;
    Vector3 playerPos;
    [SerializeField] float deleteTimer;

    protected override void Awake()
    {
        base.Awake();
        state = AI_State.Approach;
        bypass = new Stack<Vector3>();
    }

    protected override IEnumerator StateRoutine()
    {
        while (this)
        {
            if (enemy && enemy.alive)
            {
                state = StateCheck();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    protected override IEnumerator BehaviorRoutine()
    {
        while (this)
        {
            if (!enemy.isStunned && enemy.alive)
            {
                switch (state)
                {
                    case AI_State.Approach:
                        ApproachMove();
                        break;
                    case AI_State.Bypass:
                        ByPassMove();
                        break;
                    case AI_State.Attack:
                        AttackMove();
                        break;
                    case AI_State.Dumb:
                        DumbMove();
                        break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    AI_State StateCheck()
    {
        playerPos = playerTransform.position + Vector3.up;
        float distance = Vector3.Distance(playerPos, enemy.EnemyPos);

        if (distance <= enemy.enemyData.Range)
        {
            if (!Physics.Raycast(enemy.EnemyPos, (playerPos - enemy.EnemyPos).normalized, distance - enemy.enemyData.Size, LayerMask.GetMask("Ground")))
            {
                if (state != AI_State.Attack)
                {
                    enemy.StartAttack();
                }
                return AI_State.Attack;
            }
        }

        if (Physics.SphereCast(enemy.EnemyPos, enemy.enemyData.Size, (playerPos - enemy.EnemyPos).normalized, out _, distance - enemy.enemyData.Size, LayerMask.GetMask("Ground")))
        {
            if (state != AI_State.Bypass)
            {
                enemy.StopAttack();
                StartCoroutine(FindBypass());
            }
            return AI_State.Bypass;
        }


        if (distance > enemy.enemyData.Range)
        {
            enemy.StopAttack();
            return AI_State.Approach;
        }

        return state;
    }

    void ApproachMove()
    {
        transform.LookAt(playerPos);
        transform.Translate((playerPos - enemy.EnemyPos).normalized * enemy.enemyData.MoveSpeed * Time.deltaTime, Space.World);
    }

    void AttackMove()
    {
        transform.LookAt(playerPos);
        if (moveSide)
        {
            if (!enemy.TranslateGradually(transform.right, enemy.enemyData.MoveSpeed * 0.5f * Time.deltaTime))
                moveSide = !moveSide;
        }
        else
        {
            if (!enemy.TranslateGradually(-transform.right, enemy.enemyData.MoveSpeed * 0.5f * Time.deltaTime))
                moveSide = !moveSide;
        }
    }

    void ByPassMove()
    {
        if (bypass.Count > 0)
        {
            transform.LookAt(Vector3.Lerp(transform.forward, bypass.Peek(), Time.deltaTime));
            transform.Translate(enemy.enemyData.MoveSpeed * Time.deltaTime * (bypass.Peek() - enemy.EnemyPos).normalized, Space.World);
            if(Vector3.SqrMagnitude((enemy.EnemyPos) - bypass.Peek()) <= 1f)
                bypass.Pop();
        }
    }

    void DumbMove()
    {
        transform.Rotate(Vector3.up);
        transform.Translate(transform.up * Time.deltaTime);
        deleteTimer += Time.deltaTime;
        if(deleteTimer > 60f)
            GameManager.Resource.Destroy(gameObject);
    }

    IEnumerator FindBypass()
    {
        yield return null;
        while (state == AI_State.Bypass)
        {
            if (Vector3.SqrMagnitude(playerPos - prevPlayerPosition) > 9f || bypass.Count == 0)
            {
                transform.LookAt(playerPos);
                bypass = PathFinder.PathFinding(transform, enemy.EnemyPos, playerPos, enemy.enemyData.Size);
                prevPlayerPosition = playerPos;
            }

            if(bypass.Count == 0)
            {
                state = AI_State.Dumb;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void OnDrawGizmos()
    {
        if (enemy.onGizmo)
        {
            Gizmos.color = Color.green;
            if (state == AI_State.Bypass && bypass.Count > 0)
            {
                foreach (Vector3 bypass in bypass)
                {
                    Gizmos.DrawWireSphere(bypass, enemy.enemyData.Size);
                }
            }
        }
    }
}
