/// <summary>
/// �ڽ� ��带 ���������� ������Ʈ
/// �ڽ� ��尡 �����Ѵٸ� ���� �ڽ� ��� ����
/// �ڽ� ��尡 �����Ѵٸ� ���� �ڽ� ���� �������� ����
/// </summary>
public class BT_Fallback : BT_Composite
{
    public BT_Fallback()
    {
        SetNodeType(NodeType.Fallback);
    }

    public override NodeState Renew()
    {
        for(int i = 0; i < GetChildrenCount(); i++)
        {
            // �� �ڽ� ��带 �ϴ� ������Ʈ
            NodeState currentState = GetChild(i).Tick();

            // ���� �ڽ� ��尡 �������� �ʾҴٸ�
            if(currentState != NodeState.Failure)
            {
                // �ش� ��带 ������ ��� �ڽ� ��带 �ʱ�ȭ
                ClearChild(i);
                // �ش� ����� ���� ����� ��ȯ
                return currentState;
            }
            // �����ߴٸ� ���� �ڽ� ����
            else
            {
                continue;
            }
        }

        // ��� �����ߴٸ� ���и� ��ȯ
        return NodeState.Failure;
    }

    /// <summary>
    /// Ư�� �ڽ��� ������ ��� �ڽ��� �ʱ�ȭ
    /// </summary>
    /// <param name="skipIndex">�ʱ�ȭ���� ���� ��ȣ</param>
    protected void ClearChild(int skipIndex)
    {
        for(int i = 0; i < GetChildrenCount(); i++) 
        { 
            if(i != skipIndex)
            {
                GetChild(i).Reset();
            }
        }
    }
}
