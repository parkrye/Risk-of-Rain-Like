using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    EventSystem eventSystem;
    Canvas sceneCanvas, ingameCanvas, popUpCanvas;
    Stack<PopUpUI> popUpStack;

    void Awake()
    {
        eventSystem = GameManager.Resource.Instantiate<EventSystem>("UI/EventSystem");
        eventSystem.transform.parent = transform;
    }

    public void CreateInGameScene()
    {
        ingameCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        ingameCanvas.gameObject.name = "InGameCanvas";
        ingameCanvas.sortingOrder = 0;
    }

    public void CreateSceneCanvas()
    {
        sceneCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        sceneCanvas.gameObject.name = "SceneCanvas";
        sceneCanvas.sortingOrder = 1;
    }

    public void CreatePopupCanvas()
    {

        popUpCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        popUpCanvas.gameObject.name = "PopupCanvas";
        popUpCanvas.transform.SetParent(transform, false);
        popUpCanvas.sortingOrder = 5;

        popUpStack = new Stack<PopUpUI>();
    }

    // 이하 PopUpUI
    public T ShowPopupUI<T>(T popup) where T : PopUpUI
    {
        if (popUpStack.Count > 0)
        {
            popUpStack.Peek().gameObject.SetActive(false);
        }

        T ui = GameManager.Pool.GetUI<T>(popup);
        ui.transform.SetParent(popUpCanvas.transform, false);

        popUpStack.Push(ui);

        Time.timeScale = 0f;

        return ui;
    }

    public T ShowPopupUI<T>(string path) where T : PopUpUI
    {
        T uI = GameManager.Resource.Load<T>(path);
        return ShowPopupUI(uI);
    }

    public void ClosePopupUI()
    {
        GameManager.Pool.Release(popUpStack.Pop());

        if (popUpStack.Count == 0)
            Time.timeScale = 1f;

        if (popUpStack.Count > 0)
        {
            popUpStack.Peek().gameObject.SetActive(true);
        }
    }

    // 이하 InGameUI
    public T ShowInGameUI<T>(T inGameUI) where T : InGameUI
    {
        T ui = GameManager.Pool.GetUI(inGameUI);
        ui.transform.SetParent(ingameCanvas.transform, false);

        return ui;
    }

    public T ShowInGameUI<T>(string path) where T : InGameUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowInGameUI(ui);
    }

    public void CloseInGameUI(InGameUI inGameUI)
    {
        GameManager.Pool.Release(inGameUI);
    }

    // 이하 SceneUI
    public T ShowSceneUI<T>(T sceneUI) where T : SceneUI
    {
        T ui = GameManager.Pool.GetUI(sceneUI);
        ui.transform.SetParent(sceneCanvas.transform, false);

        return ui;
    }

    public T ShowSceneUI<T>(string path) where T : SceneUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowSceneUI(ui);
    }

    public void CloseSceneUI(SceneUI sceneUI)
    {
        GameManager.Pool.Release(sceneUI);
    }
}
