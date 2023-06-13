using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behavior_GoStraight : BT_Action
{
    public Enemy_Behavior_GoStraight(GameObject _enemy)
    {
        enemy = _enemy;
        player = GameManager.Data.Player.gameObject;
    }

    public override void Initialize()
    {

    }

    public override void Terminate()
    {

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
            //ȸ��
            enemy.transform.LookAt(player.transform.position);

            //�̵�
            enemy.GetComponent<Rigidbody>().velocity = (enemy.transform.forward * enemy.GetComponent<Enemy>().enemyData.Speed);
        }
    }
}
