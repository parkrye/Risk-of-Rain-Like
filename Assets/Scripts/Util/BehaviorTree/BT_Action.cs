using UnityEngine;

public class BT_Action : BT_Behavior
{
    public BT_Action()
    {
        SetNodeType(NodeType.Action);
    }

    public override void Reset()
    {
        SetState(NodeState.Invalid);
    }

    public override NodeState Tick()
    {
        // ���� ���°� �ʱ� ���¶��
        if(GetState() == NodeState.Invalid)
        {
            // �ʱ�ȭ
            Initialize();
            // ���¸� ��������
            SetState(NodeState.Running);
        }

        // �����ϰ� �� ���¸� ����
        SetState(Renew());

        // ���� ���°� �������� �ƴ϶��
        if(GetState() != NodeState.Running) 
        { 
            // ����
            Terminate();
        }

        // ���� ���� ��ȯ
        return GetState();
    }
}
