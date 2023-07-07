using UnityEngine;

/// <summary>
/// �������� ���� �׸��� ��ũ��Ʈ
/// </summary>
public class CircleDrawer : MonoBehaviour
{
    LineRenderer circleRenderer;    // ���� ������
    int steps;                      // �� ����
    float radius;                   // ������
    Vector3 target;                 // ���� �׷��� ��ġ

    void Awake()
    {
        circleRenderer = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// ���� �޼ҵ�. ���� �ڵ����� ���� �׸�
    /// </summary>
    /// <param name="_target">���� �׷��� ��ǥ</param>
    /// <param name="_stpes">�� ���� ������ ������ ������</param>
    /// <param name="_radius">���� ������</param>
    public void Setting(Vector3 _target, int _stpes, float _radius)
    {
        target = _target;
        steps = _stpes;
        radius = _radius;

        DrawCircle();
    }

    /// <summary>
    /// ������ �׸��� �޼ҵ�
    /// </summary>
    void DrawCircle()
    {
        float angle = 0f;                       // ����

        circleRenderer.loop = true;             // ���� �������� ó���� ���� �̾��ش�

        circleRenderer.positionCount = steps;   // �� ���� ����
        float addAngle = 360f / steps;          // �� ����

        // �� ���и���
        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            // �ﰢ�Լ��� angle�� x, y ��ġ�� ����
            float x = radius * Trigonometrics.Cos(angle);
            float y = radius * Trigonometrics.Sin(angle);

            // �� ��ġ ����
            circleRenderer.SetPosition(currentStep, target + new Vector3(x, 0f, y));

            // ���� ������ ����
            angle += addAngle;
        }
    }
}
