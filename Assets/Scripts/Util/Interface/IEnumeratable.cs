using System.Collections;

/// <summary>
/// ������ ���� �������̽�
/// Monobehavior�� ��ӹ��� ���� ��ũ��Ʈ���� �����ڰ� �ʿ��� ���, �ܺο��� ��ũ��Ʈ�� ������ ���� ���θ� �ľ��ϰ� ��� ��������ֱ� ����
/// </summary>
public interface IEnumeratable
{
    /// <summary>
    /// �ܺο��� ��������� �� �ִ� ������
    /// </summary>
    /// <returns></returns>
    public IEnumerator Enumerator();
}
