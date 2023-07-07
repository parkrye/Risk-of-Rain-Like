/// <summary>
/// ����� ������ �ְ��ϴ� �������̽�
/// </summary>
public interface IDamagePublisher
{
    /// <summary>
    /// ����� ���� �����ڸ� �߰��Ѵ�
    /// </summary>
    /// <param name="_subscriber">����� ���� ������</param>
    public void AddDamageSubscriber(IDamageSubscriber _subscriber);

    /// <summary>
    /// ����� ���� �����ڸ� �����Ѵ�
    /// </summary>
    /// <param name="_subscriber">������ ����� ���� ������</param>
    public void RemoveDamageSubscriber(IDamageSubscriber _subscriber);
    
    /// <summary>
    /// ����� �߻��� ���� �������� �޼ҵ带 �����Ų��
    /// </summary>
    /// <param name="_damage">�߻��� �����</param>
    /// <returns>������ �����</returns>
    public float DamageOccurrence(float _damage);
}
