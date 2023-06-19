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
