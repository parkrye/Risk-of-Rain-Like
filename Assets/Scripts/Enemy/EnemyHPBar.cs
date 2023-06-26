using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.GetComponent<RectTransform>().localPosition = new Vector3(0f, enemy.enemyData.Size * 1.5f, 0f);
        slider.GetComponent<RectTransform>().sizeDelta = new Vector2(enemy.enemyData.Size, enemy.enemyData.Size * 0.5f);
        slider.maxValue = enemy.enemyData.MaxHP;
        slider.value = enemy.hp;
        enemy.OnHPEvent.AddListener(SetValue);
    }

    void OnEnable()
    {
        slider.value = enemy.hp;
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}