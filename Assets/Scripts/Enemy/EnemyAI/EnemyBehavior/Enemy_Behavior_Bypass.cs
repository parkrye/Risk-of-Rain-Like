using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behavior_Bypass : BT_Action
{
    Stack<Vector3> bypassStack;

    public Enemy_Behavior_Bypass(GameObject _enemy)
    {
        enemy = _enemy;
        player = GameManager.Data.Player.gameObject;
        bypassStack = new Stack<Vector3>();
    }

    /// <summary>
    /// �ʱ�ȭ�� ��ã�� ��Ʈ ����
    /// </summary>
    public override void Initialize()
    {
        bypassStack = PathFinder.PathFindingForAerial(enemy.transform, player.transform, enemy.GetComponent<Enemy>().enemyData.Range);
    }

    public override void Terminate()
    {
        bypassStack.Clear();
    }

    public override NodeState Renew()
    {
        OnChase();
        return NodeState.Running;
    }

    void OnChase()
    {
        if (player)
        {
            enemy.GetComponent<Rigidbody>().velocity = (bypassStack.Peek().normalized * enemy.GetComponent<Enemy>().enemyData.Speed);
            if(Vector3.Distance(enemy.transform.position, bypassStack.Peek()) <= 0.5f)
                bypassStack.Pop();
        }
    }
}
