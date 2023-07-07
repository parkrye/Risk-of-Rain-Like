using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾� ī�޶� �̵��� ���� ��ũ��Ʈ
/// </summary>
public class PlayerCameraController : MonoBehaviour
{
    PlayerDataModel playerDataModel;                            // �÷��̾� ������ ��

    [SerializeField] Vector2 pointerPos;                        // ������ ��ġ
    [SerializeField] Vector3 cameraOffset, downViewOffset, upViewOffset, defaultCameraOffset, closeCameraOffset;
                                                                // ī�޶� ����ġ, ���� ����ġ, ���� ����ġ, �⺻ ����ġ, ���� ����ġ
    [SerializeField] float xRotation;                           // x ȸ����
    [SerializeField] CinemachineVirtualCamera virtualCamera;    // ���� ī�޶�
    public Camera minimapCamera;                                // �̴ϸ� ī�޶�

    public Transform lookFromTransform, lookAtTransform;        // ī�޶� ���� ���� ��ġ, ���� �� ��ġ

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;               // Ŀ���� �����
        playerDataModel = GetComponent<PlayerDataModel>();      // ������ �� ����
        defaultCameraOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
                                                                // �ʱ� ����ġ ����
        cameraOffset = defaultCameraOffset;                     // ī�޶� ����ġ�� �ʱ� ����ġ��
        closeCameraOffset = cameraOffset * 0.1f;                // ���� ����ġ�� �ʱ� ����ġ�� 10���� 1
        downViewOffset = new Vector3(0f, 5f, -2.5f);            // ���� ����ġ
        upViewOffset = new Vector3(0f, 0f, -0.5f);              // ���� ����ġ
    }

    void Update()
    {
        // ESC UI ���� ��Ȳ�̶��
        if (!playerDataModel.onESC)
        {
            // ���� �������κ��� ī�޶� ��������
            Ray ray = new Ray(lookFromTransform.position, (virtualCamera.transform.position - lookFromTransform.position));
            // ���� �����Ѵٸ�
            if (Physics.Raycast(ray, Vector3.Distance(virtualCamera.transform.position, lookFromTransform.position), LayerMask.GetMask("Ground")))
            {
                // ī�޶� ����ġ�� �������� ������ �̵���Ų��
                cameraOffset = Vector3.Lerp(cameraOffset, closeCameraOffset, Time.deltaTime * playerDataModel.TimeScale);
            }
            // ���ٸ�
            else
            {
                // ī�޶� ����ġ�� �⺻���� ������ �̵���Ų��
                cameraOffset = Vector3.Lerp(cameraOffset, defaultCameraOffset, Time.deltaTime * playerDataModel.TimeScale);
            }
        }
    }

    void LateUpdate()
    {
        // ESC UI ���� ��Ȳ�̶��
        if (!playerDataModel.onESC)
        {
            // y�� ȸ���� ���� ĳ���� ȸ��
            transform.localEulerAngles += new Vector3(0f, pointerPos.x * playerDataModel.mouseSensivity * Time.deltaTime, 0f);

            // x�� ȸ���� ���� �� ����
            xRotation -= pointerPos.y * playerDataModel.mouseSensivity * 3f * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);

            // x�� ȸ���� ���� ī�޶� ���� ������ ȸ��
            lookFromTransform.localEulerAngles = new Vector3(xRotation, 0f, 0f);

            if (xRotation < 0f) // x�� ȸ������ 0���� �۴ٸ�(���� �ٶ󺻴ٸ�) ī�޶�� �����Ѵ�
                virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(cameraOffset, upViewOffset, (-xRotation * 0.0125f));
            else                // �ƴ϶��(�Ʒ��� �ٶ󺻴ٸ�) ī�޶�� �����Ѵ�
                virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(cameraOffset, downViewOffset, (xRotation * 0.0125f));
        }
    }

    /// <summary>
    /// ���콺 �Է¿� ���� �޼ҵ�
    /// </summary>
    /// <param name="inputValue"></param>
    void OnPointer(InputValue inputValue)
    {
        pointerPos = inputValue.Get<Vector2>();
    }
}
