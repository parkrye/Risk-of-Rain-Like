using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ȸ�ΰ� �ִٸ� ����, ���ٸ� ����
/// </summary>
public class Enemy_Condition_CheckBypassRoute : BT_Condition
{
    public Stack<Vector3> bypassStack { get; private set; }

    public Enemy_Condition_CheckBypassRoute(GameObject _enemy)
    {
        enemy = _enemy;
        player = GameManager.Data.Player.gameObject;
        bypassStack = new Stack<Vector3>();
    }
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
        if(bypassStack.Count > 0)
        {
            Debug.Log("Find Bypass");
            enemy.GetComponent<Enemy_AI>().SetBypassRoute(bypassStack);
            return NodeState.Success;
        }
        Debug.Log("not Find Bypass");
        return NodeState.Failure;
    }
}
