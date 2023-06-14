using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behavior_TakeRoundabout : BT_Action
{
    Stack<Vector3> roundaboutDir;

    public Enemy_Behavior_TakeRoundabout(GameObject _enemy)
    {
        enemy = _enemy;
        player = GameManager.Data.Player.gameObject;
        roundaboutDir = new Stack<Vector3>();
    }

    /// <summary>
    /// �ʱ�ȭ�� ��ã�� ��Ʈ ����
    /// </summary>
    public override void Initialize()
    {
        roundaboutDir = PathFinder.PathFindingForAerial(enemy.transform, player.transform, enemy.GetComponent<Enemy>().enemyData.Range);
    }

    public override void Terminate()
    {
        roundaboutDir.Clear();
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
            enemy.GetComponent<Rigidbody>().velocity = (roundaboutDir.Peek().normalized * enemy.GetComponent<Enemy>().enemyData.Speed);
            if(Vector3.Distance(enemy.transform.position, roundaboutDir.Peek()) <= 0.5f)
                roundaboutDir.Pop();
        }
    }
}
