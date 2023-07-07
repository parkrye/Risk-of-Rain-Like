using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// Ư�� CSW ������ �а� ���� ��ũ��Ʈ
/// </summary>
public static class CSVRW
{
    /// <summary>
    /// ��� ������ �д� �޼ҵ�
    /// </summary>
    /// <returns>��ųʸ� ������ ��� ����</returns>
    public static Dictionary<string, int> ReadCSV_Records()
    {
        Dictionary<string, int> answer = new();         // ������ ��ųʸ�

        TextAsset data = GameManager.Resource.Load<Object>("CSV/Records") as TextAsset;
                                                        // �ؽ�Ʈ�������� ��ȯ�� ��� ���� ������
        string[] texts = data.text.Split("\n");         // �����͸� �ٹٲ� ������ ������ ���ڿ� �迭

        for (int i = 0; i < texts.Length; i++)          // �� ���ڿ� ��ҿ� ���Ͽ�
        {
            if (texts[i].Length <= 1)                   // ���̰� 1 ���϶�� ��� ����
                break;                                  // (���� ��� ������ ��� �����Ϳ� �� ���ڿ� �� ���� �߰��Ǳ� ����)
            string[] line = texts[i].Split(",");        // �������� ������ �� ���ڿ���
            answer.Add(line[0], int.Parse(line[1]));    // Ű�� ������ ����
        }

        return answer;                                  // ������ ��ųʸ��� ��ȯ
    }

    /// <summary>
    /// ��� ������ �����ϴ� �޼ҵ�
    /// </summary>
    public static void WriteCSV_Records(Dictionary<string, int> data)
    {
        StringBuilder sb = new();                           // ������ ��Ʈ������
        string delimiter = ",";                             // ������
        foreach(KeyValuePair<string, int> pair in data)     // �� ������ �ֿ� ���Ͽ�
        {
            // Ű, ������, ���� ����
            sb.Append(pair.Key);
            sb.Append(delimiter);
            sb.AppendLine(pair.Value.ToString());
        }
        Stream fileStream = new FileStream("Assets/Resources/CSV/Records.csv", FileMode.Create, FileAccess.Write);
                                                                    // ������ �ּ�, ������ ���ų� ���� ����
        StreamWriter outStream = new(fileStream, Encoding.UTF8);    // ��� ����
        outStream.WriteLine(sb);                                    // ��Ʈ�������� ����
        outStream.Close();                                          // ��� ����
    }

    /// <summary>
    /// ���� ������ �д� �޼ҵ�
    /// </summary>
    /// <returns>��ųʸ� ������ ���� ����</returns>
    public static Dictionary<string, List<int>> ReadCSV_Achivements()
    {
        Dictionary<string, List<int>> answer = new();       // ������ ��ųʸ�

        TextAsset data = GameManager.Resource.Load<Object>("CSV/Achivements") as TextAsset;
                                                            // �ؽ�Ʈ�������� ��ȯ�� ���� ���� ������
        string[] texts = data.text.Split("\n");             // �����͸� �ٹٲ� ������ ������ ���ڿ� �迭

        for (int i = 0; i < texts.Length; i++)              // �� ���ڿ� ��ҿ� ���Ͽ�
        {
            if (texts[i].Length <= 1)                       // ���̰� 1 ���϶�� ��� ����
                break;                                      // (���� ��� ������ ��� �����Ϳ� �� ���ڿ� �� ���� �߰��Ǳ� ����)
            string[] line = texts[i].Split(",");            // �������� ������ ���ڿ�����
            List<int> list = new();
            for(int j = 1; j < line.Length; j++)
                list.Add(int.Parse(line[j]));
            answer.Add(line[0], list);                      // ù��°�� Ű��, �������� ����Ʈ�� ����
        }

        return answer;
    }

    /// <summary>
    /// ���� ������ �����ϴ� �޼ҵ�
    /// </summary>
    public static void WriteCSV_Achivements(Dictionary<string, List<int>> data)
    {
        StringBuilder sb = new();                               // ������ ��Ʈ������
        string delimiter = ",";                                 // ������
        foreach (KeyValuePair<string, List<int>> pair in data)  // �� ������ �ֿ� ���Ͽ�
        {
            sb.Append(pair.Key);                                // Ű
            for(int i = 0; i < data[pair.Key].Count; i++)
            {
                sb.Append(delimiter);                           // ������
                sb.Append(pair.Value[i]);                       // �� ���� �ݺ�
            }
            sb.AppendLine();                                    // �ٹٲ��� �����ϱ⸦ �ݺ�
        }
        Stream fileStream = new FileStream("Assets/Resources/CSV/Achivements.csv", FileMode.Create, FileAccess.Write);
        // ������ �ּ�, ������ ���ų� ���� ����
        StreamWriter outStream = new(fileStream, Encoding.UTF8);    // ��� ����
        outStream.WriteLine(sb);                                    // ��Ʈ�������� ����
        outStream.Close();                                          // ��� ����
    }
}
