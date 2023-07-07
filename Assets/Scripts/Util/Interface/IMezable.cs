using System.Collections;
using UnityEngine;

/// <summary>
/// ���� ȿ�� �������̽�
/// </summary>
public interface IMezable
{
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="stunDuration">���� �ð�</param>
    public void Stuned(float stunDuration);

    /// <summary>
    /// ���� ������
    /// </summary>
    /// <param name="stunDuration">���� �ð�</param>
    /// <returns></returns>
    IEnumerator StunRoutine(float stunDuration);

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="slowDuration">���� �ð�</param>
    /// <param name="slowModifier">���� ����</param>
    public void Slowed(float slowDuration, float slowModifier);

    /// <summary>
    /// ���� ������
    /// </summary>
    /// <param name="slowDuration">���� �ð�</param>
    /// <param name="slowModifier">���� ����</param>
    /// <returns></returns>
    IEnumerator SlowRoutine(float slowDuration, float slowModifier);

    /// <summary>
    /// �и�
    /// </summary>
    /// <param name="knockBackDistance">�и� �Ÿ�</param>
    /// <param name="backFromTransform">�� ��ġ</param>
    public void KnockBack(float knockBackDistance, Transform backFromTransform);

    /// <summary>
    /// �и� ������
    /// </summary>
    /// <param name="knockBackDistance">�и� �Ÿ�</param>
    /// <param name="backFromTransform">�� ��ġ</param>
    /// <returns></returns>
    IEnumerator KnockBackRoutine(float knockBackDistance, Transform backFromTransform);
}
