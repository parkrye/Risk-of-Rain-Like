using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    /// <summary>
    /// ������ ���� ��ã��
    /// </summary>
    /// <param name="start">���� ��ǥ</param>
    /// <param name="end">���� ��ǥ</param>
    /// <param name="distance">���� �Ÿ�</param>
    /// <returns>end�� ���� ���� ��ġ�� �̵� ��ǥ ����</returns>
    public static Stack<Vector3> PathFindingForAerial(Vector3 start, Vector3 end)
    {
        Stack<Vector3> answer = new Stack<Vector3>();   // ��ȯ ����
        answer.Push(end);     // �ʱⰪ = ��ǥ ��ǥ(�÷��̾� ��ǥ)

        Dictionary<Vector3, bool> visited = new Dictionary<Vector3, bool>();    // ��ǥ, ��� �湮 ���� ��ųʸ�
        Dictionary<Vector3, Node> nodes = new Dictionary<Vector3, Node>();      // ��ǥ, ��� ��ųʸ�
        PriorityQueue<Node, float> pq = new PriorityQueue<Node, float>();           // �� ���� �Ÿ��� ��带 ������ �켱���� ť

        // �ʱ� ��带 ����
        Node startNode = new Node();
        startNode.position = start;
        nodes.Add(startNode.position, startNode);
        pq.Enqueue(startNode, 0);

        // �켱���� ť�� ��尡 �ִٸ� �ݺ�
        while (pq.Count > 0)
        {
            Node node = pq.Dequeue();                // ���� ���
            if (!visited.ContainsKey(node.position)) // ���� �湮���� ���� ����� �湮 ��忡 �߰�
                visited.Add(node.position, true);
            else
                continue;

            // ���� ���� : ���� ��ǥ���� ��ǥ ��ǥ ���̿� ���� ����
            if (CheckPassable(node.position, end))
            {
                // ����� �θ� ���� ������ �ݺ�
                while (nodes.ContainsKey(node.parent))
                {
                    answer.Push(node.position); // ���� ����� ��ǥ�� �����ϰ�
                    node = nodes[node.parent];  // ����� �θ� ����
                }
                return answer;                  // ����� ������ ��ȯ(��ǥ ��ǥ => ���� ��ǥ => ... => �ʱ� ��ǥ)
            }

            // => �÷��̾� ���� ��ǥ�� Ž���ϵ��� ����
            // �� x, y, z�κ��� -1 ~ +1 ������ ��ǥ�� Ž��
            for (float x = -1f; x <= 1f; x += 1f)
            {
                for (float y = -1f; y <= 1f; y += 1f)
                {
                    for (float z = -1f; z <= 1f; z += 1f)
                    {
                        // 0, 0, 0�� ���� ��ǥ�̹Ƿ� �н�
                        if (x == y && y == z && z == 0f)
                            continue;

                        // ���� Ž���� ��ǥ
                        Vector3 findPosition = node.position + x * Vector3.right + y * Vector3.up + z * Vector3.forward;

                        // �̹� �湮�� ��ǥ��� �н�
                        if (visited.ContainsKey(findPosition))
                            continue;

                        // ���̿� ���� �ִٸ� �н�
                        if (!CheckPassable(node.position, findPosition))
                            continue;

                        float g = node.g + Mathf.Sqrt(x * x + y * y + z * z);    // �̵� �Ÿ� + �̵��� �Ÿ�
                        float h = Vector3.Distance(findPosition, end);      // ���� �Ÿ� = ������� ��ǥ���� ���� �Ÿ�

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
    /// <param name="distance">���� �Ÿ�</param>
    /// <returns>���� �Ÿ� �̳��� ���� �ִٸ� false, ���ٸ� true</returns>
    static bool CheckPassable(Vector3 start, Vector3 end)
    {
        Ray ray = new Ray(start, (end - start).normalized);
        if (Physics.Raycast(ray, Vector3.Distance(start, end), LayerMask.GetMask("Ground")))
        {
            return false;
        }
        return true;
    }
}
