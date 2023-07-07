using System.Collections;
using UnityEngine;

/// <summary>
/// ���� ���� AI
/// �÷��̾���� �Ÿ��� ���� �ΰ��� ������ ���ϴ� AI
/// ���� ������ ���� ���͸��� ����
/// </summary>
public class EnemyAI_TypeB : EnemyAI
{
    [SerializeField] bool onGizmo, onCheckHP;
    Boss boss;
    Vector3 playerPos;

    protected override void Awake()
    {
        base.Awake();
        boss = GetComponent<Boss>();
    }

     protected override void OnEnable()
    {
        base.OnEnable();
        boss.StartAttack();
        onCheckHP = true;
    }

    protected override IEnumerator StateRoutine()
    {
        while (this)
        {
            playerPos = playerTransform.position + Vector3.up;
            if (Vector3.SqrMagnitude(playerPos - enemy.EnemyPos) <= (enemy.enemyData.Range * enemy.enemyData.Range))
            {
                boss.ChangeToNear();
            }
            else
            {
                boss.ChangeToFar();
            }

            if (onCheckHP && boss.HP <= boss.enemyData.MaxHP * 0.5f)
            {
                boss.ChangeToCritical();
                onCheckHP = false;
            }
            yield return null;
        }
    }

    protected override IEnumerator BehaviorRoutine()
    {
        yield return null;
    }
}
