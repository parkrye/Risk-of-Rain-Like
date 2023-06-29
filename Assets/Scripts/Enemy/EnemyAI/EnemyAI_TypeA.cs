using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� AI
/// ���� => ( ��ȸ ���� ) => ����
/// </summary>
public class EnemyAI_TypeA : EnemyAI
{
    enum AI_State { Approach, Bypass, Attack, Dumb }
    [SerializeField] AI_State state;
    Stack<Vector3> bypass;
    Vector3 prevPlayerPosition;
    [SerializeField] bool onGizmo, moveSide;
    Vector3 playerPos;
    [SerializeField] float deleteTimer;

    protected override void Awake()
    {
        base.Awake();
        state = AI_State.Approach;
        bypass = new Stack<Vector3>();
    }

    void Update()
    {
        state = StateCheck();
    }

    void FixedUpdate()
    {
        if (!enemy.isStunned)
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
    }

    AI_State StateCheck()
    {
        playerPos = playerTransform.position + Vector3.up;
        float distance = Vector3.Distance(playerPos, enemy.enemyPos);

        if (distance <= enemy.enemyData.Range)
        {
            if(state != AI_State.Attack)
            {
                if (!Physics.Raycast(enemy.enemyPos, (playerPos - enemy.enemyPos).normalized, distance - enemy.enemyData.Size * 0.5f, LayerMask.GetMask("Ground")))
                {
                    enemy.StartAttack();
                    StopCoroutine(FindBypass());
                    return AI_State.Attack;
                }
            }
        }

        if(state != AI_State.Bypass)
        {
            if (Physics.SphereCast(enemy.enemyPos, enemy.enemyData.Size, (playerPos - enemy.enemyPos).normalized, out _, distance - enemy.enemyData.Size * 0.5f, LayerMask.GetMask("Ground")))
            {
                enemy.StopAttack();
                StartCoroutine(FindBypass());
                return AI_State.Bypass;
            }
        }

        if(distance > enemy.enemyData.Range)
        {
            enemy.StopAttack();
            StopCoroutine(FindBypass());
            return AI_State.Approach;
        }

        return state;
    }

    void ApproachMove()
    {
        transform.LookAt(playerPos);
        transform.Translate((playerPos - enemy.enemyPos).normalized * enemy.enemyData.MoveSpeed * Time.deltaTime, Space.World);
    }

    void AttackMove()
    {
        transform.LookAt(playerPos);
        if (moveSide)
        {
            moveSide = enemy.TranslateGradually(transform.right, enemy.enemyData.MoveSpeed * Time.deltaTime);
        }
        else
        {
            moveSide = enemy.TranslateGradually(-transform.right, enemy.enemyData.MoveSpeed * Time.deltaTime);
        }
    }

    void ByPassMove()
    {
        if (bypass.Count > 0)
        {
            transform.LookAt(bypass.Peek());
            transform.Translate(enemy.enemyData.MoveSpeed * Time.deltaTime * (bypass.Peek() - enemy.enemyPos).normalized, Space.World);
            if(Vector3.SqrMagnitude((enemy.enemyPos) - bypass.Peek()) <= 1f)
                bypass.Pop();
        }
    }

    void DumbMove()
    {
        transform.Rotate(Vector3.up);
        deleteTimer += Time.deltaTime;
        if(deleteTimer > 60f)
            GameManager.Resource.Destroy(gameObject);
    }

    IEnumerator FindBypass()
    {
        while (state == AI_State.Bypass)
        {
            if (Vector3.SqrMagnitude(playerPos - prevPlayerPosition) > 9f || bypass.Count == 0)
            {
                transform.LookAt(playerPos);
                bypass = PathFinder.PathFinding(transform, enemy.enemyPos, playerPos, enemy.enemyData.Size);
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
        if (onGizmo)
        {
            Gizmos.color = Color.green;
            if (state == AI_State.Bypass && bypass.Count > 0)
            {
                foreach (var bypass in bypass)
                {
                    Gizmos.DrawWireSphere(bypass, enemy.enemyData.Size);
                }
            }
        }
    }
}
