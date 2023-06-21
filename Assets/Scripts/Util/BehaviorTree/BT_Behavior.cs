// �ൿ ����� ����
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Invalid, // �⺻ ����
    Success, // ���� ����
    Failure, // ���� ����
    Running, // ���� ����
    Aborted, // ���� �߻�
}

// ����� ����
public enum NodeType
{
    Root,       // ��Ʈ ���
    Fallback,   // �ڽ� ��带 ��ȸ. �ϳ��� �����ϸ� ��� ���� ��ȯ. ��� �����ϸ� ���� ��ȯ. Or ����
    Sequence,   // �ڽ� ��带 ��ȸ. �ϳ��� �����ϸ� ��� ���� ��ȯ. ��� �����ϸ� ���� ��ȯ. And ����
    Condition,  // ������ ���� ��� �ڽ� ��带 ����
    Action,     // �ൿ ���
}

public class BT_Behavior
{
    NodeState state;
    NodeType type;
    int index;
    BT_Behavior parent;
    protected GameObject enemy, player;

    public BT_Behavior()
    {
        state = NodeState.Invalid;
    }

    public void SetParent(BT_Behavior node)
    {
        parent = node;
    }

    public BT_Behavior GetParent()
    {
        return parent;
    }

    public NodeState GetState()
    {
        return state;
    }

    public void SetState(NodeState _state)
    {
        state = _state;
    }

    public NodeType GetNodeType()
    {
        return type;
    }

    public void SetNodeType(NodeType _type)
    {
        type = _type;
    }

    public int GetIndex()
    {
        return index;
    }

    public void SetIndex(int _index)
    {
        index = _index;
    }

    /// <summary>
    /// ��� ���� �ʱ�ȭ
    /// </summary>
    virtual public void Reset()
    {
        state = NodeState.Invalid;
    }

    /// <summary>
    /// ��� �ʱ�ȭ
    /// </summary>
    public virtual void Initialize()
    {

    }

    /// <summary>
    /// ��� ����
    /// </summary>
    /// <returns>���� �� ����</returns>
    public virtual NodeState Renew()
    {
        return NodeState.Success;
    }

    /// <summary>
    /// ��� ����
    /// </summary>
    public virtual void Terminate()
    {

    }

    /// <summary>
    /// ������Ʈ ó��
    /// </summary>
    /// <returns></returns>
    public virtual NodeState Tick()
    {
        // ��尡 �ʱ� ���¶��
        if(state == NodeState.Invalid)
        {
            // �ʱ�ȭ
            Initialize();
            // ���¸� ��������
            state = NodeState.Running;
        }

        // ��带 �����ϰ� ��ȯ�� ���� ����
        state = Renew();

        // ���°� �������� �ƴ϶��
        if(state != NodeState.Running)
        {
            // ����
            Terminate();
        }

        // ��� ���� ��ȯ
        return state;
    }
}
