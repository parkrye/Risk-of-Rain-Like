using System.Collections;

/// <summary>
/// ���ظ� ���� �� �ִ� �������̽�
/// </summary>
public interface IHitable
{
    /// <summary>
    /// ���ظ� �޴� �޼ҵ�
    /// </summary>
    /// <param name="hitDamage">�߻� ����</param>
    /// <param name="damageDuration">���� ���� �ð�</param>
    public void Hit(float hitDamage, float damageDuration);

    /// <summary>
    /// ���� �߻� ������
    /// </summary>
    /// <param name="hitDamage">�߻� ����</param>
    /// <param name="damageDuration">���� ���� �ð�</param>
    /// <returns>0.1�ʸ��� �߻�</returns>
    IEnumerator HitRoutine(float hitDamage, float damageDuration);

    /// <summary>
    /// ��� �޼ҵ�
    /// </summary>
    public void Die();
}
