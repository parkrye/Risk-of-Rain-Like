using UnityEngine;

/// <summary>
/// ��Ÿ� ���� ���� �ִٸ� ����, ���ٸ� ����
/// </summary>
public class Enemy_Condition_CheckWall : BT_Condition
{
    public Enemy_Condition_CheckWall(GameObject _enemy)
    {
        enemy = _enemy;
        player = GameManager.Data.Player.gameObject;
    }

    public override NodeState Renew()
    {
        if (player)
        {
            if(Physics.Raycast(enemy.transform.position, (player.transform.position - enemy.transform.position).normalized, enemy.GetComponent<Enemy>().enemyData.Range, LayerMask.GetMask("Ground")))
            {
                Debug.Log("Wall Beside");
                return NodeState.Success;
            }
        }
        Debug.Log("Not Wall");
        return NodeState.Failure;
    }
}
