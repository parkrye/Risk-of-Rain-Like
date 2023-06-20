using UnityEngine.Events;

/// <summary>
/// ���� ��/�ƴϿ� �˾�â
/// </summary>
public class YesNoPopUpUI : PopUpUI
{
    public UnityEvent YesEvent, NoEvent;

    protected override void Awake()
    {
        base.Awake();

        buttons["YesButton"].onClick.AddListener(YesButton);
        buttons["NoButton"].onClick.AddListener(NoButton);
    }

    /// <summary>
    /// �ؽ�Ʈ ������ ����
    /// </summary>
    /// <param name="textNum">0: ����, 1: yes��ư, 2: no��ư</param>
    public void SetText(int textNum, string text)
    {
        switch(textNum)
        {
            case 0:
                texts["DescText"].text = text;
                break;
            case 1:
                texts["YesText"].text = text;
                break;
            case 2:
                texts["NoText"].text = text;
                break;
        }
    }

    /// <summary>
    /// YesEvent�� �ߵ���Ű�� ����
    /// </summary>
    public void YesButton()
    {
        YesEvent?.Invoke();
        CloseUI();
    }

    /// <summary>
    /// NoEvent�� �ߵ���Ű�� ����
    /// </summary>
    public void NoButton()
    {
        NoEvent?.Invoke();
        CloseUI();
    }
}
