using System.Collections;
using UnityEngine;

public class MainSceneUI : SceneUI
{
    bool click;

    void OnEnable()
    {
        Initialize();
    }

    public override void Initialize()
    {
        buttons["Start"].onClick.AddListener(OnStartButton);
        buttons["Option"].onClick.AddListener(OnOptionButton);
        buttons["Quit"].onClick.AddListener(OnQuitButton);
        buttons["Achivement"].onClick.AddListener(OnAchivementButton);
        buttons["Reset"].onClick.AddListener(OnResetButton);
        click = false;
    }

    IEnumerator StartButtonRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        GameManager.Scene.LoadScene("ReadyScene");
    }

    void OnStartButton()
    {
        if (!click)
        {
            click = true;
            buttons["Start"].GetComponent<Animator>().SetTrigger("Click");
            StartCoroutine(StartButtonRoutine());
        }
    }

    IEnumerator OptionButtonRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        GameManager.UI.ShowPopupUI<PopUpUI>("UI/OptionUI");
    }

    void OnOptionButton()
    {
        buttons["Option"].GetComponent<Animator>().SetTrigger("Click");
        StartCoroutine(OptionButtonRoutine());
    }

    IEnumerator QuitButtonRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        Application.Quit();
    }

    void OnQuitButton()
    {
        if (!click)
        {
            click = true;
            buttons["Quit"].GetComponent<Animator>().SetTrigger("Click");
            StartCoroutine(QuitButtonRoutine());
        }
    }

    void OnAchivementButton()
    {
        if (!click)
        {
            click = true;
            GameManager.Scene.LoadScene("AchivementScene");
        }
    }

    void OnResetButton()
    {
        YesNoPopUpUI ui = GameManager.UI.ShowPopupUI<YesNoPopUpUI>("UI/YesNoUI");
        ui.SetText(0, "�����͸� �ʱ�ȭ�Ͻðڽ��ϱ�?");
        ui.SetText(1, "��");
        ui.SetText(2, "�ƴϿ�");
        ui.YesEvent.AddListener(() => { GameManager.Data.ResetRecords(); });
    }
}
