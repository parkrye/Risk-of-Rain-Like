/// <summary>
/// ����� ���� ������
/// </summary>
public interface IDamageSubscriber
{
    /// <summary>
    /// �߻� ������� �����Ͽ� ��ȯ�Ѵ�
    /// </summary>
    /// <param name="originDamage">�Է� �����</param>
    /// <returns>���� �����</returns>
    public float ModifiyDamage(float originDamage);
}
