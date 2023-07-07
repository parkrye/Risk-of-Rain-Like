using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ĳ������ �ൿ �� ��ȣ�ۿ뿡 ���� ��ũ��Ʈ
/// </summary>
public class PlayerActionController : MonoBehaviour
{
    PlayerDataModel playerDataModel;                // �����͸�

    public Transform AttackTransform;               // ���� ��ġ
    public Transform lookAtTransform, lookFromTransform, interactTransform, closeAttackTransform;
                                                    // ī�޶� ���� �� ��ġ, ī�޶� ���� ���� ��ġ, ��ȣ�ۿ� ��ġ, �������� ��ġ
    public float closeAttackRange, interactRange;   // �������� ��Ÿ�, ��ȣ�ۿ� ��Ÿ�
    float cosResult;                                // ���� Ȯ���� ���� �ڻ��� �����
    ESCUI escUI;                                    // ESC �Է½� UI

    void Awake()
    {
        playerDataModel = GetComponent<PlayerDataModel>();
        cosResult = Trigonometrics.Cos(60f);
    }

    /// <summary>
    /// �ֹ��� ���ݿ� ���� �޼ҵ�
    /// </summary>
    /// <param name="inputValue"></param>
    void OnAction1(InputValue inputValue)
    {
        if(playerDataModel.hero.Action(0, inputValue.isPressed))
        {

        }
    }

    /// <summary>
    /// �������� ���ݿ� ���� �޼ҵ�
    /// </summary>
    /// <param name="inputValue"></param>
    void OnAction2(InputValue inputValue)
    {
        if (playerDataModel.hero.Action(1, inputValue.isPressed))
        {

        }
    }

    /// <summary>
    /// ������ų ��뿡 ���� �޼ҵ�
    /// </summary>
    /// <param name="inputValue"></param>
    void OnAction3(InputValue inputValue)
    {
        if (playerDataModel.hero.Action(2, inputValue.isPressed))
        {

        }
    }

    /// <summary>
    /// �ֿ佺ų ��뿡 ���� �޼ҵ�
    /// </summary>
    /// <param name="inputValue"></param>
    void OnAction4(InputValue inputValue)
    {
        if (playerDataModel.hero.Action(3, inputValue.isPressed))
        {

        }
    }

    /// <summary>
    /// ��ȣ�ۿ뿡 ���� �޼ҵ�
    /// </summary>
    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(interactTransform.position, interactRange);    // ��ȣ�ۿ� �������κ��� ��ȣ�ۿ� �Ÿ� ���� ������Ʈ�鿡 ���Ͽ�
        Vector3 playerPosition = transform.position;
        playerPosition.y = 0f;                                                                      // ���̸� ������ �÷��̾� ��ġ��
        Vector3 playerLook = new Vector3(transform.forward.x, 0f, transform.forward.z);             // ���̸� ������ �÷��̾� �ü� ������ �̿��Ͽ�
        for(int i = 0; i < colliders.Length; i++)
        {
            IInteractable interactable = colliders[i].GetComponent<IInteractable>();                // ���� ��ȣ�ۿ� ������ ������Ʈ�̰�
            if (interactable is null)
                continue;
            Vector3 colliderPosition = colliders[i].transform.position;
            colliderPosition.y = 0f;                                                                // ���̸� ������ ������Ʈ�� ��ġ�� ���Ͽ�
            Vector3 dirTarget = (colliderPosition - playerPosition).normalized;
            if (Vector3.Dot(playerLook, dirTarget) < cosResult)
                continue;

            interactable?.Interact();                                                               // �÷��̾��� ���濡 �ִٸ� ��ȣ�ۿ��Ѵ�
        }
    }

    /// <summary>
    /// ��ȣ�ۿ� �Է¿� ���� �޼ҵ�
    /// </summary>
    /// <param name="inputValue"></param>
    void OnInteract(InputValue inputValue)
    {
        Interact();
    }

    /// <summary>
    /// ESC �Է¿� ���� �޼ҵ�
    /// </summary>
    /// <param name="inputValue"></param>
    void OnESC(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            // ���� ���� ���̰�, �÷��� ���� ���
            if (GameManager.Scene.CurScene.name.StartsWith("LevelScene") && GameManager.Scene.ReadyToPlay)
            {
                // ESC UI�� ���ٸ� ESC UI ����
                if (!playerDataModel.onESC)
                {
                    escUI = GameManager.UI.ShowPopupUI<ESCUI>("UI/ESCUI");
                }
                // �ִٸ� ESC UI ����
                else
                {
                    escUI?.CloseUI();
                }
            }
        }
    }

    /// <summary>
    /// Tab �Է¿� ���� �޼ҵ�
    /// </summary>
    /// <param name="inputValue"></param>
    void OnTab(InputValue inputValue)
    {
        // ���� �� ���콺�� ���̰�
        if (inputValue.isPressed)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        // �� �� ���콺�� �����
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// ����� ��� �޼ҵ�
    /// �׶��׶� �ʿ��� ����� �ۼ� �� �׽�Ʈ
    /// </summary>
    void OnDrawGizmos()
    {
        if (playerDataModel.onGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(closeAttackTransform.position, closeAttackRange);
        }
    }
}
