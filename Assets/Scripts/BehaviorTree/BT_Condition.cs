public class BT_Condition : BT_Behavior
{
    public BT_Condition()
    {
        SetNodeType(NodeType.Condition);
    }

    public override NodeState Tick()
    {
        // ������ Ȯ��
        SetState(Renew());

        if(GetState() == NodeState.Running)
        {
            // Renew�� ����� �ݵ�� success, failure �� �� �ϳ���
        }

        // ���ǿ� ���� ����� ��ȯ
        return GetState();
    }
}
