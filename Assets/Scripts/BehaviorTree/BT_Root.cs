public class BT_Root : BT_Behavior
{
    BT_Behavior child;

    public BT_Root()
    {
        SetNodeType(NodeType.Root);
        SetParent(null);
    }

    public void AddChild(BT_Behavior _child)
    {
        child = _child;
        child.SetParent(this);
    }

    public BT_Behavior GetChild()
    {
        return child;
    }

    /// <summary>
    /// �ڽ� ��带 ����
    /// </summary>
    public override void Terminate()
    {
        child.Terminate();
    }

    public override NodeState Tick()
    {
        // �ڽ��� ���ٸ� �ʱ� �������� ��ȯ
        if (child == null)
            return NodeState.Invalid;
        // �ڽ��� ���°� �ʱ� ���¶��
        else if(child.GetState() == NodeState.Invalid)
        {
            // �ڽ� ��带 �ʱ�ȭ�ϰ�
            child.Initialize();
            // �ڽ� ��� ���¸� ���������� �����ϰ�
            child.SetState(NodeState.Running);
        }
        // �ڽ� ��带 �����ϰ� �� ���¸� �ڽ��� ���¿� ����
        SetState(child.Renew());

        // �ڽ��� ���¸� �ڽ��� ���·� �������ְ�
        child.SetState(GetState());

        // �ڽ��� ���°� ���� ���̶��
        if(GetState() != NodeState.Running)
        {
            // �ڽ� ��带 ����
            Terminate();
        }

        // �ڽ��� ���¸� ��ȯ
        return GetState();
    }
}
