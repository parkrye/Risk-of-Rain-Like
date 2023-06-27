using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    /// <summary>
    /// ������ ���� ��ã��
    /// </summary>
    /// <param name="start">���� Ʈ������. start�� end�� �ٶ󺸰� �־�� ��</param>
    /// <param name="end">���� ��ǥ</param>
    /// <returns>�̵� ��ǥ ����</returns>
    public static Stack<Vector3> PathFinding(Transform start, Vector3 end, float radius)
    {
        Stack<Vector3> answer = new Stack<Vector3>();   // ��ȯ ����

        Dictionary<Vector3, bool> visited = new Dictionary<Vector3, bool>();    // ��ǥ, ��� �湮 ���� ��ųʸ�
        Dictionary<Vector3, Node> nodes = new Dictionary<Vector3, Node>();      // ��ǥ, ��� ��ųʸ�
        PriorityQueue<Node, float> pq = new PriorityQueue<Node, float>();           // �� ���� �Ÿ��� ��带 ������ �켱���� ť
        float moveModifier = 1f;
        int counter = 0;

        // �ʱ� ��带 ����
        Node startNode = new Node();
        startNode.position = start.position + Vector3.up;
        nodes.Add(startNode.position, startNode);
        pq.Enqueue(startNode, 0);

        // �켱���� ť�� ��尡 �ִٸ� �ݺ�
        while (pq.Count > 0 && counter++ < 100)
        {
            Node node = pq.Dequeue();                // ���� ���
            if (visited.ContainsKey(node.position))
                continue;
            visited.Add(node.position, true);

            // ���� ���� : ���� ��ǥ���� ��ǥ ��ǥ ���̿� ���� ����
            if (CheckPassable(node.position, end, radius))
            {
                // ����� �θ� ���� ������ �ݺ�
                while (nodes.ContainsKey(node.parent))
                {
                    answer.Push(node.position); // ���� ����� ��ǥ�� �����ϰ�
                    node = nodes[node.parent];  // ����� �θ� ����
                }
                return answer;                  // ����� ������ ��ȯ(��ǥ ��ǥ => ���� ��ǥ => ... => �ʱ� ��ǥ)
            }

            // �� 17���� Ž��
            // �� x, y�κ��� -1 ~ +1 ������ ��ǥ�� Ž��
            // z�� -1�� �Ǵ� ���� �÷��̾�κ��� �־����� ����̹Ƿ� ����(�� ���ӿ����� ȸ���ؾ� �� ������ �������� ������� ����)
            for (int x = -1; x <= 1; x += 1)
            {
                for (int y = -1; y <= 1; y += 1)
                {
                    for (int z = 0; z <= 1; z += 1)
                    {
                        if (x == y && y == z && z == 0)
                            continue;

                        // ���� Ž���� ��ǥ
                        Vector3 findPosition = node.position + (x * start.right + y * start.up + z * start.forward) * moveModifier;

                        // �̹� �湮�� ��ǥ��� �н�
                        if (visited.ContainsKey(findPosition))
                            continue;

                        // ���̿� ���� �ִٸ� �н�
                        if (!CheckPassable(node.position, findPosition, radius, (Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z))))
                            continue;

                        float g = node.g + x * x + y * y + z * z;                // �̵� �Ÿ� + �̵��� �Ÿ� (�뷫)
                        float h = Vector3.SqrMagnitude(end - findPosition);      // ���� �Ÿ� = ������� ��ǥ���� ���� �Ÿ� (�뷫)

                        // �� ��� ����
                        Node findNode = new Node(findPosition, node.position, g, h);
                        if (!nodes.ContainsKey(findPosition))           // ���� �� ��尡 ó�� �߰��� �����
                        {
                            nodes.Add(findPosition, findNode);          // ��� ��Ͽ� �� ��带 �߰��ϰ�
                            pq.Enqueue(findNode, findNode.f);           // ť�� �߰�
                        }
                        else if (nodes[findPosition].f > findNode.f)     // ���� �� ��尡 ������ �־�����, �������� �� ����Ÿ��� ���ٸ�
                        {
                            nodes[findPosition] = findNode;             // ��� ����� �� ���� �����ϰ�
                            pq.Enqueue(findNode, findNode.f);           // ť�� �߰�
                        }
                    }
                }
            }
        }

        // ���� ã�� ���ߴٸ� �״�� ��ȯ
        return answer;
    }

    /// <summary>
    /// ������ ����ü
    /// ��ǥ, �θ�(�� ��带 ����Ų ����� ��ǥ), ������� �Ÿ�, ����Ǵ� �������� �Ÿ�, �� ���� �Ÿ��� ����
    /// </summary>
    struct Node
    {
        public Vector3 position;
        public Vector3 parent;

        public float g;
        public float h;
        public float f;

        public Node(Vector3 _position, Vector3 _parent, float _g, float _h)
        {
            position = _position;
            parent = _parent;
            g = _g;
            h = _h;
            f = g + h;
        }
    }

    /// <summary>
    /// ������ �� ����
    /// </summary>
    /// <param name="start">���� ��ǥ</param>
    /// <param name="end">��ǥ ��ǥ</param>
    /// <param name="radius">���� ũ��</param>
    /// <returns>���� �Ÿ� �̳��� ���� �ִٸ� false, ���ٸ� true</returns>
    static bool CheckPassable(Vector3 start, Vector3 end, float radius, int sum = 0)
    {
        float distance;
        if (sum > 0)
        {
            if (sum == 1)
                distance = 1f;
            else if (sum == 2)
                distance = 1.4f;
            else
                distance = 1.7f;
        }
        else
        {
            distance = Vector3.Distance(start, end);
        }
        // x, y, z ��� 1�� �̵��� ����� ���̰� �� 1.7f
        if (Physics.SphereCast(start, radius, (end - start).normalized, out _, distance, LayerMask.GetMask("Ground")))
        {
            return false;
        }
        return true;
    }
}
