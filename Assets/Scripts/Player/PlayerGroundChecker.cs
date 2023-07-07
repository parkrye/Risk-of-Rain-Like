using System.Collections;
using UnityEngine;

/// <summary>
/// �÷��̾��� ���� ���˿� ���� ��ũ��Ʈ
/// </summary>
public class PlayerGroundChecker : MonoBehaviour
{
    /// <summary>
    /// ���� ���� ����
    /// ����(0.5f)�� �������� �ʾҰ� ����ִ� ������ 1�� �̻��̶�� true, �ƴ϶�� false
    /// </summary>
    public bool IsGround
    {
        get 
        { 
            if (ready && groundCounter > 0) 
                return true;
            return false;
        }
    }
    int groundCounter;
    bool ready;

    void Awake()
    {
        groundCounter = 0;
        ready = true;
    }

    /// <summary>
    /// ���� ���˽� ī���͸� 1 �ø���
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == LayerMask.GetMask("Ground"))
        {
            groundCounter++;
        }
    }

    /// <summary>
    /// ���� Ż��� ī���͸� 1 ���δ�
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if((1 << other.gameObject.layer) == LayerMask.GetMask("Ground"))
        {
            groundCounter--;
        }
    }

    /// <summary>
    /// ���� �� �Ͻ������� ���� �˻縦 �������� �ϴ� �޼ҵ�
    ///  ������ ��� ĳ������ ��ġ�� ��ü�� �ӵ��� y������ 3~4 ������ ������ �̻�(���̳ʽ� ������ ����)�� �߻�
    ///  �� ������ �÷��̾��� ���� �ʱ�ȭ ������ �ǵ�ġ �ʰ� �߻��Ͽ�, ���鿡�� ������ ���� ���� Ž���� ������ false�� ������ִ� �޼ҵ�� �ӽ� �ذ�
    /// </summary>
    public void JumpReady()
    {
        StartCoroutine(JumpReadyRoutine());
    }

    IEnumerator JumpReadyRoutine()
    {
        ready = false;
        yield return new WaitForSeconds(0.5f);
        ready = true;
    }
}
