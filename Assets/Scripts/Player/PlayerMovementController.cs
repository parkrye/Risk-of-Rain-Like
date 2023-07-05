using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    PlayerDataModel playerDataModel;
    [SerializeField] PlayerGroundChecker groundChecker;

    public Vector3 moveDir, dirModifier;
    [SerializeField] Vector3 curVelocity;
    [SerializeField] float slopeDegree;
    [SerializeField] bool isSlope;
    [SerializeField] AudioSource jumpAudio;
    RaycastHit slopeHit;

    void Awake()
    {
        playerDataModel = GetComponent<PlayerDataModel>();
    }

    void Update()
    {
        if (!playerDataModel.onESC)
        {
            CheckGround();
            CheckSlope();
        }
    }

    void CheckGround()
    {
        if (groundChecker.IsGround)
        {
            playerDataModel.jumpCount = playerDataModel.jumpLimit;
            playerDataModel.animator.SetBool("IsGround", true);
            return;
        }
        playerDataModel.animator.SetBool("IsGround", false);
    }

    /// <summary>
    /// �ٴڸ��� ���� ���͸� �̿��Ͽ� ���� ���ִ� �ٴ��� �������� Ȯ���ϴ� �޼ҵ�
    /// </summary>
    void CheckSlope()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if(Physics.Raycast(ray, out slopeHit, 0.2f, LayerMask.GetMask("Ground")))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);   // ���� ���Ϳ� �ٴڸ��� ���� ���� ������ ���� ���Ͽ�
            isSlope = angle != 0f && angle <= slopeDegree;              // ���� 0�� �ƴϰ�(����), ���ص� ��� ���� ���϶�� ����
            return;
        }
        isSlope = false;
    }

    /// <summary>
    /// ������ ���� ����� ���ϰ� �ʿ��� ���� ���� ������ ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    void FixedUpdate()
    {
        if (!playerDataModel.onESC)
        {
            Move();
        }
    }

    void Move()
    {
        float animatorFoward = playerDataModel.animator.GetFloat("Foward");
        float animatorSide = playerDataModel.animator.GetFloat("Side");

        // ���� ����
        float gravity = (playerDataModel.rb.velocity.y < 0 ? -playerDataModel.rb.velocity.y : playerDataModel.rb.velocity.y) - Physics.gravity.y * Time.deltaTime * playerDataModel.TimeScale;
        if(groundChecker.IsGround && isSlope)
        {
            curVelocity = AdjustDirectionToSlope(moveDir);
            gravity = 0f;
        }
        else
            curVelocity = moveDir;
        curVelocity = transform.right * curVelocity.x + transform.forward * curVelocity.z;

        // �̵�
        Debug.Log(gravity);
        playerDataModel.rb.velocity = playerDataModel.MoveSpeed * playerDataModel.TimeScale * curVelocity + Vector3.down * gravity + dirModifier;
        if (Vector3.SqrMagnitude(dirModifier - Vector3.zero) > 0.1f)
            dirModifier = Vector3.Lerp(dirModifier, Vector3.zero, Time.deltaTime * 5f);

        // �ִϸ��̼�
        playerDataModel.animator.SetFloat("Foward", Mathf.Lerp(animatorFoward, moveDir.z, Time.deltaTime * playerDataModel.TimeScale * 5f));
        playerDataModel.animator.SetFloat("Side", Mathf.Lerp(animatorSide, moveDir.x, Time.deltaTime * playerDataModel.TimeScale * 5f));
    }

    void OnMove(InputValue inputValue)
    {
        Vector2 tmp = inputValue.Get<Vector2>();

        if (playerDataModel.controllable)
            moveDir = new Vector3(tmp.x, 0f, tmp.y);
        else
            moveDir = Vector3.zero;
    }

    void OnJump(InputValue inputValue)
    {
        if (playerDataModel.controllable)
        {
            if (playerDataModel.jumpCount > 0)
            {
                if (playerDataModel.hero.Jump(inputValue.isPressed))
                {
                    jumpAudio.Play();
                    if(groundChecker.IsGround)
                        groundChecker.JumpReady();
                }
            }
            else
            {
                playerDataModel.hero.Jump(false);
            }
        }
    }
}
