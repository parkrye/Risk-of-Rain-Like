using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDrawer : MonoBehaviour
{
    LineRenderer circleRenderer;
    public int steps;
    public float radius;

    void Awake()
    {
        circleRenderer = GetComponent<LineRenderer>();
    }

    public void Setting(int _stpes, float _radius)
    {
        steps = _stpes;
        radius = _radius;
    }

    /// <summary>
    /// ������ �׸��� �޼ҵ�
    /// </summary>
    public void DrawCircle()
    {
        // ���� ���� ����
        circleRenderer.positionCount = steps;

        // �� ���и���
        for(int currentStep = 0; currentStep < steps; currentStep++)
        {
            // �� ��° ��������(����)
            float circumferenceProgress = (float)currentStep / steps;

            // ���� �ѷ� �� ���� ����
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            // ���� �������� x, y�� ��ġ
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            // ���� �׷��� �� ��ġ
            float x = xScaled * radius;
            float y = yScaled * radius;

            // �׷��� �� ��ġ ��ǥȭ
            Vector3 currentPosition = new Vector3(x, 0f, y);

            // ������ �׸���
            circleRenderer.SetPosition(currentStep, transform.position + currentPosition);
        }
    }
}
