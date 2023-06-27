using UnityEngine;

public class EnemySummon : MonoBehaviour
{
    /// <summary>
    /// ������ ��ġ�� ������ ���ʹ̸� ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="location">�߽� ��ġ</param>
    /// <param name="distance">���� �Ÿ�</param>
    /// <returns>��ȯ�� ���ʹ� ������Ʈ</returns>
    public static GameObject RandomLocationSummon(Transform location, float distance)
    {
        EnemyData[] enemyDatas = GameManager.Resource.LoadAll<EnemyData>("Enemy");

        Vector3 spawnPosition = Vector3.zero;
        float remainDistance = distance;

        spawnPosition.x = Random.Range(remainDistance * 0.2f, remainDistance * 0.8f);
        remainDistance -= spawnPosition.x;

        spawnPosition.y = Random.Range(remainDistance * 0.2f, remainDistance * 0.8f);
        remainDistance -= spawnPosition.y;

        spawnPosition.z = remainDistance;

        GameObject enemy = GameManager.Resource.Instantiate(enemyDatas[Random.Range(0, enemyDatas.Length)].enemy, location.position + spawnPosition, Quaternion.identity, true);
        return enemy;
    }

    /// <summary>
    /// ������ ��ġ�� Ư���� ���ʹ̸� ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="location">�߽� ��ġ</param>
    /// <param name="distance">���� �Ÿ�</param>
    /// <param name="path">���ʹ� �������� ���</param>
    /// <returns>��ȯ�� ���ʹ� ������Ʈ</returns>
    public static GameObject RandomLocationSummon(Transform location, float distance, string path)
    {
        Vector3 spawnPosition = Vector3.zero;
        float remainDistance = distance;

        spawnPosition.x = Random.Range(remainDistance * 0.2f, remainDistance * 0.8f);
        remainDistance -= spawnPosition.x;

        spawnPosition.y = Random.Range(remainDistance * 0.2f, remainDistance * 0.8f);
        remainDistance -= spawnPosition.y;

        spawnPosition.z = remainDistance;

        GameObject enemy = GameManager.Resource.Instantiate(GameManager.Resource.Load<EnemyData>(path).enemy, location.position + spawnPosition, Quaternion.identity, true);
        return enemy;
    }

    /// <summary>
    /// Ư���� ��ġ�� ������ ���ʹ̸� ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="location">��ȯ ��ġ</param>
    /// <returns>��ȯ�� ���ʹ� ������Ʈ</returns>
    public static GameObject TargetLocationSummon(Transform location)
    {
        EnemyData[] enemyDatas = GameManager.Resource.LoadAll<EnemyData>("Enemy");
        GameObject enemy = GameManager.Resource.Instantiate(enemyDatas[Random.Range(0, enemyDatas.Length)].enemy, location.position, Quaternion.identity, true);
        return enemy;
    }

    /// <summary>
    /// Ư���� ��ġ�� Ư���� ���ʹ̸� ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="location">��ȯ ��ġ</param>
    /// <param name="path">��ȯ�� ���ʹ� �������� ���</param>
    /// <returns>��ȯ�� ���ʹ� ������Ʈ</returns>
    public static GameObject TargetLocationSummon(Transform location, string path)
    {
        GameObject enemy = GameManager.Resource.Instantiate(GameManager.Resource.Load<EnemyData>(path).enemy, location.position, Quaternion.identity, true);
        return enemy;
    }

    /// <summary>
    /// Ư���� ��ġ�� Ư���� ���ʹ̸� ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="location">��ȯ ��ġ</param>
    /// <param name="enemyData">��ȯ�� ���ʹ� ������</param>
    /// <returns>��ȯ�� ���ʹ� ������Ʈ</returns>
    public static GameObject TargetLocationSummon(Transform location, EnemyData enemyData)
    {
        GameObject enemy = GameManager.Resource.Instantiate(enemyData.enemy, location.position, Quaternion.identity, true);
        return enemy;
    }
}
