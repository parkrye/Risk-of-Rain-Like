using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPositionGimic : MonoBehaviour
{
    public GameObject Cover;    // ������ ���۵Ǹ� ������� ��

    public void SetGimic()
    {
        if (Cover != null)
        {
            Cover.SetActive(false);
        }
    }
}
