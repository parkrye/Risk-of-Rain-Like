using UnityEngine;

/// <summary>
/// Ư���� �̵��� ���� �������̽�
/// GroundChecker�� �̿��Ͽ� �̵� �� GroundChecker�� ���鿡 ������ ��� �̵��� ����ϵ��� ����
/// </summary>
public interface ITranslatable
{
    /// <summary>
    /// ��ü�� ������ �̵���Ű�� �Լ�
    /// ���� �̵� ��ο� ���� �ִٸ� �̵����� �ʴ´�
    /// </summary>
    /// <param name="dir">�̵� ����</param>
    /// <param name="distance">�̵� �Ÿ�</param>
    /// <returns>�̵� ���� ����</returns>
    bool TranslateGradually(Vector3 dir, float distance);

    /// <summary>
    /// ��ü�� �ѹ��� �̵���Ű�� �Լ�
    /// ���� �̵� ��ġ�� ��ü�� �ִٸ� ���ǿ� ���� �̵���Ű�� �ʴ´�
    /// </summary>
    /// <param name="pos">�̵� ��ǥ(���� ��ǥ)</param>
    /// <param name="ignoreGround">�浹 ���� ����</param>
    /// <returns>�̵� ���� ����</returns>
    bool TranslateSuddenly(Vector3 pos, bool ignoreGround = true);
}
