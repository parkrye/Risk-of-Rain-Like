/// <summary>
/// �ڽ� ��带 ���������� ������Ʈ
/// �ڽ� ��尡 �����Ѵٸ� ���� �ڽ� ��� ����
/// �ڽ� ��尡 �����Ѵٸ� ���� �ڽ� ���� �������� ����
/// </summary>
public class BT_Sequence : BT_Composite
{
    public BT_Sequence()
    {
        SetNodeType(NodeType.Sequence);
    }

    public override NodeState Renew()
    {
        NodeState currentState = NodeState.Invalid;

        for(int i = 0; i < GetChildrenCount(); i++)
        {
            // ���� �ڽ��� ����
            currentState = GetChild(i).GetState();

            // ���� �ڽ� ��尡 �ൿ�� �ƴϰų�, (�ൿ�̶��) ���� �ڽ� ����� ���°� ������ �ƴ϶��
            if(GetChild(i).GetNodeType() != NodeType.Action || GetChild(i).GetState() != NodeState.Success)
            {
                // ���� �ڽ� ��带 ������Ʈ
                currentState = GetChild(i).Tick();
            }

            // ������Ʈ �� ���� �ڽ� ��尡 �������� �ʾҴٸ�
            if(currentState != NodeState.Success)
            {
                // ���� ��� ���¸� ��ȯ
                return currentState;
            }
            // �����ߴٸ� ���� �ڽ� ����
            else
            {
                continue;
            }
        }

        // ��� �ڽ� ��尡 �����̶�� ������ ��ȯ
        return NodeState.Success;
    }
}
