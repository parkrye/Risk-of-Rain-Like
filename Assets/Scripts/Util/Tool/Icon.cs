using UnityEngine;

/// <summary>
/// ������ ��ũ���ͺ� ������Ʈ
/// </summary>
[CreateAssetMenu (fileName = "Icon", menuName = "Data/Icon")]
public class Icon : ScriptableObject
{
    public Sprite sprite;   // ������ �̹���
    public string desc;     // ������ ����
}
