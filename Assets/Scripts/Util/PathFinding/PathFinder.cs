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
    public static Stack<Vector3> PathFindingForAerial(Transform start, Transform end, float distance)
    {
        Stack<Vector3> answer = new Stack<Vector3>();   // ��ȯ ����
        answer.Push(end.position);                      // �ʱⰪ = ��ǥ ��ǥ(�÷��̾� ��ǥ)

        // ���� �÷��̾�� ���ʹ� ���̿� ���� �ִٸ�
        if (!CheckPassable(start.position, end.position, distance))
        {
            // x, y, z �Ÿ� ����. �ּ� 1
            int xDiff = (int)Mathf.Abs(start.position.x - end.position.x);
            if (xDiff == 0) xDiff = 1;
            int yDiff = (int)Mathf.Abs(start.position.y - end.position.y);
            if (yDiff == 0) yDiff = 1;
            int zDiff = (int)Mathf.Abs(start.position.z - end.position.z);
            if (zDiff == 0) zDiff = 1;

            Dictionary<Vector3, bool> visited = new Dictionary<Vector3, bool>();    // ��ǥ, ��� �湮 ���� ��ųʸ�
            Dictionary<Vector3, Node> nodes = new Dictionary<Vector3, Node>();      // ��ǥ, ��� ��ųʸ�
            PriorityQueue<Node, int> pq = new PriorityQueue<Node, int>();           // �� ���� �Ÿ��� ��带 ������ �켱���� ť

            // �ʱ� ��带 ����
            Node startNode = new Node();
            startNode.position = start.position;
            nodes.Add(startNode.position, startNode);
            pq.Enqueue(startNode, 0);

            // �켱���� ť�� ��尡 �ִٸ� ��� �ݺ�
            while(pq.Count > 0)
            {
                Node node = pq.Dequeue();               // ���� ���
                if(!visited.ContainsKey(node.position)) // ���� �湮���� ���� ����� �湮 ��忡 �߰�
                    visited.Add(node.position, true);

                // ���� ���� 1 : ��ǥ ��ǥ���� �Ÿ��� ���� �Ÿ� ����
                if (Vector3.Distance(node.position, end.position) <= distance)
                {
                    // ����� �θ� ���� ������ �ݺ�
                    while (node.parent != null)
                    {
                        answer.Push(node.position); // ���� ����� ��ǥ�� �����ϰ�
                        if (nodes.ContainsKey(node.parent))
                            node = nodes[node.parent];  // ����� �θ� ����
                        else
                            break;
                    }
                    return answer;                  // ����� ������ ��ȯ(��ǥ ��ǥ => ���� ��ǥ => ... => �ʱ� ��ǥ)
                }

                // ���� ���� 2 : ���� ��ǥ���� ��ǥ ��ǥ ���̿� ���� ����
                if (CheckPassable(node.position, end.position, distance))
                {
                    // ����� �θ� ���� ������ �ݺ�
                    while (node.parent != null)
                    {
                        answer.Push(node.position); // ���� ����� ��ǥ�� �����ϰ�
                        node = nodes[node.parent];  // ����� �θ� ����
                    }
                    return answer;                  // ����� ������ ��ȯ(��ǥ ��ǥ => ���� ��ǥ => ... => �ʱ� ��ǥ)
                }

                // �� x, y, z�κ��� -1 ~ +1 ������ ��ǥ�� Ž��
                for (int x = -1; x <= 1; x++)
                {
                    for(int y = -1; y <= 1; y++)
                    {
                        for(int  z = -1; z <= 1; z++)
                        {
                            // 0, 0, 0�� ���� ��ǥ�̹Ƿ� �н�
                            if (x == y && y == z && z == 0)
                                continue;

                            // ���� Ž���� ��ǥ
                            Vector3 findPosition = node.position + xDiff * x * Vector3.right + yDiff * y * Vector3.up + zDiff * z * Vector3.forward;

                            // �̹� �湮�� ��ǥ��� �н�
                            if (visited.ContainsKey(findPosition))
                                continue;

                            int g = node.g + Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z);    // �̵� �Ÿ� + �̵��� x, y, z �Ÿ�
                            int h = (int)Vector3.Distance(findPosition, end.position);      // ���� �Ÿ� = ������� ��ǥ���� ���� �Ÿ�

                            // �� ��� ����
                            Node findNode = new Node(findPosition, node.position, g, h);
                            if (!nodes.ContainsKey(findPosition))           // ���� �� ��尡 ó�� �߰��� �����
                            {
                                nodes.Add(findPosition, findNode);          // ��� ��Ͽ� �� ��带 �߰��ϰ�
                                pq.Enqueue(findNode, findNode.f);           // ť�� �߰�
                            }
                            else if(nodes[findPosition].f > findNode.f)     // ���� �� ��尡 ������ �־�����, �������� �� ����Ÿ��� ���ٸ�
                            {
                                nodes[findPosition] = findNode;             // ��� ����� �� ���� �����ϰ�
                                pq.Enqueue(findNode, findNode.f);           // ť�� �߰�
                            }
                        }
                    }
                }
            }
        }
        // ���� �����ٸ� �״�� ��ȯ
        else
        {
            return answer;
        }

        // Ž�� ��� ���� ã�� ���ߴٸ� �÷��̾��� �ڷ� �̵���Ű��
        TeleportToBackside(start, end, distance);
        return answer;  // �״�� ��ȯ
    }

    /// <summary>
    /// ������ ����ü
    /// ��ǥ, �θ�(�� ��带 ����Ų ����� ��ǥ), ������� �Ÿ�, ����Ǵ� �������� �Ÿ�, �� ���� �Ÿ��� ����
    /// </summary>
    struct Node
    {
        public Vector3 position;
        public Vector3 parent;

        public int g;
        public int h;
        public int f;

        public Node(Vector3 _position, Vector3 _parent, int _g, int _h)
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
    static bool CheckPassable(Vector3 start, Vector3 end, float distance)
    {
        if (Physics.Raycast(start, end - start, distance, LayerMask.GetMask("Ground")))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// �÷��̾��� �ڷ� ���ʹ̸� �̵���Ų��
    /// </summary>
    /// <param name="enemy">���ʹ� ��ġ</param>
    /// <param name="player">�÷��̾� ��ġ</param>
    /// <param name="distance">���ʹ� ���� �Ÿ�</param>
    static void TeleportToBackside(Transform enemy, Transform player, float distance)
    {
        RaycastHit hit;
        if(Physics.Raycast(player.position, -player.forward, out hit, distance, LayerMask.GetMask("Ground")))
        {
            enemy.position = hit.point;
        }
        else
        {
            enemy.position = player.position - player.forward * distance;
        }
    }
}
