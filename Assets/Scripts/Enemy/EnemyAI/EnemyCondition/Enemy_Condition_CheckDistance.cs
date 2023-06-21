using UnityEngine;

/// <summary>
/// �Ÿ��� ��Ÿ� ���϶�� ����, �ʰ���� ����
/// </summary>
public class Enemy_Condition_CheckDistance : BT_Condition
{
    public Enemy_Condition_CheckDistance(GameObject _enemy)
    {
        enemy = _enemy;
        player = GameManager.Data.Player.gameObject;
    }

    public override NodeState Renew()
    {
        if (player)
        {
            float fDistance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (fDistance <= enemy.GetComponent<Enemy>().enemyData.Range)
            {
                Debug.Log("In Attack Range");
                return NodeState.Success;
            }
        }
        Debug.Log("Out Attack Range");
        return NodeState.Failure;
    }
}
