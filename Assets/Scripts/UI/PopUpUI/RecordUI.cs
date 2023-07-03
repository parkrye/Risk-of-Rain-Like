using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordUI : PopUpUI
{
    protected override void Awake()
    {
        base.Awake();
        texts["StageText"].text += ((int)GameManager.Data.Records["Stage"]).ToString();
        texts["TimeText"].text += ((int)GameManager.Data.Records["Time"]).ToString();
        switch (GameManager.Data.Records["Difficulty"])
        {
            case 1f:
                texts["DifficultyText"].text += "Easy";
                break;
            case 2:
                texts["DifficultyText"].text += "Normal";
                break;
            case 3:
                texts["DifficultyText"].text += "Hard";
                break;
        }
        texts["KillText"].text += ((int)GameManager.Data.Records["Kill"]).ToString();
        texts["DamageText"].text += ((int)GameManager.Data.Records["Damage"]).ToString();
        texts["HitText"].text += ((int)GameManager.Data.Records["Hit"]).ToString();
        texts["HealText"].text += ((int)GameManager.Data.Records["Heal"]).ToString();
        texts["MoneyText"].text += ((int)GameManager.Data.Records["Money"]).ToString();
        texts["CostText"].text += ((int)GameManager.Data.Records["Cost"]).ToString();

        switch (GameManager.Data.Player.heroNum)
        {
            case 0:
                texts["HeroText"].text += "�ü�";
                break;
            case 1:
                texts["HeroText"].text += "����";
                break;
            case 2:
                texts["HeroText"].text += "������";
                break;
        }
        texts["LevelText"].text += GameManager.Data.Player.LEVEL;
        foreach (KeyValuePair<ItemData, int> item in GameManager.Data.Player.Inventory.GetInventory)
        {
            Image itemIcon = GameManager.Resource.Instantiate<Image>("UI/ItemImage", images["Items"].transform);
            itemIcon.sprite = item.Key.ItemIcon;
            if(item.Value > 1)
            {
                itemIcon.GetComponentInChildren<TextMeshProUGUI>().text = item.Value.ToString();
                break;
            }
        }

        buttons["RetryButton"].onClick.AddListener(RetryButton);
        buttons["TitleButton"].onClick.AddListener(TitleButton);
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    void RetryButton()
    {
        GameManager.Scene.LoadScene("ReadyScene");
        GameManager.ResetSession();
    }

    void TitleButton()
    {
        GameManager.Scene.LoadScene("TitleScene");
    }
}
