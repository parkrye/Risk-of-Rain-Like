using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPositionGimic : MonoBehaviour
{
    public GameObject Door;    // ������ ���۵Ǹ� ������� ��

    public void SetGimic()
    {
        if(Door != null)
        {
            Door.SetActive(false);
        }
    }
}
