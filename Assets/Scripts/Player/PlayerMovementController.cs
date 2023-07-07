using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾� ĳ���� �̵��� ���� ��ũ��Ʈ
/// </summary>
public class PlayerMovementController : MonoBehaviour
{
    PlayerDataModel playerDataModel;                        // �÷��̾� ������ ��
    [SerializeField] PlayerGroundChecker groundChecker;     // ���� ���� ������

    public Vector3 moveDir, dirModifier;                    // �̵� ����, �̵� ������
    [SerializeField] Vector3 curMoveVelocity, moveVelocity; // ���� �̵� ����
    [SerializeField] float slopeDegree, gravity;            // ���� �� �ִ� ���� ����, �߷°�
    [SerializeField] bool isSlope;                          // ���� ���� ���� �ִ���
    [SerializeField] AudioSource jumpAudio;                 // ������ �����
    RaycastHit slopeHit;                                    // ���� �˻�� �����ɽ�Ʈ �浹

    void Awake()
    {
        playerDataModel = GetComponent<PlayerDataModel>();
    }

    void Update()
    {
        if (!playerDataModel.onESC) // ESC UI�� ���ٸ�
        {
            CheckGround();          // ���� �˻�
            CheckSlope();           // ���� �˻�
            MoveCalculator();       // �̵� ����
        }
    }

    /// <summary>
    /// ���� �˻� �޼ҵ�
    /// </summary>
    void CheckGround()
    {
        // ���� �˻��κ��� ���鿡 �ִٰ� �ǴܵǸ�
        if (groundChecker.IsGround)
        {
            playerDataModel.jumpCount = playerDataModel.jumpLimit;  // ���� ī��Ʈ�� �ʱ�ȭ�ϰ�
            playerDataModel.animator.SetBool("IsGround", true);     // �ִϸ��̼��� ���� ���·� ����
            return;
        }
        playerDataModel.animator.SetBool("IsGround", false);        // �ƴ϶�� �ִϸ��̼��� ���� ���·� ����
    }

    /// <summary>
    /// �ٴڸ��� ���� ���͸� �̿��Ͽ� ���� ���ִ� �ٴ��� �������� Ȯ���ϴ� �޼ҵ�
    /// </summary>
    void CheckSlope()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);    // ��� ����. �Ǻ����� ��¦ ���κ��� �Ʒ���
        if(Physics.Raycast(ray, out slopeHit, 0.2f, LayerMask.GetMask("Ground")))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);   // ���� ���Ϳ� �ٴڸ��� ���� ���� ������ ���� ���Ͽ�
            isSlope = angle != 0f && angle <= slopeDegree;              // ���� 0�� �ƴϰ�, ���ص� ��� ���� ���϶�� ���鿡 ���ִ�
            return;
        }
        isSlope = false;                                                // ���� 0�̰ų�(����), ���ص� ��� ���� �ʰ���� ���鿡 ������ �ʴ�
    }

    /// <summary>
    /// ������ ���� ����� ���ϰ� �ʿ��� ���� ���� ������ ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="direction">�̵� ����</param>
    /// <returns>�������� ������ ����ȭ ����</returns>
    Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    void MoveCalculator()
    {
        // ���� ����
        // �߷� = ���� y �ӵ��� ���밪 + �߷�
        gravity = (playerDataModel.rb.velocity.y < 0 ? -playerDataModel.rb.velocity.y : playerDataModel.rb.velocity.y) - Physics.gravity.y * Time.deltaTime * playerDataModel.TimeScale;

        // ���� ���� �ִٸ�
        if (groundChecker.IsGround && isSlope)
        {
            curMoveVelocity = AdjustDirectionToSlope(moveDir);   // ���� �̵� ������ ���鿡 ������Ű��
            gravity = 0f;                                        // �߷��� 0����
        }
        else
            curMoveVelocity = moveDir;                           // �׷��� �ʴٸ� ���� �̵� ������ �̵� ���� �״��

        // ���� �̵� ���⿡�� y���� �����ϰ�, �÷��̾� ����, �ӵ��� ����
        curMoveVelocity = playerDataModel.MoveSpeed * (transform.right * curMoveVelocity.x + transform.forward * curMoveVelocity.z);

        // �̵� �ӷ�, �߷�, �̵� ����ġ ����
        moveVelocity = curMoveVelocity + Vector3.down * gravity + dirModifier;

        // �̵� ����ġ�� ���� 0�� ������ ����
        if (Vector3.SqrMagnitude(dirModifier) > 0f)
            dirModifier = Vector3.Lerp(dirModifier, Vector3.zero, Time.deltaTime * 5f);

        // �ִϸ��̼� �� ����
        playerDataModel.animator.SetFloat("Foward", Mathf.Lerp(playerDataModel.animator.GetFloat("Foward"), moveDir.z, Time.deltaTime * playerDataModel.TimeScale * 5f));
        playerDataModel.animator.SetFloat("Side", Mathf.Lerp(playerDataModel.animator.GetFloat("Side"), moveDir.x, Time.deltaTime * playerDataModel.TimeScale * 5f));
    }

    void FixedUpdate()
    {
        if (!playerDataModel.onESC) // ESC UI�� ���ٸ�
        {
            Move();                 // �̵��Ѵ�
        }
    }

    /// <summary>
    /// �̵� �޼ҵ�
    /// </summary>
    void Move()
    {
        playerDataModel.rb.velocity = moveVelocity;
    }

    /// <summary>
    /// �̵� �Է¿� ���� �޼ҵ�
    /// </summary>
    /// <param name="inputValue"></param>
    void OnMove(InputValue inputValue)
    {
        Vector2 tmp = inputValue.Get<Vector2>();

        // �÷��̾ �Է� ������ ���¶��, Vector3�� x,z�� ����
        if (playerDataModel.controllable)
            moveDir = new Vector3(tmp.x, 0f, tmp.y);
        // �ƴ϶�� ���� ����
        else
            moveDir = Vector3.zero;
    }

    /// <summary>
    /// ���� �Է¿� ���� �޼ҵ�
    /// </summary>
    /// <param name="inputValue"></param>
    void OnJump(InputValue inputValue)
    {
        // �ϴ� �÷��̾ �Է� ������ ������ ��
        if (playerDataModel.controllable)
        {
            // ���� ���� Ƚ���� �����ִٸ�
            if (playerDataModel.jumpCount > 0)
            {
                // �ϴ� ����
                if (playerDataModel.hero.Jump(inputValue.isPressed))
                {
                    jumpAudio.Play();               // ������� ����ϰ�
                    if(groundChecker.IsGround)      // ���鿡 �־��ٸ�
                        groundChecker.JumpReady();  // ���� ��⸦ ȣ��
                }
            }
            else
                playerDataModel.hero.Jump(false);   // �������� �ʴٸ� false ����
        }
    }
}
