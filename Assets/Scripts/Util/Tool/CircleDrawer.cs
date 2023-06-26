using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDrawer : MonoBehaviour
{
    LineRenderer circleRenderer;
    int steps;
    float radius;
    Vector3 target;

    void Awake()
    {
        circleRenderer = GetComponent<LineRenderer>();
    }

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
        // ����
        float angle = 0f;

        // �������� ó���� ���� �̾��ش�
        circleRenderer.loop = true;

        // ���� ���� ����
        circleRenderer.positionCount = steps;

        // �� ���и���
        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            // �ﰢ�Լ��� angle�� ���� x, y ��ġ�� ����
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            circleRenderer.SetPosition(currentStep, target + new Vector3(x, y, 0f));

            angle += 2f * Mathf.PI / steps;
        }
    }
}
